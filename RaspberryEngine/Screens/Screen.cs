using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaspberryEngine.Assets;
using RaspberryEngine.Components;
using RaspberryEngine.Graphics;
using RaspberryEngine.Helpers;
using RaspberryEngine.Particles;

namespace RaspberryEngine.Screens
{
    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class Screen : IScreen{
        private const int _maxParticles = 5000;

        private List<Text> _texts;
        public List<LoadableAsset> Assets; //The assets being used by this screen

        /// <summary>
        /// Load data for the screen.
        /// </summary>
        protected Screen()
        {
            RendertargetWidth = 0;
            RendertargetHeight = 0;
            Camera = new Camera();
            _texts = new List<Text>();
            Sprites = new List<Sprite>();
            Emitters = new List<Emitter>();
            Particles = new List<Particle>();
            Assets = new List<LoadableAsset>();
        }

        public virtual void Initialize() { }

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            for (int i = Sprites.Count - 1; i >= 0; i--)
            {
                // update Animation frames
                if (Sprites[i].Animated)
                    Sprites[i].Frame++;
                // remove all oneSteps
                if (Sprites[i].OneStep)
                    Sprites.RemoveAt(i);
            }
            
            for (int i = _texts.Count - 1; i >= 0; i--)
                // remove all oneSteps
                if (_texts[i].OneStep)
                    _texts.RemoveAt(i);

            //Update particles and remove them if they are out of lifes
            for (int i = Particles.Count - 1; i >= 0; i--)
                if (Particles[i].Update())
                    Particles.RemoveAt(i);

            //Update all emitters and add new particles to _Particles
            foreach (Emitter e in Emitters)
                Particles.AddRange(e.Update(null, Vector2.Zero));

            //Remove a rande of particles if they are to many
            int removeRange = Particles.Count - _maxParticles;
            if (removeRange > 0)
                Particles.RemoveRange(0, removeRange);
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw (GameTime gameTime)
		{
			ScreenManager.GraphicsDevice.SetRenderTarget (RenderTarget);
			ScreenManager.GraphicsDevice.Clear (Color.CornflowerBlue);

			//Draw all normal sprites
			ScreenManager.SpriteBatch.Begin (SpriteSortMode.BackToFront, BlendState.AlphaBlend, null,
                    null, null, null, Camera.get_transformation (ScreenManager.GraphicsDevice));

            foreach (Sprite s in Sprites)
            {
                if (s.Frame == null)
                {
                    if(s.Area == null)
                        ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.AssetsManager.GetAsset(s.TextureKey), s.Position, null, s.Color, s.Angle, s.Origin, s.Scale, SpriteEffects.None, s.Depth);
                    else ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.AssetsManager.GetAsset(s.TextureKey), s.Area.Value, null, s.Color, s.Angle, s.Origin, SpriteEffects.None, s.Depth);
                }
                else
                {
                    if(s.Area == null)
                        ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.AssetsManager.GetAsset(s.TextureKey), s.Position, GameHelper.GetAnimationFrame(s.Frame.Value, (Texture2D)ScreenManager.AssetsManager.GetAsset(s.TextureKey)), s.Color, s.Angle, s.Origin, s.Scale, SpriteEffects.None, s.Depth);
                    else ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.AssetsManager.GetAsset(s.TextureKey), s.Area.Value, GameHelper.GetAnimationFrame(s.Frame.Value, (Texture2D)ScreenManager.AssetsManager.GetAsset(s.TextureKey)), s.Color, s.Angle, s.Origin, SpriteEffects.None, s.Depth);
                }
            } 

            foreach (Text s in _texts)
            {
                ScreenManager.SpriteBatch.DrawString((SpriteFont)ScreenManager.AssetsManager.GetAsset(s.FontKey), s.TextString, s.Position, s.Color, s.Angle, s.Origin, s.Scale, SpriteEffects.None, s.Depth);
            }

			ScreenManager.SpriteBatch.End();

			//Draw all particles
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null,
                   null, null, null, Camera.get_transformation(ScreenManager.GraphicsDevice));

			foreach (Particle p in Particles) {
                ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.AssetsManager.GetAsset(p.TextureKey), p.Position, null, p._color, p.Angle,
                new Vector2(((Texture2D)ScreenManager.AssetsManager.GetAsset(p.TextureKey)).Width / 2f, ((Texture2D)ScreenManager.AssetsManager.GetAsset(p.TextureKey)).Height / 2f),
                p._scale, SpriteEffects.None, 0f);
			}
			ScreenManager.SpriteBatch.End ();

			ScreenManager.GraphicsDevice.SetRenderTarget (null);
			RenderTexture = RenderTarget;
		}

        /// <param name="Relative">If the newPosition should be added to the current position</param>
        public void UpdateCamera(Vector2 newPosition, bool Relative)
        {
            if (Relative)
                Camera.Pos += newPosition;
            else Camera.Pos = newPosition;
        }

        public int RendertargetWidth { get; set; }

        public int RendertargetHeight { get; set; }

        public Camera Camera { get; private set; }

        public List<Sprite> Sprites { get; private set; }

        public List<Emitter> Emitters { get; private set; }

        public List<Particle> Particles { get; private set; }

        public RenderTarget2D RenderTarget { private get; set; }

        public Texture2D RenderTexture { get; private set; }

        public bool DisplayFPS{ get; set; }

        public void AddText(string FontKey, string Text, Vector2 Position, Vector2 Orgin, Vector2 Scale, float Angle, Color Color, float Depth)
        {
            _texts.Add(new Text(true, FontKey, Text, Position, Orgin, Scale, Angle, Color, Depth));
        }
        public void AddSprite(bool Onestep, string TextureKey, Vector2 Position, Vector2 Orgin, Vector2 Scale, float Angle, Color Color, float Depth, byte? Frame, bool Animated)
        {
            Sprites.Add(new Sprite(Onestep, TextureKey, Position, null, Orgin, Scale, Angle, Color, Depth, Frame, Animated));
        }
        public void AddSprite(bool Onestep, string TextureKey, Rectangle Area, Vector2 Orgin, float Angle, Color Color, float Depth, byte? Frame, bool Animated)
        {
            Sprites.Add(new Sprite(Onestep, TextureKey, Vector2.Zero, Area, Orgin, Vector2.One, Angle, Color, Depth, Frame, Animated));
        }
        
        public void AddEmitter(string TextureKey, Vector2 Position, EmitterSettings Settings)
        {
            Emitters.Add(new Emitter(Position, TextureKey, Settings));
        }

        public Engine ScreenManager; //The screenManager for this screen
    }
}
