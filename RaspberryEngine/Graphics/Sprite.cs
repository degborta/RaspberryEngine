using Microsoft.Xna.Framework;

namespace RaspberryEngine.Graphics
{
    public class Sprite
    {
        public bool _OneStep;
        public string _TextureKey; //texture
        public Vector2 _Position; //position.
        public Rectangle? _Area; //position.
        public Vector2 _Orgin;
        public Vector2 _Scale; //size
        public float _Angle; //what direction the particle is looking
        public Color _Color; //color tint
        public float _Depth;

        public byte? _Frame; //When sprite sheets or animations are used
        public bool _Animated;

        public Sprite(bool OneStep, string TextureKey, Vector2 Position, Rectangle? Area, Vector2 Orgin,
                        Vector2 Scale, float Angle, Color Color, float Depth, byte? Frame, bool Animated)
        {
            _OneStep = OneStep;
            _TextureKey = TextureKey;
            _Position = Position;
            _Area = Area;
            _Orgin = Orgin;
            _Scale = Scale;
            _Angle = Angle;
            _Color = Color;
            _Depth = Depth;
            _Frame = Frame;
            if (_Frame == null)
                _Animated = false;
            else _Animated = Animated;
        }
    }
}
