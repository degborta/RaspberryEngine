using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Extrude.Framework.Entity {
	class EntityManager {
	
		SpriteBatch spriteBatch;
		List<Entity> _entities = new List<Entity>();

		public List<Entity> Entities {
			get { return _entities; }
		}

		public EntityManager(GraphicsDevice device) {
			spriteBatch = new SpriteBatch(device);
		}



		public void Update(float elapsedTime) {
			for (int i = 0, length = _entities.Count; i < length; i++) {
				_entities[i].Update(elapsedTime);
			}
		}

		public void Draw() {
			for (int i = 0, length = _entities.Count; i < length; i++) {
				_entities[i].Draw(spriteBatch);
			}
		}

		public void Add(Entity someObject) {
			_entities.Add(someObject);
		}
	}

}
