using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace GameProject.Screens
{
    // This screen implements the actual game logic. 
    public class GameplayScreen : GameScreen
    {
        private const int OBSTACLE_COUNT = 10;
        private ContentManager _content;

        private SoundEffect deathSound;

        private ObstacleSprite[] obstacles;
        private PlayerSprite playerSprite;
        private Stopwatch _timer;
        private double score;
        private SpriteFont bangers;
        private Song backgroundMusic;

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (this._content == null)
                this._content = new ContentManager(ScreenManager.Game.Services, "Content");
            ScreenManager.ToggleSparks();

            Vector2 position = new Vector2(
                ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2
                );
            System.Random random = new System.Random();

            playerSprite = new PlayerSprite(ScreenManager.GraphicsDevice);
            obstacles = new ObstacleSprite[OBSTACLE_COUNT];
            for (int i = 0; i < OBSTACLE_COUNT; i++)
            {
                Vector2 velocity = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                velocity.Normalize();
                velocity *= 200;
                obstacles[i] = new ObstacleSprite(velocity, position, ScreenManager.GraphicsDevice, ScreenManager.Game);

            }

            playerSprite.LoadContent(_content);
            foreach (var obstacle in obstacles) obstacle.LoadContent(_content);
            bangers = _content.Load<SpriteFont>("bangers");
            deathSound = _content.Load<SoundEffect>("DeathAudio");
            backgroundMusic = _content.Load<Song>("BackgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            _timer = new Stopwatch();
            _timer.Start();

            

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                    playerSprite.Update(gameTime);
                    foreach (var obstacle in obstacles)
                    {
                        obstacle.Update(gameTime);
                        if (obstacle.Bounds.CollidesWith(playerSprite.Bounds))
                        {
                        ScreenManager.ToggleSparks();
                        score = _timer.Elapsed.TotalSeconds;
                        _timer.Reset();
                        for (int i = OBSTACLE_COUNT+1; i > 1 ; i--) ScreenManager.Game.Components.RemoveAt(i);
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new DeathMenuScreen(Math.Round(score, 2).ToString()), null);              
                        deathSound.Play();
                        MediaPlayer.Pause();                        
                        }
                    }
                }
        }

        
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            foreach (var obstacle in obstacles) obstacle.Draw(gameTime, spriteBatch);
            playerSprite.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(bangers, (Math.Round(_timer.Elapsed.TotalSeconds, 2)).ToString(),
                new Vector2(2, 2), Color.White);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
