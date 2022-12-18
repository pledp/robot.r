using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace pLdevTest
{
    public class ScrollBar
    {
        public Rectangle bounds;
        public Rectangle grip;
        private MouseState mouseState;
        private Texture2D scrollTexture;
        private Texture2D gripTexture;
        private float mouseRelative;
        private bool gripActive;
        private int gripMax;
        private int finalValue;

        public ScrollBar(Rectangle rectangle, Texture2D scrollbarTexture, Texture2D gripBarTexture, int max, int value)
        {
            bounds = rectangle;
            value = Math.Min(Math.Max(value, 0), max);
            gripMax = max;

            grip = new Rectangle(bounds.Left, bounds.Y, bounds.Width, bounds.Height/gripMax);
            scrollTexture = scrollbarTexture;
            gripTexture = gripBarTexture;

            value = Convert.ToInt16(max - (mouseRelative / (bounds.Height - grip.Height)) * max);
        }
        public void Update()
        {
            mouseState = Mouse.GetState();
            if (EnterScroll() && mouseState.LeftButton == ButtonState.Pressed)
            {
                gripActive = true;
            } else
            {
                gripActive = false;
            }
            if(gripActive)
            {
                mouseRelative = mouseState.Y - (bounds.Y + grip.Height / 2);
                mouseRelative = Math.Min(mouseRelative, bounds.Height - grip.Height);
                mouseRelative = Math.Max(0, mouseRelative);
                grip.Y = Convert.ToInt32(bounds.Y + mouseRelative);
                finalValue = Convert.ToInt32((mouseRelative / (bounds.Height - grip.Height)) * gripMax);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(scrollTexture, bounds, Color.Gray);
            spriteBatch.Draw(gripTexture, grip, Color.Black);
        }

        private bool EnterScroll()
        {
            if (mouseState.X < grip.X + grip.Width &&
                mouseState.X > grip.X &&
                mouseState.Y < grip.Y + grip.Height &&
                mouseState.Y > grip.Y)
            {
                return true;
            }
            return false;
        }
        public void UpdateProportions(Rectangle newBounds)
        {
            bounds.Y = newBounds.Y + 50;
            bounds.X = newBounds.X + newBounds.Width;
            bounds.Height = newBounds.Height - 50;
            if(finalValue < 2)
            {
                grip.Y = Convert.ToInt32(newBounds.Y + (finalValue * grip.Height)) + 50;
            } else
            {
                grip.Y = Convert.ToInt32(newBounds.Y + ((finalValue-1) * grip.Height)) + 50;
            }
            grip.X = bounds.Left;
            grip.Height = bounds.Height / gripMax;
        }
    }
}
