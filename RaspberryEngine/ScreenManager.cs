using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        bool NetworkEnabled = false;

        private NetworkManager NetworkManager;
        public NetworkManager Network
        {
            get { return NetworkManager; }
        }

        private AssetsManager AssetsManager;
        public AssetsManager Assets
        {
            get { return AssetsManager; }
        }

        private GraphicsDeviceManager Graphics;

    	private FPSCounter _fpsCounter;
        List<Screen> screens = new List<Screen>();

        private SpriteBatch spriteBatch;
        private Rectangle screenBounds;
        public Rectangle ScreenBounds
        {
            get { return screenBounds; }
        }
        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

       

        #endregion

        /// <summary>
        /// A constructor with Network disabled
        /// </summary>
        public ScreenManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            NetworkEnabled = false;

            AssetsManager = new AssetsManager(Game.Content);
            Graphics = graphics;
            _fpsCounter = new FPSCounter();
        }

        /// <summary>
        /// A constructor with Network enabled
        /// </summary>
        public ScreenManager(Game game, GraphicsDeviceManager graphics, string Server_ip, int Port, string UserName, string Password)
            : base(game)
        {
            NetworkEnabled = true;
            NetworkManager = new NetworkManager(Server_ip, Port, UserName, Password);

            AssetsManager = new AssetsManager(Game.Content);
            Graphics = graphics;
            _fpsCounter = new FPSCounter();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            screenBounds = new Rectangle(0,0,Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight); // This is used by the screens to get the size for the default rendertarget
            spriteBatch = new SpriteBatch(base.GraphicsDevice);
            base.LoadContent();
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        public void Exit()
        {
            AssetsManager.Unload();
            NetworkManager.Disconnect();
            base.UnloadContent();
        }

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (NetworkEnabled)
                NetworkManager.Update();
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screens[0].Update(gameTime);

			// Make sure to update the fpscounter
			_fpsCounter.Update(gameTime);

            base.Update(gameTime);
		}

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw (GameTime gameTime)
        {
        	_fpsCounter.IncrementFrameCount();

			GraphicsDevice.Clear (Color.CornflowerBlue);
			
			//Update the draw for the active screen
			screens [0].Draw (gameTime);
			
			spriteBatch.Begin ();
			foreach (Screen s in screens)
            {
				spriteBatch.Draw (s.RenderTexture, screenBounds, Color.White);
			}
			spriteBatch.End ();

		}

        /// <summary>
        /// Prints a string for debugging.
        /// </summary>
        public override string ToString()
        {
            return "Wop wop nothing here to see, U MAD?";
            /*
            List<string> screenNames = new List<string>();

            foreach (Screen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Console.WriteLine(string.Join(", ", screenNames.ToArray()));

            //Calculate FPS
            float ElapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _TotalTime += ElapsedTime;
            if (_TotalTime >= 1)
            { _DisplayFPS = _FPS; _FPS = 0; _TotalTime = 0; }
            _FPS += 1;

            Console.WriteLine("FPS: " + _DisplayFPS);
            Console.WriteLine("Particles: " + _Particles.Count);
            Console.WriteLine("Emitters: " + _Emitters.Count);
            Console.WriteLine("Sprites: " + _Sprites.Count);
            Console.WriteLine("Textures: " + _Textures.Count);
            Console.WriteLine("Camera Position: " + _Camera._pos.ToString());*/
		}

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;
            if (screen.RendertargetWidth == 0 && screen.RendertargetHeight == 0)
            {
                screen.RendertargetWidth = screenBounds.Width;
                screen.RendertargetHeight = screenBounds.Height;
            }
            screen.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screen.RendertargetWidth, screen.RendertargetHeight);
            screens.Insert(0,screen);
            AssetsManager.AddScreensAssets(screen.Assets);

            screens[0].Initialize();
        }

        /// <summary>
        /// Removes a screen from the screen manager.
        /// </summary>
        public void RemoveScreen(Screen screen)
        {
            screens.Remove(screen);
            AssetsManager.RemoveScreensAssets(screen.Assets);
        }

        /// <summary>
        /// Removes a screen from the screen manager and adds an other.
        /// </summary>
        public void ReplaceScreen(Screen screen)
        {
            //remove old screen
            screens.Remove(screens[0]);

            //start loading the new screen
            screen.ScreenManager = this;
            if (screen.RendertargetWidth == 0 && screen.RendertargetHeight == 0)
            {
                screen.RendertargetWidth = screenBounds.Width;
                screen.RendertargetHeight = screenBounds.Height;
            }
            screen.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screen.RendertargetWidth, screen.RendertargetHeight);
            screens.Insert(0, screen);
            AssetsManager.AddScreensAssets(screen.Assets);

            //remove the old screens assets
            AssetsManager.RemoveScreensAssets(screen.Assets);

            screens[0].Initialize();
        }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public Screen[] GetScreens()
        {
            return screens.ToArray();
        }

		public bool HighRes
		{
            get { return AssetsManager.HighRes; }
            set { AssetsManager.HighRes = value; }
		}

    	public int FrameRate{
    		get{
    			return _fpsCounter.FrameRate;
    		}
    	}
    }
}
