using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Extrude.Framework.Particles
{
    public class EmitterSettings
    {
        //Particle Settings these are presets
        public string _name;
        public float _SpawnRate;
        public float _minVelocity;
        public float _maxVelocity;
        public float _minVelocityAngle;
        public float _maxVelocityAngle;
        public float _minAngle;
        public float _maxAngle;
        public float _minAngleVelocity;
        public float _maxAngleVelocity;
        public Color _color1;
        public Color _color2;
        public Color _color3;
        public float _minScale;
        public float _maxScale;
        public float _minScaleVelocity;
        public float _maxScaleVelocity;
        public float _minLifeVelocity;
        public float _maxLifeVelocity;

        public EmitterSettings(string name, float SpawnRate,
        float minVelocity, float maxVelocity, float minVelocityAngle, float maxVelocityAngle,
        float minAngle, float maxAngle, float minAngleVelocity, float maxAngleVelocity,
        float minScale, float maxScale, float minScaleVelocity, float maxScaleVelocity,
        float minLifeVelocity, float maxLifeVelocity,
        Color color1, Color color2, Color color3)
        {
            _name = name;
            _SpawnRate = SpawnRate;
            _minVelocity = minVelocity;
            _maxVelocity = maxVelocity;
            _minVelocityAngle = minVelocityAngle;
            _maxVelocityAngle = maxVelocityAngle;
            _minAngle = minAngle;
            _maxAngle = maxAngle;
            _minAngleVelocity = minAngleVelocity;
            _maxAngleVelocity = maxAngleVelocity;
            _color1 = color1;
            _color2 = color2;
            _color3 = color3;
            _minScale = minScale;
            _maxScale = maxScale;
            _minScaleVelocity = minScaleVelocity;
            _maxScaleVelocity = maxScaleVelocity;
            _minLifeVelocity = minLifeVelocity;
            _maxLifeVelocity = maxLifeVelocity;
        }

        #region Preset
        public static EmitterSettings[] PresetSettings =
            {
                new EmitterSettings(
                "Sparkler", // Name (its not used. only exist for you to know what type of paricle this is)
                1f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                0f,5f, // movement 'Speed'
                0,360, // movement 'Direction'
                0,360,  // Start 'Angle'
                0,0,    // rotation 'Speed'
                0.01f,0.4f, // Start 'Scale'
                0f,0.01f, // Scale 'Speed'
                0.05f,0.5f,  // Life 'Drain'
                Color.White, Color.Orange, Color.OrangeRed), // 'Colors' to shift between

                new EmitterSettings(
                "Glow", // Name (its not used. only exist for you to know what type of paricle this is)
                0.1f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                0f,3f, // movement 'Speed'
                0,360, // movement 'Direction'
                0,360,  // Start 'Angle'
                0,1,    // rotation 'Speed'
                0f,0.5f, // Start 'Scale'
                0.002f,0.005f, // Scale 'Speed'
                0.05f,0.05f,  // Life 'Drain'
                Color.Transparent, Color.White, Color.Cyan), // 'Colors' to shift between

                new EmitterSettings(
                "Fire", // Name (its not used. only exist for you to know what type of paricle this is)
                1f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                0f,4f, // movement 'Speed'
                160,200, // movement 'Direction'
                0,360,  // Start 'Angle'
                -0.1f,0.1f,    // rotation 'Speed'
                0.2f,0.3f, // Start 'Scale'
                -0.001f,-0.01f, // Scale 'Speed'
                0.05f,0.1f,  // Life 'Drain'
                Color.LightYellow, Color.DarkOrange, Color.Red), // 'Colors' to shift between

                new EmitterSettings(
                "Smoke", // Name (its not used. only exist for you to know what type of paricle this is)
                5f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                1f,1f, // movement 'Speed'
                180,180, // movement 'Direction'
                0,360,  // Start 'Angle'
                -0.01f,0.01f,    // rotation 'Speed'
                0.2f,0.2f, // Start 'Scale'
                0.0001f,0.001f, // Scale 'Speed'
                0.005f,0.005f,  // Life 'Drain'
                Color.Transparent, Color.Gray, Color.Black), // 'Colors' to shift between
        };
        #endregion
    }

    public class Emitter
    {
        #region Fields
        Random _Random = new Random();

        //Emitter Settings
        Vector2 _Position;
        string _TextureKey;

        //Particle Settings these are presets
        EmitterSettings _Settings;
        float _SpawnNext;

        #endregion

        public Emitter(Vector2 Position, string TextureKey, EmitterSettings EmitterSettings)
        {
            _Position = Position;
            _TextureKey = TextureKey;
            _Settings = EmitterSettings;
        }

        public List<Particle> Update(Vector2? Position, Vector2 Gravity)
        {
            List<Particle> newParticles = new List<Particle>();

            //Update emitters position if its not solid
            if (Position != null)
                _Position = new Vector2(Position.Value.X, Position.Value.Y);

            //Update and spawn new particles
            _SpawnNext++;
            while (_SpawnNext > _Settings._SpawnRate)
            {
                newParticles.Add(GenerateParticle());
                _SpawnNext -= _Settings._SpawnRate;
            }

            return newParticles;
        }

        private Particle GenerateParticle()
        {
            float velocityAngle = MathHelper.Lerp(_Settings._minVelocityAngle, _Settings._maxVelocityAngle, (float)_Random.NextDouble());
            Vector2 velocity = MathHelper.Lerp(_Settings._minVelocity, _Settings._maxVelocity, (float)_Random.NextDouble()) * new Vector2((float)Math.Sin(MathHelper.ToRadians(velocityAngle)), (float)Math.Cos(MathHelper.ToRadians(velocityAngle)));
            float angle = MathHelper.Lerp(_Settings._minAngle, _Settings._maxAngle, (float)_Random.NextDouble());
            float angleVelocity = MathHelper.Lerp(_Settings._minAngleVelocity, _Settings._maxAngleVelocity, (float)_Random.NextDouble());
            float scale = MathHelper.Lerp(_Settings._minScale, _Settings._maxScale, (float)_Random.NextDouble());
            float scaleVelocity = MathHelper.Lerp(_Settings._minScaleVelocity, _Settings._maxScaleVelocity, (float)_Random.NextDouble());
            float lifeVelocity = MathHelper.Lerp(_Settings._minLifeVelocity, _Settings._maxLifeVelocity, (float)_Random.NextDouble());
            return new Particle(
                    _TextureKey,
                    _Position,
                    velocity,
                    angle,
                    angleVelocity,
                    _Settings._color1, _Settings._color2, _Settings._color3,
                    scale, scaleVelocity,
                    lifeVelocity);
        }
    }
}
