using Microsoft.Xna.Framework;

namespace RaspberryEngine.Graphics
{
    class Text
    {
        public bool _OneStep;
        public string _FontKey; //texture
        public string _Text;
        public Vector2 _Position; //position.
        public Vector2 _Orgin;
        public Vector2 _Scale; //size
        public float _Angle; //what direction the particle is looking
        public Color _Color; //color tint
        public float _Depth;

        public Text(bool OneStep, string FontKey, string Text, Vector2 Position, Vector2 Orgin,
                        Vector2 Scale, float Angle, Color Color, float Depth)
        {
            _OneStep = OneStep;
            _FontKey = FontKey;
            _Text = Text;
            _Position = Position;
            _Orgin = Orgin;
            _Scale = Scale;
            _Angle = Angle;
            _Color = Color;
            _Depth = Depth;
        }
    }
}
