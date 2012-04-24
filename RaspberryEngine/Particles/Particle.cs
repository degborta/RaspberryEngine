using Microsoft.Xna.Framework;

namespace RaspberryEngine.Particles
{
	public class Particle
	{
		public string TextureKey;
		public Vector2 Position; //position.
		private Vector2 _velocity; //speed & direction
		public float Angle; //what direction the particle is looking
		private float _angleVelocity; //Dont let things rotate to mutch, OK!
		private Color _color1; //color tint
		private Color _color2; //color tint
		private Color _color3; //color tint
		public Color _color; //color tint
		public float _scale; //size
		private float _scaleVelocity;
		public float _life; //how mutch life it got(if set to -1 it will never die but that can lead to some crazy shit if you know what I mean)
		private float _lifeVelocity; //how mutch life it got(if set to -1 it will never die but that can lead to some crazy shit if you know what I mean)

		public Particle(string textureKey, Vector2 position, Vector2 velocity, float angle, float angleVelocity,
			Color color1, Color color2, Color color3, float scale, float scaleVelocity, float lifeVelocity)
		{
			this.TextureKey = textureKey;
			this.Position = position;
			this.Angle = angle;
			
			_velocity = velocity;
			_angleVelocity = angleVelocity;
			_color1 = color1;
			_color2 = color2;
			_color3 = color3;
			_scale = scale;
			_scaleVelocity = scaleVelocity;
			_life = 2f;
			_lifeVelocity = lifeVelocity;
		}

		/// <summary>
		/// Update particle
		/// </summary>
		/// <returns>true if particle is dead</returns>
		public bool Update()
		{
			_life -= _lifeVelocity;

			if (_life >= 1)
			{ _color = Color.Lerp(_color2, _color1, _life - 1); }
			else { _color = Color.Lerp(_color3, _color2, _life); _color.A = (byte)(_life * 255); }

			Position += _velocity;
			Angle += _angleVelocity;
			_scale += _scaleVelocity;

			if (_life < 0)
				return true;
			else return false;
		}
	}
}
