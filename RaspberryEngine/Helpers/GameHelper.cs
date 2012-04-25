using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaspberryEngine.Helpers
{
    public static class GameHelper
    {
        public static Rectangle GetAnimationFrame(byte frame, Texture2D texture)
        {
            // update the frame counter
            byte nrOfFrames = (byte)(texture.Width / texture.Height);
            while (frame > nrOfFrames)
                frame -= nrOfFrames;

            Rectangle sourcerect = new Rectangle(texture.Height * frame, 0,
                texture.Height, texture.Height);

            return sourcerect;
        }

        public static Vector2 GetOrigin(Texture2D texture)
        {
            return new Vector2(texture.Width/2,texture.Height/2);
        }
        public static Vector2 GetOrigin(Rectangle rectangle)
        {
            return new Vector2(rectangle.Width / 2, rectangle.Height / 2);
        }
    }
}
