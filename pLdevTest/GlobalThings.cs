using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public static class GlobalThings
    {
        public static bool EnterArea(Rectangle area, MouseState mouseState)
        {
            if (mouseState.X < area.X + area.Width &&
                mouseState.X > area.X &&
                mouseState.Y < area.Y + area.Height &&
                mouseState.Y > area.Y)
            {
                return true;
            }
            return false;
        }
    }
}
