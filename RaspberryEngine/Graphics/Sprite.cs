using Microsoft.Xna.Framework;

namespace RaspberryEngine.Graphics
{
    public class Sprite
    {
    	
        public Sprite(bool oneStep, string textureKey, Vector2 position, Rectangle? area, Vector2 origin,
                        Vector2 scale, float angle, Color color, float depth, byte? frame, bool animated)
        {
            _oneStep = oneStep;
            _textureKey = textureKey;
            _position = position;
            _area = area;
            _origin = origin;
            _scale = scale;
            _angle = angle;
            _color = color;
            _depth = depth;
            _frame = frame;
            if (Frame == null)
                Animated = false;
            else Animated = animated;
        }

		private bool _oneStep;
		public bool OneStep {
			get { return _oneStep; }
			set { _oneStep = value; }
		}

		//texture
		private string _textureKey;
		public string TextureKey {
			get {
				return _textureKey;
			}
			set {
				_textureKey = value;
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

		private Rectangle? _area;
		public Rectangle? Area {
			get {
				return _area;
			}
			set {
				_area = value;
			}
		} //position.

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

		private byte? _frame;
		public byte? Frame {
			get {
				return _frame;
			}
			set {
				_frame = value;
			}
		}

		//When sprite sheets or animations are used
		private bool _animated;
		public bool Animated {
			get {
				return _animated;
			}
			set {
				_animated = value;
			}
		}

    }
}
