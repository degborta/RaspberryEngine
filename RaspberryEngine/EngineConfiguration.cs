using Microsoft.Xna.Framework;
using RaspberryEngine.Screens;

namespace RaspberryEngine
{
    public class EngineConfiguration
    {
        public bool EnableFixedTimeStep { get; set; }
        public bool EnableVerticalSync { get; set; }
        public bool EnableFullScreen { get; set; }
        public int TargetFrameRate { get; set; }
        public int PreferredBackBufferWidth { get; set; }
        public int PreferredBackBufferHeight { get; set; }
        public DisplayOrientation SupportedOrientations { get; set; }

        public string ContentDirectory { get; set; }
        public bool EnableHighQualityContent { get; set; }

        public string AppId { get; set; }
        public int Port { get; set; }
        public string ServerIp { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
