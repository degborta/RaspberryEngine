using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Extrude.Framework.Particles
{
    public class Particle
    {
        public string _TextureKey;
        public Vector2 _Position; //position.
        private Vector2 _Velocity; //speed & direction
        public float _Angle; //what direction the particle is looking
        private float _AngleVelocity; //Dont let things rotate to mutch, OK!
        private Color _Color1; //color tint
        private Color _Color2; //color tint
        private Color _Color3; //color tint
        public Color _Color; //color tint
        public float _Scale; //size
        private float _ScaleVelocity;
        public float _Life; //how mutch life it got(if set to -1 it will never die but that can lead to some crazy shit if you know what I mean)
        private float _LifeVelocity; //how mutch life it got(if set to -1 it will never die but that can lead to some crazy shit if you know what I mean)

        public Particle(string TextureKey, Vector2 Position, Vector2 Velocity, float Angle, float AngleVelocity,
            Color Color1, Color Color2, Color Color3, float Scale, float ScaleVelocity, float LifeVelocity)
        {
            _TextureKey = TextureKey;
            _Position = Position;
            _Velocity = Velocity;
            _Angle = Angle;
            _AngleVelocity = AngleVelocity;
            _Color1 = Color1;
            _Color2 = Color2;
            _Color3 = Color3;
            _Scale = Scale;
            _ScaleVelocity = ScaleVelocity;
            _Life = 2f;
            _LifeVelocity = LifeVelocity;
        }

        /// <summary>
        /// Update particle
        /// </summary>
        /// <returns>true if particle is dead</returns>
        public bool update()
        {
            _Life -= _LifeVelocity;

            if (_Life >= 1)
            { _Color = Color.Lerp(_Color2, _Color1, _Life - 1); }
            else { _Color = Color.Lerp(_Color3, _Color2, _Life); _Color.A = (byte)(_Life * 255); }

            _Position += _Velocity;
            _Angle += _AngleVelocity;
            _Scale += _ScaleVelocity;

            if (_Life < 0)
                return true;
            else return false;
        }
    }
}
