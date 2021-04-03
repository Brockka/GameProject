using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.ParticleSystem
{
    public class FireParticleSystem : ParticleSystem
    {
        IParticleEmitter _emitter;

        public FireParticleSystem(Game game, IParticleEmitter emitter) : base(game, 2000)
        {
            _emitter = emitter;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "explosionBlue";

            minNumParticles = 2;
            maxNumParticles = 5;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var Velocity = -_emitter.Velocity;

            var acceleration = new Vector2(300, 300);

            var scale = RandomHelper.NextFloat(0.015f, 0.20f);

            var lifetime = RandomHelper.NextFloat(0.05f, 0.1f);

            p.Initialize(where + new Vector2(16,16), Velocity, acceleration, Color.BlueViolet, scale: scale, lifetime: lifetime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AddParticles(_emitter.Position);
        }
    }
}
