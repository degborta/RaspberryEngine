using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Extrude.Framework.Screens {
	public class PhysicsScreen : Screen, IScreen {


		protected World World;

		public PhysicsScreen() : base() {
			World = null;
		}
		
		public override void Initialize() {
			base.Initialize();

			if (World == null) {
				World = new World(Vector2.Zero);
			} else {
				World.Clear();
			}

			// Loading may take a while... so prevent the game from "catching up" once we finished loading
			ScreenManager.Game.ResetElapsedTime();
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
			World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
			base.Update(gameTime);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			base.Draw(gameTime);
		}

		
	}
}
