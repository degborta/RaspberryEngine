using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using RaspberryEngine.Assets;
using RaspberryEngine.Components;
using RaspberryEngine.Screens;
using RaspberryEngine.Network;

namespace RaspberryEngine
{
    /// <summary>
    /// The screen manager is a class which manages one or more Screen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class Engine : Game
    {
        #region Fields

        //Properties
        private int _preferredBackBufferWidth;
        public int PreferredBackBufferWidth
        {
            get { return _preferredBackBufferWidth; }
            set { _preferredBackBufferWidth = value; }
        }

        private int _preferredBackBufferHeight;
        public int PreferredBackBufferHeight
        {
            get { return _preferredBackBufferHeight; }
            set { _preferredBackBufferHeight = value; }
        }

        private int _targetFrameRate;
        public int TargetFrameRate
        {
            get { return _targetFrameRate; }
            set { _targetFrameRate = value; }
        }

        private bool _useHighQualityContent;
        public bool UseHighQualityContent
        {
            get { return _useHighQualityContent; }
            set { _useHighQualityContent = value; }
        }

        private string _contentDirectory;
        public string ContentDirectory;

        private DisplayOrientation _supportedOrientations;
        public DisplayOrientation SupportedOrientations
        {
            set { _supportedOrientations = value; }
        }

        private GraphicsDeviceManager _graphics;

        private bool _networkEnabled;
        public bool NetworkEnabled
        {
            get { return _networkEnabled; }
            set { _networkEnabled = value; }
        }

        private Rectangle _screenBounds;
        public Rectangle ScreenBounds
        {
            get { return _screenBounds; }
        }

        private NetworkManager _networkManager;
        public NetworkManager NetworkManager
        {
            get { return _networkManager; }
        }

        private AssetsManager _assetsManager;
        public AssetsManager AssetsManager
        {
            get { return _assetsManager; }
        }

        private SpriteBatch _spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        private List<Screen> _screens = new List<Screen>();
        public List<Screen> Screens
        {
            get { return _screens; }
        }

        private FPSCounter _fpsCounter;
        public FPSCounter FpsCounter
        {
            get { return _fpsCounter; }
        }

        #endregion

        /// <summary>
        /// A constructor with Network disabled
        /// </summary>
        public Engine()
        {
            Content.RootDirectory = _contentDirectory;
            _graphics = new GraphicsDeviceManager(this);
            _assetsManager = new AssetsManager(Content);
            _networkManager = new Network.NetworkManager();
            _fpsCounter = new FPSCounter();

            // Framerate differs between platforms.
            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
            //graphics.IsFullScreen = true;

            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30);

            _graphics.DeviceReset += new EventHandler<EventArgs>(OnGraphicsComponentDeviceReset);
            _graphics.SupportedOrientations = _supportedOrientations;
        }

        protected void OnGraphicsComponentDeviceReset(object sender, EventArgs e)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            _screenBounds = new Rectangle(0,0,_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight); // This is used by the screens to get the size for the default rendertarget
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (NetworkEnabled)
                _networkManager.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            _screens[0].Update(gameTime);

			// Make sure to update the fpscounter
			_fpsCounter.Update(gameTime);
            _fpsCounter.IncrementCounter();
            base.Update(gameTime);
		}

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear (Color.CornflowerBlue);
			
			//Update the draw for the active screen
			_screens [0].Draw (gameTime);
			
			_spriteBatch.Begin ();
			foreach (Screen s in _screens)
            {
				_spriteBatch.Draw (s.RenderTexture, _screenBounds, Color.White);
			}
			_spriteBatch.End ();

            _fpsCounter.IncrementCounter();
            base.Draw(gameTime);
		}
        
        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        public void Exit()
        {
            _assetsManager.Unload();
            _networkManager.Disconnect();
            base.UnloadContent();
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;
            if (screen.RendertargetWidth == 0 && screen.RendertargetHeight == 0)
            {
                screen.RendertargetWidth = _screenBounds.Width;
                screen.RendertargetHeight = _screenBounds.Height;
            }
            screen.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screen.RendertargetWidth, screen.RendertargetHeight);
            _screens.Insert(0,screen);
            _assetsManager.AddScreensAssets(screen.Assets);

            _screens[0].Initialize();
        }

        /// <summary>
        /// Removes a screen from the screen manager.
        /// </summary>
        public void RemoveScreen(Screen screen)
        {
            _screens.Remove(screen);
            _assetsManager.RemoveScreensAssets(screen.Assets);
        }

        /// <summary>
        /// Replace a screen with another one. Useful when there is only one screen active and the screens contain the same assets.
        /// </summary>
        public void ReplaceScreen(Screen screen)
        {
            //remove old screen
            _screens.Remove(_screens[0]);

            //start loading the new screen
            screen.ScreenManager = this;
            if (screen.RendertargetWidth == 0 && screen.RendertargetHeight == 0)
            {
                screen.RendertargetWidth = _screenBounds.Width;
                screen.RendertargetHeight = _screenBounds.Height;
            }
            screen.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screen.RendertargetWidth, screen.RendertargetHeight);
            _screens.Insert(0, screen);
            _assetsManager.AddScreensAssets(screen.Assets);

            //remove the old screens assets
            _assetsManager.RemoveScreensAssets(screen.Assets);

            _screens[0].Initialize();
        }

        
    }
}
