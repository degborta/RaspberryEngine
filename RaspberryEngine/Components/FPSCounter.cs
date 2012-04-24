using System;
using Microsoft.Xna.Framework;

namespace RaspberryEngine.Components 
{
	public class FPSCounter
    {
		// FPS status
		private int _frameRate = 0;
        private int _frameCounter = 0;
		private TimeSpan _elapsedTime = TimeSpan.Zero;
		
        public override string  ToString()
		{
            return "FPS: " + _frameRate.ToString();
		}

        public void Update(GameTime gameTime)
        {
			//Update framerate
			_elapsedTime += gameTime.ElapsedGameTime;

			if (_elapsedTime > TimeSpan.FromSeconds(1)) 
            {
				_elapsedTime -= TimeSpan.FromSeconds(1);
                _frameRate = _frameCounter;
                _frameCounter = 0;
			}
        }

        public void IncrementCounter()
        {
            _frameCounter++;
        }

	}
}
