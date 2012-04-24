using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using RaspberryEngine.Screens;

namespace RaspberryEngine.Components
{
    public enum MenuAlign
    {
        Center, Left, Right
    }

    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public class Menu
    {
        string fontKey;
        Rectangle optionsBound;
        List<string> optionsMenu;
        TouchCollection Touch;

        //Public settings
        public float spacing = 1.2f;
        public Vector2 startPosition = new Vector2(10, 10);
        public MenuAlign align = MenuAlign.Left;

        string optionPressed;
        public string OptionPressed
        {
            get { return optionPressed; }
        }

        public Menu(Vector2 startPosition, string fontKey)
        {
            //The font that will be used for the menu
            this.fontKey = fontKey;
            this.spacing = 1.2f;
            this.align = MenuAlign.Center;
            this.startPosition = startPosition;

            optionsMenu = new List<string>();
            optionPressed = string.Empty;
            optionsBound = Rectangle.Empty;
        }

        public Menu(Vector2 startPosition, float spacing, MenuAlign align, string fontKey)
        {
            //The font that will be used for the menu
            this.fontKey = fontKey;
            this.spacing = spacing;
            this.align = align;
            this.startPosition = startPosition;

            optionsMenu = new List<string>();
            optionPressed = string.Empty;
            optionsBound = Rectangle.Empty;
        }

        public void Update(TouchCollection Touch)
        { this.Touch = Touch; }

        public void Draw(Screen screen)
        {
            //Reset button state
            optionPressed = null;

            //Update every button and see if any is pressed
            for (int s = 0; s < optionsMenu.Count; s++)
            {
                Rectangle bound = optionsBound;
                Vector2 position = new Vector2((startPosition.X) - bound.Width / 2, startPosition.Y + (s * (optionsBound.Height * spacing)));
                bound.X = (int)position.X;
                bound.Y = (int)position.Y;

                //Check what alignment we want on the button text
                float orginX;
                switch (align)
                {
                    case MenuAlign.Center:
                        orginX = ((SpriteFont)screen.ScreenManager.AssetsManager.GetAsset(fontKey)).MeasureString(optionsMenu[s]).X / 2 - bound.Width / 2;
                        break;
                    case MenuAlign.Right:
                        orginX = ((SpriteFont)screen.ScreenManager.AssetsManager.GetAsset(fontKey)).MeasureString(optionsMenu[s]).X - bound.Width;
                        break;
                    default:
                        orginX = 0;
                        break;
                }

                //Add a draw of the button
                screen.AddText(fontKey, optionsMenu[s], position, new Vector2(orginX, 0), Vector2.One, 0, Color.White, 0);

                //check if any button was pressed
                if (Touch == null)
                    return;

                foreach (TouchLocation t in Touch)
                {
                    if (bound.Contains(t.Position))
                    {
                        optionPressed = optionsMenu[s];
                        break;
                    }
                    if (optionPressed != null)
                        break;
                }
            }
        }

        public void AddOptions(Screen screen, string[] newMenuOptions)
        {
            //Add all the options to the menu
            foreach (string s in newMenuOptions)
            {
                //measure the current string to find the largest string (we use this as the size of the buttons)
                Vector2 measure = ((SpriteFont)screen.ScreenManager.AssetsManager.GetAsset(fontKey)).MeasureString(s.ToUpper());

                if (measure.X > optionsBound.Width)
                    optionsBound.Width = (int)Math.Ceiling(measure.X);
                if (measure.Y > optionsBound.Height)
                    optionsBound.Height = (int)Math.Ceiling(measure.Y);

                //add to option list
                optionsMenu.Add(s.ToUpper());
            }
        }


    }
}
