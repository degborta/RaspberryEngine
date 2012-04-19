using System;
using Microsoft.Xna.Framework;

namespace RaspberryEngine.Components {
	class FPSCounter {
		// FPS status
		private int _drawRate = 0;
		private int _drawCounter = 0;


        private int _updateRate = 0;
        private int _updateCounter = 0;
		private TimeSpan _elapsedTime = TimeSpan.Zero;
		
		public string FrameUpdateRate { 
			get{ return "UPS: " + _updateRate.ToString() + " DPS: " + _drawRate.ToString(); }
		}

        public FPSCounter(){}

        public void Update(GameTime gameTime)
        {
			//Update framerate
			_elapsedTime += gameTime.ElapsedGameTime;

			if (_elapsedTime > TimeSpan.FromSeconds(1)) 
            {
				_elapsedTime -= TimeSpan.FromSeconds(1);

				_updateRate = _updateCounter;
                _updateCounter = 0;

                _drawRate = _drawCounter;
                _drawCounter = 0;
			}
        }

		public void IncrementUpdate()
        {
			_updateCounter++;
		}
        public void IncrementDraw()
        {
            _drawCounter++;
        }
	}
}
