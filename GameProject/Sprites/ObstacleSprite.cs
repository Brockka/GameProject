using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Collisions;
using GameProject.ParticleSystem;

namespace GameProject
{
    /// <summary>
    /// A class representing the obstacle sprites
    /// </summary>
    public class ObstacleSprite : IParticleEmitter
    {
        private const float ANIMATION_SPEED = .25f;

        private double animationTimer;

        private bool animated;

        private Texture2D atlas;

        public Vector2 Velocity { get; set; }

        public Vector2 Position { get; set; }

        private GraphicsDevice graphics;

        private BoundingCircle bounds;

        private FireParticleSystem fire;

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
        public ObstacleSprite(Vector2 velocity, Vector2 position, GraphicsDevice graphics, Game game)
        {
            this.Velocity = velocity;
            this.Position = position;
            this.graphics = graphics;
            this.bounds = new BoundingCircle(position + new Vector2(24, 30), 30);
            fire = new FireParticleSystem(game, this);
            game.Components.Add(fire);
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
            //todo: add your update logic here
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 xAcceleration = new Vector2(2, 0);
            Vector2 yAcceleration = new Vector2(0, 2);
            if (Velocity.X > 0) Velocity += xAcceleration * t;
            else Velocity -= xAcceleration * t;
            if (Velocity.Y > 0) Velocity += yAcceleration * t;
            else Velocity -= yAcceleration * t;
            Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Velocity;

            if (Position.X < graphics.Viewport.X
                || Position.X > graphics.Viewport.Width)
            {
                Velocity = new Vector2(-Velocity.X, Velocity.Y);
            }

            if (Position.Y < graphics.Viewport.Y
                || Position.Y > graphics.Viewport.Height)
            {
                Velocity = new Vector2(Velocity.X, -Velocity.Y);
            }
            //Update the bounds
            bounds.Center = new Vector2(Position.X + 24, Position.Y + 30);
        }

        /// <summary>
        /// Draws the obstacle spriteS
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > ANIMATION_SPEED)
            {
                animationTimer -= ANIMATION_SPEED;
                if (animated) animated = false;
                else animated = true;
                
            }
            if (animated) spriteBatch.Draw(atlas, Position, new Rectangle(240, 160, 16, 16), Color.Red, 0, new Vector2(0, 0), 3.0f, SpriteEffects.None, 0);
            else spriteBatch.Draw(atlas, Position, new Rectangle(240, 160, 16, 16), Color.LightGoldenrodYellow, 0, new Vector2(0, 0), 2.75f, SpriteEffects.FlipHorizontally, 0);
        }
    }
}
