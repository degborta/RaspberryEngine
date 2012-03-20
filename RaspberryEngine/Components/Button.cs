using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace RaspberryEngine.Components
{
    public class Button
    {
        public string Name;

        public Button(string name) { Name = name; }

        public virtual Rectangle getBound()
        {
            return Rectangle.Empty;
        }

        public virtual bool Update(TouchCollection Touch)
        {
            return false;
        }
    }

    public class ButtonRectangle : Button
    {
        public Rectangle Bound;

        public ButtonRectangle(string name, Rectangle bound) 
            : base(name)
        { Bound = bound; }

        public override bool Update(TouchCollection Touch)
        {
            foreach (TouchLocation t in Touch)
            {
                if (Bound.Contains(t.Position))
                { return true; }
            }
            return false;
        }

        public override Rectangle getBound()
        {
            return Bound;
        }
    }

    public class ButtonCircle : Button
    {
        public Vector2 Position;
        public float Radius;

        public ButtonCircle(string name, Vector2 position, float radius)
            : base(name)
        {
            Position = position;
            Radius = radius;
        }

        public override bool Update(TouchCollection Touch)
        {
            foreach (TouchLocation t in Touch)
            {
                if ((Position - t.Position).Length() < Radius)
                { return true; }
            }
            return false;
        }

        public override Rectangle getBound()
        {
            return new Rectangle(
                (int)(Position.X - Radius), 
                (int)(Position.Y - Radius), 
                (int)(Position.X + Radius), 
                (int)(Position.Y + Radius));
        }
    }
}
