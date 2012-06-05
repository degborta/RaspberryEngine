using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RaspberryEngine.Screens;

namespace RaspberryEngine.Components
{
    public class KeyTouch
    {
        public string _descriptionText { private get; set; }
        public string _inputText { get; private set; }

        private readonly bool _passwordMode;

        private readonly Rectangle[,] _r;
        private string[,] _c;

        //Visuals
        private readonly int _buttonWidth;
        private Rectangle _textfield;
        private readonly string[] _textureKeys;
        private readonly string _fontKey;

        public KeyTouch(string description, bool passwordMode, string[] texture, string font, Rectangle area)
        {
            _descriptionText = description;
            //This is the size of the buttons collision box
            //It is made to fit the resolution
            _buttonWidth = (int)Math.Floor(area.Width / 10f);

            //**** ** *** ******** ****... ops pasword mode was on ;P
            _passwordMode = passwordMode;

            //Visuals
            _textureKeys = texture;
            _fontKey = font;

            _inputText = "";
            _c = GetChar(true);

            //Create the button rectangles
            _r = new Rectangle[_c.GetLength(0), _c.GetLength(1)];
            for (int y = 0; y < _r.GetLength(0); y++)
                for (int x = 0; x < _r.GetLength(1); x++)
                {
                    int multi = 1;
                    if (x == _r.GetLength(1) - 1)
                        multi = 2;

                    _r[y, x] = new Rectangle((x * _buttonWidth) + _buttonWidth / 2,
                                             ((y * _buttonWidth) + _buttonWidth / 2),
                                             _buttonWidth * multi,
                                             _buttonWidth);
                }
            
            //Create the textfield rectangle
            _textfield = new Rectangle(_buttonWidth / 2,
                                     (_buttonWidth / 2),
                                      _buttonWidth * 9,
                                      _buttonWidth);
        }

        public bool Update(Vector2? cursor)
        {
            string s = GetInput(cursor);
            switch (s)
            {
                case "Back":
                    if (_inputText.Length > 0)
                        _inputText = _inputText.Remove(_inputText.Length - 1, 1);
                    break;

                case "@123":
                    _c = GetChar(false);
                    break;

                case "ABC":
                    _c = GetChar(true);
                    break;

                case "Space":
                    _inputText += " ";
                    break;

                case "OK":
                    return true;

                default:
                    _inputText += s;
                    return false;
            }
            return false;
        }

        private string GetInput(Vector2? cursor)
        {
            if (cursor == null)
                return "";

            var touchX = (int)Math.Round(cursor.Value.X);
            var touchY = (int)Math.Round(cursor.Value.Y);

            for (int y = 0; y < _r.GetLength(0); y++)
                for (int x = 0; x < _r.GetLength(1); x++)
                    if (_r[y, x].Contains(touchX, touchY))
                        return _c[y, x];
            return "";
        }

        public void Draw(Screen screen, GameTime time)
        {
            DrawTextfields(screen, time);
            for (int y = 2; y < _r.GetLength(0); y++)
                for (int x = 0; x < _r.GetLength(1); x++)
                    DrawButton(x, y,  screen);
        }

        private void DrawButton(int x, int y, Screen screen)
        {
            int buttonW = (int)(_buttonWidth * 0.2f);
            Rectangle left = new Rectangle(
                _r[y,x].X,
                _r[y, x].Y,
                buttonW,
                _r[y, x].Height);

            Rectangle center = new Rectangle(
                _r[y, x].X + buttonW,
                _r[y, x].Y,
                _r[y, x].Width - (buttonW * 2),
                _r[y, x].Height);

            Rectangle right = new Rectangle(
                _r[y, x].X + _r[y, x].Width - buttonW,
                _r[y, x].Y,
                buttonW,
                _r[y, x].Height);

            screen.AddSprite(true, _textureKeys[0], left, Vector2.Zero, 0, Color.White, 1, null, false);
            screen.AddSprite(true, _textureKeys[1], center, Vector2.Zero, 0, Color.White, 1, null, false);
            screen.AddSprite(true, _textureKeys[2], right, Vector2.Zero, 0, Color.White, 1, null, false);
            screen.AddText(_fontKey, _c[y, x], new Vector2(
                                              (_r[y, x].X + (_r[y, x].Width / 2)) -
                                              ((SpriteFont)
                                               screen.ScreenManager.AssetsManager.GetAsset(_fontKey)).
                                                  MeasureString(_c[y, x]).X / 2,
                                              (_r[y, x].Y + (_r[y, x].Height / 2)) -
                                              ((SpriteFont)
                                               screen.ScreenManager.AssetsManager.GetAsset(_fontKey)).
                                                  MeasureString(_c[y, x]).Y / 2),
                           Vector2.Zero, Vector2.One,
                           0, Color.White, 0);
        }

        private void DrawTextfields(Screen screen, GameTime time)
        {
            //Draw field background
            int buttonW = (int)(_buttonWidth * 0.2f);
            Rectangle left = new Rectangle(_r[1, 0].X, _r[1, 0].Y, buttonW, _r[1, 0].Height);
            Rectangle center = new Rectangle(_r[1, 0].X + buttonW, _r[1, 0].Y, (_r[1, 0].Width * (_r.GetLength(1) + 1)) - (buttonW * 2), _r[1, 0].Height);
            Rectangle right = new Rectangle(_r[1, 0].X + center.Width + buttonW, _r[1, 0].Y, buttonW, _r[1, 0].Height);

            screen.AddSprite(true, _textureKeys[3], left, Vector2.Zero, 0, Color.White, 1, null, false);
            screen.AddSprite(true, _textureKeys[4], center, Vector2.Zero, 0, Color.White, 1, null, false);
            screen.AddSprite(true, _textureKeys[5], right, Vector2.Zero, 0, Color.White, 1, null, false);

            screen.AddText(_fontKey, _descriptionText, new Vector2(
                                                     _r[0, 0].X + buttonW,
                                                     (_r[0, 0].Y + (_textfield.Height / 2)) -
                                                     ((SpriteFont)screen.ScreenManager.AssetsManager.GetAsset(_fontKey)).MeasureString(_descriptionText).Y / 2),
                                                     Vector2.Zero, Vector2.One,
                                                     0, Color.White, 0);

            //this is the input string that is drawn
            //its the same as _inputText but with a few modifications
            string drawString = _inputText;
            //do the password thing
            if (_passwordMode)
            {
                int stars = drawString.Length - 0; if (stars < 0) stars = 0;
                string s = ""; for (int i = 0; i < stars; i++) s += "*";
            }
            //add the flashing indicator
            if (time.TotalGameTime.Milliseconds / 500 % 2 == 0) drawString += "|";
            //format the string so its not to long for the textfield
            while (((SpriteFont)screen.ScreenManager.AssetsManager.GetAsset(_fontKey)).MeasureString(drawString).X > center.Width) drawString = drawString.Remove(0, 1);

            screen.AddText(_fontKey, drawString, new Vector2(
                                                     _r[1, 0].X + buttonW,
                                                     (_r[1, 0].Y + (_textfield.Height / 2)) -
                                                     ((SpriteFont)screen.ScreenManager.AssetsManager.GetAsset(_fontKey)).MeasureString(drawString).Y / 2),
                                                     Vector2.Zero, Vector2.One,
                                                     0, Color.White, 0);
        }

        static string[,] GetChar(bool chars)
        {
            // !#$%&'*+-/=?^_`{|}~
            // .
            // "(),:;<>@[\]

            if (chars)
                return new string[,]
                     {
                         {" ", " ", " ", " ", " ", " ", " ", " "},
                         {" ", " ", " ", " ", " ", " ", " ", " "},
                         {"A", "B", "C", "D", "E", "F", "G", "Back"},
                         {"H", "I", "J", "K", "L", "M", "N", "@123"},
                         {"O", "P", "Q", "R", "S", "T", "U", "Space"},
                         {"V", "W", "X", "Y", "Z", ",", ".", "OK"}
                     };
            else
                return new string[,]
                     {
                         {" ", " ", " ", " ", " ", " ", " ", " "},
                         {" ", " ", " ", " ", " ", " ", " ", " "},
                         {"7", "8", "9", "/", "!", "?", "&", "Back"},
                         {"4", "5", "6", "*", "<", ">", "$", "ABC"},
                         {"1", "2", "3", "-", "{", "}", "~", "Space"},
                         {"0", ",", ".", "+", "[", "]", "@", "OK"}
                     };
        }
    }
}
