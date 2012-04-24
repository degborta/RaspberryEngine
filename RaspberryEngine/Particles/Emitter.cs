using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RaspberryEngine.Particles
{
    

    public class Emitter
    {
        #region Fields
        Random _random = new Random();
        //Emitter Settings
        Vector2 _position;
        string _textureKey;

        //Particle Settings these are presets
        EmitterSettings _settings;
        float _spawnNext;

        #endregion

        public Emitter(Vector2 position, string textureKey, EmitterSettings emitterSettings)
        {
            _position = position;
            _textureKey = textureKey;
            _settings = emitterSettings;
        }

        public List<Particle> Update(Vector2? position, Vector2 gravity)
        {
            List<Particle> newParticles = new List<Particle>();

            //Update emitters position if its not solid
            if (position != null)
                _position = new Vector2(position.Value.X, position.Value.Y);

            //Update and spawn new particles
            _spawnNext++;
            while (_spawnNext > _settings.SpawnRate)
            {
                newParticles.Add(GenerateParticle());
                _spawnNext -= _settings.SpawnRate;
            }

            return newParticles;
        }

        private Particle GenerateParticle()
        {
            float velocityAngle = MathHelper.Lerp(_settings.MinVelocityAngle, _settings.MaxVelocityAngle, (float)_random.NextDouble());
            Vector2 velocity = MathHelper.Lerp(_settings.MinVelocity, _settings.MaxVelocity, (float)_random.NextDouble()) * new Vector2((float)Math.Sin(MathHelper.ToRadians(velocityAngle)), (float)Math.Cos(MathHelper.ToRadians(velocityAngle)));
            float angle = MathHelper.Lerp(_settings.MinAngle, _settings.MaxAngle, (float)_random.NextDouble());
            float angleVelocity = MathHelper.Lerp(_settings.MinAngleVelocity, _settings.MaxAngleVelocity, (float)_random.NextDouble());
            float scale = MathHelper.Lerp(_settings.MinScale, _settings.MaxScale, (float)_random.NextDouble());
            float scaleVelocity = MathHelper.Lerp(_settings.MinScaleVelocity, _settings.MaxScaleVelocity, (float)_random.NextDouble());
            float lifeVelocity = MathHelper.Lerp(_settings.MinLifeVelocity, _settings.MaxLifeVelocity, (float)_random.NextDouble());
            return new Particle(
                    _textureKey,
                    _position,
                    velocity,
                    angle,
                    angleVelocity,
                    _settings.Color1, _settings.Color2, _settings.Color3,
                    scale, scaleVelocity,
                    lifeVelocity);
        }
    }
}
