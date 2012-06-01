using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
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
        private readonly GraphicsDeviceManager _graphics;

        private readonly EngineConfiguration _configuration;

        public SpriteBatch SpriteBatch { get; private set; }

        public Rectangle ScreenBounds { get; private set; }

        public AssetsManager AssetsManager { get; private set; }

        public bool NetworkEnabled { get; set; }

        public FPSCounter FpsCounter { get; private set; }

        public List<Screen> Screens { get; private set; }

        public NetworkManager NetworkManager { get; private set; }

        /// <summary>
        /// A constructor with Network disabled
        /// </summary>
        public Engine(EngineConfiguration configuration)
        {
            Screens = new List<Screen>();
            IsMouseVisible = true;
            IsFixedTimeStep = configuration.EnableFixedTimeStep;

            _graphics = new GraphicsDeviceManager(this)
                            {
                                PreferredBackBufferWidth = configuration.PreferredBackBufferWidth,
                                PreferredBackBufferHeight = configuration.PreferredBackBufferHeight,
                                IsFullScreen = configuration.EnableFullScreen,
                                SynchronizeWithVerticalRetrace = configuration.EnableVerticalSync,
                                SupportedOrientations = configuration.SupportedOrientations
                            };
            _graphics.DeviceReset += OnGraphicsComponentDeviceReset;

            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / configuration.TargetFrameRate);
            
            Content.RootDirectory = configuration.ContentDirectory;

            NetworkManager = new NetworkManager
                                 {
                                     AppId = configuration.AppId,
                                     Port = configuration.Port,
                                     ServerIp = configuration.ServerIp,
                                     Username = configuration.Username,
                                     PassWord = configuration.Password
                                 };

            FpsCounter = new FPSCounter();

            AssetsManager = new AssetsManager(Content)
                                {
                                    HighRes = configuration.EnableHighQualityContent
                                };

            ScreenBounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight); // This is used by the screens to get the size for the default rendertarget
            
            Components.Add(new GamerServicesComponent(this));

            _configuration = configuration;
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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (NetworkEnabled)
                NetworkManager.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            Screens[0].Update(gameTime);

            // Make sure to update the fpscounter
            FpsCounter.Update(gameTime);
            FpsCounter.IncrementCounter();
            base.Update(gameTime);
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Update the draw for the active screen
            Screens[0].Draw(gameTime);

            SpriteBatch.Begin();
            foreach (Screen s in Screens)
            {
                SpriteBatch.Draw(s.RenderTexture, ScreenBounds, Color.White);
            }
            SpriteBatch.End();

            FpsCounter.IncrementCounter();
            base.Draw(gameTime);
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
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;
            if (screen.RendertargetWidth == 0 && screen.RendertargetHeight == 0)
            {
                screen.RendertargetWidth = ScreenBounds.Width;
                screen.RendertargetHeight = ScreenBounds.Height;
            }
            screen.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screen.RendertargetWidth, screen.RendertargetHeight);
            Screens.Insert(0, screen);
            AssetsManager.AddScreensAssets(screen.Assets);

            Screens[0].Initialize();
        }

        /// <summary>
        /// Removes a screen from the screen manager.
        /// </summary>
        public void RemoveScreen(Screen screen)
        {
            Screens.Remove(screen);
            AssetsManager.RemoveScreensAssets(screen.Assets);
        }

        /// <summary>
        /// Replace a screen with another one. Useful when there is only one screen active and the screens contain the same assets.
        /// </summary>
        public void ReplaceScreen(Screen screen)
        {
            //remove old screen
            Screens.Remove(Screens[0]);

            //start loading the new screen
            screen.ScreenManager = this;
            if (screen.RendertargetWidth == 0 && screen.RendertargetHeight == 0)
            {
                screen.RendertargetWidth = ScreenBounds.Width;
                screen.RendertargetHeight = ScreenBounds.Height;
            }
            screen.RenderTarget = new RenderTarget2D(this.GraphicsDevice, screen.RendertargetWidth, screen.RendertargetHeight);
            Screens.Insert(0, screen);
            AssetsManager.AddScreensAssets(screen.Assets);

            //remove the old screens assets
            AssetsManager.RemoveScreensAssets(screen.Assets);

            Screens[0].Initialize();
        }


    }
}
