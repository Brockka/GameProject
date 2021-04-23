using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Collisions;
using Microsoft.Xna.Framework.Audio;

namespace GameProject
{
    /// <summary>
    /// A class representing the player's sprite
    /// </summary>
    public class PlayerSprite
    {
        private KeyboardState keyboardState;

        private Texture2D texture;
        private Texture2D texture1;
        private Texture2D texture2;

        private int animationFrame;

        private const float ANIMATION_SPEED = .25f;

        private double animationTimer;

        private bool flipped;

        private SoundEffect _movementSound;

        public Vector2 Position { get; private set; } = new Vector2(600, 200);

        private BoundingRectangle _bounds = new BoundingRectangle(new Vector2(200, 200), 50, 55);

        private GraphicsDevice _graphics;

        private Color _color;

        private bool _moving;

        /// <summary>
        /// Bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// Creates Player sprite
        /// </summary>
        /// <param name="graphics">The graphics device</param>
        public PlayerSprite(GraphicsDevice graphics, SoundEffect sound)
        {
            this._graphics = graphics;
            _movementSound = sound;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("slime");
            texture1 = content.Load<Texture2D>("slime1");
            texture2 = content.Load<Texture2D>("slime2");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            _moving = false;

            // Apply keyboard movement
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && Position.Y > _graphics.Viewport.Y + 27)
            {
                Position += new Vector2(0, -5);
                _moving = true;
            }
            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) && Position.Y < _graphics.Viewport.Height - 27)
            {
                Position += new Vector2(0, 5);
                _moving = true;
            }
            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && Position.X > _graphics.Viewport.X + 25)
            {
                Position += new Vector2(-5,0);
                flipped = true;
                _moving = true;
            }
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && Position.X < _graphics.Viewport.Width - 25)
            {
                Position += new Vector2(5, 0);
                flipped = false;
                _moving = true;
            }
            // Update the bounds
            _bounds.X = Position.X - 16;
            _bounds.Y = Position.Y - 16;
        }            

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > ANIMATION_SPEED)
            {
                if (_moving)
                {
                    _color = Color.Cyan;
                    animationFrame++;
                    if (animationFrame > 3) animationFrame = 0;
                    _movementSound.Play();
                }
                else _color = Color.LightBlue;  
                animationTimer -= ANIMATION_SPEED;
                
            }         
            if(animationFrame == 0)spriteBatch.Draw(texture, Position, null, _color, 0, new Vector2(64,64), 0.5f, spriteEffects, 0);
            else if(animationFrame == 1)spriteBatch.Draw(texture2, Position, null, _color, 0, new Vector2(64, 64), 0.5f, spriteEffects, 0);
            else spriteBatch.Draw(texture1, Position, null, _color, 0, new Vector2(64, 64), 0.5f, spriteEffects, 0);
        }
    }
}
