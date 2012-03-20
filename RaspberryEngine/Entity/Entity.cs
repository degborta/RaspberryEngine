using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extrude.Framework.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Extrude.Framework.Entity {	
	public abstract class Entity {
		public Entity(){ }
		public abstract void Update(float elapsedTime);

		// Draw is handled by the screenmanager.
		//public abstract void Draw(SpriteBatch spriteBatch);

		private Vector2 _position;
		public Vector2 Position {
			get { return _position; }
			set { _position = value; }
		}

		private float _rotation = 0;
		public float Rotation {
			get { return _rotation; }
			set { _rotation = value; }
		}

		
	}
}

