using Microsoft.Xna.Framework;

namespace RaspberryEngine.Graphics
{
    class Text
    {

        public Text(bool oneStep, string fontKey, string text, Vector2 position, Vector2 origin,
                        Vector2 scale, float angle, Color color, float depth)
        {
            _oneStep = oneStep;
            _fontKey = fontKey;
            _text = text;
            _position = position;
            _origin = origin;
            _scale = scale;
            _angle = angle;
            _color = color;
            _depth = depth;
        }


		private bool _oneStep;
		public bool OneStep {
			get { return _oneStep; }
			set { _oneStep = value; }
		}

		private string _fontKey;
		public string FontKey {
			get {
				return _fontKey;
			}
			set {
				_fontKey = value;
			}
		}

		private string _text;
		public string TextString {
			get {
				return _text;
			}
			set {
				_text = value;
			}
		}
		private Vector2 _position;
		public Vector2 Position {
			get {
				return _position;
			}
			set {
				_position = value;
			}
		}
		private Vector2 _origin;
		public Vector2 Origin {
			get {
				return _origin;
			}
			set {
				_origin = value;
			}
		}
		//size
		private Vector2 _scale;
		public Vector2 Scale {
			get {
				return _scale;
			}
			set {
				_scale = value;
			}
		}

		//what direction the particle is looking
		private float _angle;
		public float Angle {
			get {
				return _angle;
			}
			set {
				_angle = value;
			}
		}
		//color tint
		private Color _color;
		public Color Color {
			get {
				return _color;
			}
			set {
				_color = value;
			}
		}

		private float _depth;
		public float Depth {
			get {
				return _depth;
			}
			set {
				_depth = value;
			}
		}

    }
}
