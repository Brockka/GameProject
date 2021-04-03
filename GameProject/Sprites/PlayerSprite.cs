using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Collisions;

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

        public Vector2 Position { get; private set; } = new Vector2(600, 200);

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200, 200), 55, 60);

        private GraphicsDevice graphics;

        /// <summary>
        /// Bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Creates Player sprite
        /// </summary>
        /// <param name="graphics">The graphics device</param>
        public PlayerSprite(GraphicsDevice graphics)
        {
            this.graphics = graphics;
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

            if (Position.X < graphics.Viewport.X
                || Position.X > graphics.Viewport.Width - 64)
            {
                
            }

            if (Position.Y < graphics.Viewport.Y
                || Position.Y > graphics.Viewport.Height - 64)
            {
                
            }

            // Apply keyboard movement
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && Position.Y > graphics.Viewport.Y) Position += new Vector2(0, -5);
            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) && Position.Y < graphics.Viewport.Height) Position += new Vector2(0, 5);
            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && Position.X > graphics.Viewport.X)
            {
                Position += new Vector2(-5,0);
                flipped = true;
            }
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && Position.X < graphics.Viewport.Width)
            {
                Position += new Vector2(5, 0);
                flipped = false;
            }
            // Update the bounds
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 16;
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
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
                
            }         
            if(animationFrame == 0)spriteBatch.Draw(texture, Position, null, Color.Cyan, 0, new Vector2(64,64), 0.5f, spriteEffects, 0);
            else if(animationFrame == 1)spriteBatch.Draw(texture2, Position, null, Color.Cyan, 0, new Vector2(64, 64), 0.5f, spriteEffects, 0);
            else spriteBatch.Draw(texture1, Position, null, Color.Cyan, 0, new Vector2(64, 64), 0.5f, spriteEffects, 0);
        }
    }
}
