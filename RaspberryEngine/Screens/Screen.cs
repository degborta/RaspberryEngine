using System.Collections.Generic;
using Extrude.Framework;
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
                if (_sprites[i]._Animated)
                    _sprites[i]._Frame++;
                // remove all oneSteps
                if (_sprites[i]._OneStep)
                    _sprites.RemoveAt(i);
            }

            for (int i = _texts.Count - 1; i >= 0; i--)
                // remove all oneSteps
                if (_texts[i]._OneStep)
                    _texts.RemoveAt(i);

            //Update particles and remove them if they are out of lifes
            for (int i = _particles.Count - 1; i >= 0; i--)
                if (_particles[i].update())
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
                if (s._Frame == null)
                {
                    if(s._Area == null)
                    ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.GetAsset(s._TextureKey), s._Position, null, s._Color, s._Angle, s._Orgin, s._Scale, SpriteEffects.None, s._Depth);
                    else ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.GetAsset(s._TextureKey), s._Area.Value, null, s._Color, s._Angle, s._Orgin, SpriteEffects.None, s._Depth);
                }
                else
                {
                    if(s._Area == null)
                    ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.GetAsset(s._TextureKey), s._Position, GameHelper.getAnimationFrame(s._Frame.Value, (Texture2D)ScreenManager.GetAsset(s._TextureKey)), s._Color, s._Angle, s._Orgin, s._Scale, SpriteEffects.None, s._Depth);
                    else ScreenManager.SpriteBatch.Draw((Texture2D)ScreenManager.GetAsset(s._TextureKey), s._Area.Value, GameHelper.getAnimationFrame(s._Frame.Value, (Texture2D)ScreenManager.GetAsset(s._TextureKey)), s._Color, s._Angle, s._Orgin, SpriteEffects.None, s._Depth);
                }
            } 

            foreach (Text s in _texts)
            {
                ScreenManager.SpriteBatch.DrawString((SpriteFont)ScreenManager.GetAsset(s._FontKey), s._Text, s._Position, s._Color, s._Angle, s._Orgin, s._Scale, SpriteEffects.None, s._Depth);
            }

			ScreenManager.SpriteBatch.End();

			//Draw all particles
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null,
                   null, null, null, _camera.get_transformation(ScreenManager.GraphicsDevice));

			foreach (Particle p in _particles) {
				ScreenManager.SpriteBatch.Draw ((Texture2D)ScreenManager.GetAsset (p._TextureKey), p._Position, null, p._Color, p._Angle,
                new Vector2 (((Texture2D)ScreenManager.GetAsset (p._TextureKey)).Width / 2, ((Texture2D)ScreenManager.GetAsset (p._TextureKey)).Height / 2),
                p._Scale, SpriteEffects.None, 0f);
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

        public void AddText(string FontKey, string Text, Vector2 Position, Vector2 Orgin, Vector2 Scale, float Angle, Color Color, float Depth)
        {
            _texts.Add(new Text(true, FontKey, Text, Position, Orgin, Scale, Angle, Color, Depth));
        }
        public void AddSprite(bool Onestep, string TextureKey, Vector2 Position, Vector2 Orgin, Vector2 Scale, float Angle, Color Color, float Depth, byte? Frame, bool Animated)
        {
            _sprites.Add(new Sprite(Onestep, TextureKey, Position, null, Orgin, Scale, Angle, Color, Depth, Frame, Animated));
        }
        public void AddSprite(bool Onestep, string TextureKey, Rectangle Area, Vector2 Orgin, float Angle, Color Color, float Depth, byte? Frame, bool Animated)
        {
            _sprites.Add(new Sprite(Onestep, TextureKey, Vector2.Zero, Area, Orgin, Vector2.One, Angle, Color, Depth, Frame, Animated));
        }
        
        public void AddEmitter(string TextureKey, Vector2 Position, EmitterSettings Settings)
        {
            _emitters.Add(new Emitter(Position, TextureKey, Settings));
        }

        public ScreenManager ScreenManager; //The screenManager for this screen
    }
}
