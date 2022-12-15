using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace pLdevTest
{
    public class ErrorIndicator
    {
        string errorText;
        Vector2 pos;
        HoverFrame hoverFrame;
        bool drawFrame;

        public ErrorIndicator(Vector2 newPos, string text, string title)
        {
            pos = newPos;
            hoverFrame = new HoverFrame(title, text);
        }
        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(new Rectangle((int)pos.X, (int)pos.Y + (int)codeInput._scrollOffset.Y, 30, 30), mouseState)) 
            {
                drawFrame = true;
            }
            else
            {
                drawFrame = false;
            }

            if(drawFrame)
            {
                hoverFrame.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(GlobalThings.enemyTexture, new Rectangle((int)pos.X, (int)pos.Y, 30, 30), Color.White);

            if(drawFrame)
            {
                hoverFrame.Draw(_spriteBatch);
            }

        }
    }
}
