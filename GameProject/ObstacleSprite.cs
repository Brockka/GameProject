using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Collisions;

namespace GameProject
{
    /// <summary>
    /// A class representing the obstacle sprites
    /// </summary>
    public class ObstacleSprite
    {
        private Texture2D atlas;

        public Vector2 velocity;

        private Vector2 position;

        private GraphicsDevice graphics;

        private BoundingCircle bounds;

        /// <summary>
        //  Bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// Creates new obstacle sprite
        /// </summary>
        /// <param name="velocity">Initial velocity of obstacle</param>
        /// <param name="position">Initial position of obstacle</param>
        /// <param name="graphics">The graphics device</param>
        public ObstacleSprite(Vector2 velocity, Vector2 position, GraphicsDevice graphics)
        {
            this.velocity = velocity;
            this.position = position;
            this.graphics = graphics;
            this.bounds = new BoundingCircle(position + new Vector2(32, 24), 16);
        }

        /// <summary>
        /// Loads the obstacle sprite texture
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            atlas = content.Load<Texture2D>("colored_packed");
        }

        /// <summary>
        /// Update the obstacle sprite and bounce off of walls if necessary
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            position += (float)gameTime.ElapsedGameTime.TotalSeconds * velocity * 2;

            if (position.X < graphics.Viewport.X
                || position.X > graphics.Viewport.Width)
            {
                velocity.X *= -1;
            }

            if (position.Y < graphics.Viewport.Y
                || position.Y > graphics.Viewport.Height)
            {
                velocity.Y *= -1;
            }
            // Update the bounds
            bounds.Center = new Vector2(position.X, position.Y);
        }

        /// <summary>
        /// Draws the obstacle spriteS
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(atlas, position, new Rectangle(240, 160, 16, 16), Color.Red, 0, new Vector2(0, 0), 3.0f, SpriteEffects.None, 0);
        }
    }
}
