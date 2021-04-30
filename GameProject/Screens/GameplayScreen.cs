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
        private int _obstacleCount;
        private ContentManager _content;

        private SoundEffect _deathSound;
        private SoundEffect _movementSound;
        private bool _newHighScore;

        private ObstacleSprite[] _obstacles;
        private PlayerSprite _playerSprite;
        private Stopwatch _timer;
        private double score;
        private SpriteFont _bangers;
        private Song _backgroundMusic;
        private Texture2D _background;

        private double _highScore;

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

            Vector2 position = new Vector2(_random.Next(0,10), _random.Next(0, 10));
            System.Random random = new System.Random();

            _bangers = _content.Load<SpriteFont>("bangers");
            _deathSound = _content.Load<SoundEffect>("DeathAudio");
            _movementSound = _content.Load<SoundEffect>("Movement");
            _backgroundMusic = _content.Load<Song>("BackgroundMusic");
            _background = _content.Load<Texture2D>("ScrollingBackground");
            _highScore = HighScores.HighScoreArr[HighScores.Difficulty];
            if (HighScores.Difficulty == 0) _obstacleCount = 5;
            else if (HighScores.Difficulty == 1) _obstacleCount = 9;
            else _obstacleCount = 12;

            _playerSprite = new PlayerSprite(ScreenManager.GraphicsDevice, _movementSound);
            _obstacles = new ObstacleSprite[_obstacleCount];
            for (int i = 0; i < _obstacleCount; i++)
            {
                Vector2 velocity = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                velocity.Normalize();
                velocity *= 200;
                _obstacles[i] = new ObstacleSprite(velocity, new Vector2(_random.Next(200), _random.Next(200)), ScreenManager.GraphicsDevice, ScreenManager.Game);

            }

            _playerSprite.LoadContent(_content);
            foreach (var obstacle in _obstacles) obstacle.LoadContent(_content);
            
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);
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
                    _playerSprite.Update(gameTime);
                    foreach (var obstacle in _obstacles)
                    {
                        obstacle.Update(gameTime);
                        if (obstacle.Bounds.CollidesWith(_playerSprite.Bounds))
                        {
                        ScreenManager.ToggleSparks();
                        score = _timer.Elapsed.TotalSeconds;
                        if(_highScore < score)
                        {
                            _highScore = score;
                            HighScores.WriteFile(_highScore);
                        }
                        _timer.Reset();
                        for (int i = _obstacleCount+2; i > 2 ; i--) ScreenManager.Game.Components.RemoveAt(i);
                        ScreenManager.RemoveScreen(this);
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        if(!_newHighScore)ScreenManager.AddScreen(new DeathMenuScreen("Time survived: " + Math.Round(score, 2).ToString()), null);
                        else ScreenManager.AddScreen(new DeathMenuScreen("New High Score: " + Math.Round(score, 2).ToString()), null);
                        ScreenManager.Flash(_playerSprite.Position);
                        ScreenManager.Flash(_playerSprite.Position);
                        ScreenManager.Flash(_playerSprite.Position);
                        _deathSound.Play();
                        MediaPlayer.Pause();
                        break;
                        }
                    }
                }
        }

        
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            float scrollSpeed = (float)_timer.Elapsed.TotalSeconds * -15.0f;
            Matrix transform = Matrix.CreateTranslation(scrollSpeed, 0, 0);
            spriteBatch.Begin(transformMatrix: transform);
            spriteBatch.Draw(_background, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 2.5f, SpriteEffects.None,0);
            spriteBatch.End();

            spriteBatch.Begin();
            foreach (var obstacle in _obstacles) obstacle.Draw(gameTime, spriteBatch);
            _playerSprite.Draw(gameTime, spriteBatch);
            var textPositionX = ScreenManager.GraphicsDevice.Viewport.Width - ScreenManager.Font.MeasureString("High Score: XXXX").X - 2;
            if (_newHighScore)
            {
                spriteBatch.DrawString(_bangers, Math.Round(_timer.Elapsed.TotalSeconds, 1).ToString(), new Vector2(2, 2), Color.Green);
                spriteBatch.DrawString(_bangers, "High Score: " + Math.Round(_timer.Elapsed.TotalSeconds, 1), new Vector2(textPositionX, 2), Color.Green);
            }
             else
            {
                if (_highScore < _timer.Elapsed.TotalSeconds) _newHighScore = true;
                spriteBatch.DrawString(_bangers, Math.Round(_timer.Elapsed.TotalSeconds, 1).ToString(), new Vector2(2, 2), Color.White);
                spriteBatch.DrawString(_bangers, "High Score: " + Math.Round(_highScore, 1).ToString(), new Vector2(textPositionX, 2), Color.Yellow);
            }              
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
