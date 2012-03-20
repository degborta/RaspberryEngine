using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Extrude.Framework.Windows.Components {
	class FPSCounter {
		// FPS status
		private int _frameRate = 0;
		private int _frameCounter = 0;
		private TimeSpan _elapsedTime = TimeSpan.Zero;
		
		public int FrameRate { 
			get{
				return _frameRate;
			}
		}
        public FPSCounter(){}

        public void Update(GameTime gameTime)
        {
			//Update framerate
			_elapsedTime += gameTime.ElapsedGameTime;

			if (_elapsedTime > TimeSpan.FromSeconds(1)) {
				_elapsedTime -= TimeSpan.FromSeconds(1);
				_frameRate = _frameCounter;
				_frameCounter = 0;
			}
        }

		public void IncrementFrameCount(){
			_frameCounter++;
		}
	}
}
