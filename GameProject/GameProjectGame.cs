using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace GameProject
{
    public class GameProjectGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SoundEffect deathSound;

        private ObstacleSprite[] obstacles;
        private PlayerSprite playerSprite;
        private bool gameOver;
        private double score;
        private SpriteFont bangers;
        private Song backgroundMusic;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public GameProjectGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            gameOver = false;
            Vector2 position = new Vector2(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2
                );
            System.Random random = new System.Random();

            playerSprite = new PlayerSprite(GraphicsDevice);
            obstacles = new ObstacleSprite[10];
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = new Vector2( (float)random.NextDouble(), (float)random.NextDouble() );
                velocity.Normalize();
                velocity *= 200;
                obstacles[i] = new ObstacleSprite(velocity, position, GraphicsDevice);
                
            }
            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite.LoadContent(Content);
            foreach (var obstacle in obstacles) obstacle.LoadContent(Content);
            bangers = Content.Load<SpriteFont>("bangers");
            deathSound = Content.Load<SoundEffect>("DeathAudio");
            backgroundMusic = Content.Load<Song>("Bauchamp - 103 old school casserole beat");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!gameOver)
            {
                playerSprite.Update(gameTime);
                foreach (var obstacle in obstacles)
                {
                    obstacle.Update(gameTime);
                    if (obstacle.Bounds.CollidesWith(playerSprite.Bounds))
                    {
                        gameOver = true;
                        score = gameTime.TotalGameTime.TotalSeconds;
                        deathSound.Play();
                        MediaPlayer.Pause();
                    }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {

            if (gameOver)
            {
                GraphicsDevice.Clear(Color.Red);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(bangers, "       Game Over!\nTime Survived: " + (Math.Round(score, 2)).ToString(),
                    new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 3), Color.White);
                _spriteBatch.End();
                base.Draw(gameTime);
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();
                foreach (var obstacle in obstacles) obstacle.Draw(gameTime, _spriteBatch);
                playerSprite.Draw(gameTime, _spriteBatch);
                _spriteBatch.DrawString(bangers, (Math.Round(gameTime.TotalGameTime.TotalSeconds, 2)).ToString(),
                    new Vector2(2, 2), Color.White);
                _spriteBatch.End();

                base.Draw(gameTime);
            }
            
        }
    }
}
