using System.Collections.Generic;
using RaspberryEngine;
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
        int _maxParticles = 5000;
        int _rendertargetWidth = 0;
        int _rendertargetHeight = 0;
        private Camera _camera;

        private List<Text> _texts;
        private List<Sprite> _sprites;
        private List<Emitter> _emitters;
        private List<Particle> _particles;
        
		private RenderTarget2D _renderTarget; //This is the screens rendertarget
        private Texture2D _renderTexture; //This is the final screen image

        public List<LoadableAsset> Assets; //The assets being used by this screen

        /// <summary>
        /// Load data for the screen.
        /// </summary>
        public Screen()
        {
            _camera = new Camera();
            _texts = new List<Text>();
            _sprites = new List<Sprite>();
            _emitters = new List<Emitter>();
            _particles = new List<Particle>();
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

            for (int i = _sprites.Count - 1; i >= 0; i--)
            {
                // update Animation frames
                if (_sprites[i].Animated)
                    _sprites[i].Frame++;
                // remove all oneSteps
                if (_sprites[i].OneStep)
                    _sprites.RemoveAt(i);
            }

            for (int i = _texts.Count - 1; i >= 0; i--)
                // remove all oneSteps
                if (_texts[i].OneStep)
                    _texts.RemoveAt(i);

            //Update particles and remove them if they are out of lifes
            for (int i = _particles.Count - 1; i >= 0; i--)
                if (_particles[i].Update())
                    _particles.RemoveAt(i);

            //Update all emitters and add new particles to _Particles
            foreach (Emitter e in _emitters)
                _particles.AddRange(e.Update(null, Vector2.Zero));

            //Remove a rande of particles if they are to many
            int removeRange = _particles.Count - _maxParticles;
            if (removeRange > 0)
                _particles.RemoveRange(0, removeRange);
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw (GameTime gameTime)
		{
			ScreenManager.GraphicsDevice.SetRenderTarget (_renderTarget);
			ScreenManager.GraphicsDevice.Clear (Color.CornflowerBlue);

			//Draw all normal sprites
			ScreenManager.SpriteBatch.Begin (SpriteSortMode.BackToFront, BlendState.AlphaBlend, null,
                    null, null, null, _camera.get_transformation (ScreenManager.GraphicsDevice));

            foreach (Sprite s in _sprites)
            {
                if (s.Frame == null)
                {
                    if(s.Area == null)
                        ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.Assets.GetAsset(s.TextureKey), s.Position, null, s.Color, s.Angle, s.Origin, s.Scale, SpriteEffects.None, s.Depth);
                    else ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.Assets.GetAsset(s.TextureKey), s.Area.Value, null, s.Color, s.Angle, s.Origin, SpriteEffects.None, s.Depth);
                }
                else
                {
                    if(s.Area == null)
                        ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.Assets.GetAsset(s.TextureKey), s.Position, GameHelper.GetAnimationFrame(s.Frame.Value, (Texture2D)ScreenManager.Assets.GetAsset(s.TextureKey)), s.Color, s.Angle, s.Origin, s.Scale, SpriteEffects.None, s.Depth);
                    else ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.Assets.GetAsset(s.TextureKey), s.Area.Value, GameHelper.GetAnimationFrame(s.Frame.Value, (Texture2D)ScreenManager.Assets.GetAsset(s.TextureKey)), s.Color, s.Angle, s.Origin, SpriteEffects.None, s.Depth);
                }
            } 

            foreach (Text s in _texts)
            {
                ScreenManager.SpriteBatch.DrawString((SpriteFont)ScreenManager.Assets.GetAsset(s.FontKey), s.TextString, s.Position, s.Color, s.Angle, s.Origin, s.Scale, SpriteEffects.None, s.Depth);
            }

			ScreenManager.SpriteBatch.End();

			//Draw all particles
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null,
                   null, null, null, _camera.get_transformation(ScreenManager.GraphicsDevice));

			foreach (Particle p in _particles) {
                ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.Assets.GetAsset(p.TextureKey), p.Position, null, p._color, p.Angle,
                new Vector2(((Texture2D)ScreenManager.Assets.GetAsset(p.TextureKey)).Width / 2, ((Texture2D)ScreenManager.Assets.GetAsset(p.TextureKey)).Height / 2),
                p._scale, SpriteEffects.None, 0f);
			}
			ScreenManager.SpriteBatch.End ();

			ScreenManager.GraphicsDevice.SetRenderTarget (null);
			_renderTexture = (Texture2D)_renderTarget;
		}

        /// <param name="Relative">If the newPosition should be added to the current position</param>
        public void UpdateCamera(Vector2 newPosition, bool Relative)
        {
            if (Relative)
                _camera.Pos += newPosition;
            else _camera.Pos = newPosition;
        }

        public int RendertargetWidth
        {
            get
            {
                return _rendertargetWidth;
            }
            set
            {
                _rendertargetWidth = value;
            }
        }
        public int RendertargetHeight
        {
            get
            {
                return _rendertargetHeight;
            }
            set
            {
                _rendertargetHeight = value;
            }
        }

        public Camera Camera
        {
            get
            {
                return _camera;
            }
        }

        public List<Sprite> Sprites
        {
            get
            {
                return _sprites;
            }
        }
        public List<Emitter> Emitters
        {
            get
            {
                return _emitters;
            }
        }
        public List<Particle> Particles
        {
            get
            {
                return _particles;
            }
        }

        public RenderTarget2D RenderTarget
        {
            set
            {
                _renderTarget = value;
            }
        }
        public Texture2D RenderTexture
        {
            get
            {
                return _renderTexture;
            }
        }

    	public bool DisplayFPS{ get; set; }

        public void AddText(string fontKey, string text, Vector2 position, Vector2 orgin, Vector2 scale, float angle, Color color, float depth)
        {
            _texts.Add(new Text(true, fontKey, text, position, orgin, scale, angle, color, depth));
        }
        public void AddSprite(bool onestep, string textureKey, Vector2 position, Vector2 orgin, Vector2 scale, float angle, Color color, float depth, byte? frame, bool animated)
        {
            _sprites.Add(new Sprite(onestep, textureKey, position, null, orgin, scale, angle, color, depth, frame, animated));
        }
        public void AddSprite(bool onestep, string textureKey, Rectangle area, Vector2 orgin, float angle, Color color, float depth, byte? frame, bool animated)
        {
            _sprites.Add(new Sprite(onestep, textureKey, Vector2.Zero, area, orgin, Vector2.One, angle, color, depth, frame, animated));
        }
        
        public void AddEmitter(string textureKey, Vector2 position, EmitterSettings settings)
        {
            _emitters.Add(new Emitter(position, textureKey, settings));
        }

        public ScreenManager ScreenManager; //The screenManager for this screen
    }
}
