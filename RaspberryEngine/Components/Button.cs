using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace RaspberryEngine.Components
{
    public class Button
    {
        public string Name;
        public bool PrewTouch;

        public Button(string name) { Name = name; PrewTouch = false; }

        public virtual Rectangle Bound
        {
            get { return Rectangle.Empty; }
            set { }
        }

        public virtual bool Update(TouchCollection Touch)
        {
            return false;
        }
    }

    public class ButtonRectangle : Button
    {
        public Rectangle bound;

        public ButtonRectangle(string name, Rectangle bound)
            : base(name)
        { bound = bound; }

        public override bool Update(TouchCollection Touch)
        {
            foreach (TouchLocation t in Touch)
            {
                if (bound.Contains(t.Position))
                {
                    if (PrewTouch == false)
                    { PrewTouch = true; return true; }
                }
                else PrewTouch = false;
            }
            return false;
        }

        public override Rectangle Bound
        {
            get { return bound; }
            set { bound = value; }
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
                {
                    if (PrewTouch == false)
                    { PrewTouch = true; return true; }
                }
                else PrewTouch = false;
            }
            return false;
        }

        public override Rectangle Bound
        {
            get
            {
                return new Rectangle(
                   (int)(Position.X - Radius),
                   (int)(Position.Y - Radius),
                   (int)(Position.X + Radius),
                   (int)(Position.Y + Radius));
            }
            set
            {
                Position = new Vector2(value.Center.X, value.Center.Y);
                Radius = (value.Height + value.Width) / 4;
            }
        }
    }
}
