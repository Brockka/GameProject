using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject.ParticleSystem
{
    public class SparkParticleSystem : ParticleSystem
    {
        Rectangle _source;

        public bool Active { get; set; } = true;

        public SparkParticleSystem(Game game, Rectangle source) : base(game, 4000)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "particle";
            minNumParticles = 0;
            maxNumParticles = 2;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * -500, Vector2.Zero, Color.OrangeRed, scale: RandomHelper.NextFloat(0.5f, 1.0f), lifetime: 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Active) AddParticles(_source);
        }
    }
}
