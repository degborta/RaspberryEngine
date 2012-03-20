using Microsoft.Xna.Framework;

namespace Extrude.Framework.Screens{
	public interface IScreen {
		void Initialize();

		/// <summary>
		/// Allows the screen to run logic, such as updating the transition position.
		/// Unlike HandleInput, this method is called regardless of whether the screen
		/// is active, hidden, or in the middle of a transition.
		/// </summary>
		void Update(GameTime gameTime);

		/// <summary>
		/// This is called when the screen should draw itself.
		/// </summary>
		void Draw (GameTime gameTime);
	}
}