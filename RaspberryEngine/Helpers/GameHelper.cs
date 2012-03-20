using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Extrude.Framework.Helpers
{
    public static class GameHelper
    {
        public static Rectangle getAnimationFrame(byte frame, Texture2D texture)
        {
            // update the frame counter
            byte nrOfFrames = (byte)(texture.Width / texture.Height);
            while (frame > nrOfFrames)
                frame -= nrOfFrames;

            Rectangle sourcerect = new Rectangle(texture.Height * frame, 0,
                texture.Height, texture.Height);

            return sourcerect;
        }

        public static Vector2 getOrgin(Texture2D texture)
        {
            return new Vector2(texture.Width/2,texture.Height/2);
        }
        public static Vector2 getOrgin(Rectangle rectangle)
        {
            return new Vector2(rectangle.Width / 2, rectangle.Height / 2);
        }
    }
}
