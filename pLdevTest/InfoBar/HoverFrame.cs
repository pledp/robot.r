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
    public class HoverFrame
    {
        Vector2 pos;
        string infoTitle;
        string infoText;
        public HoverFrame(string title, string text)
        {
            infoText = text;
            infoTitle = title;
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            pos = new Vector2(mouseState.X + 30, mouseState.Y + 30 - (int)codeInput._scrollOffset.Y);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            int offsetX;
            if((int)GlobalThings.font.MeasureString(infoTitle).X > (int)GlobalThings.smallerFont.MeasureString(infoText).X)
            {
                offsetX = (int)GlobalThings.font.MeasureString(infoTitle).X;
            }
            else
            {
                offsetX = (int)GlobalThings.smallerFont.MeasureString(infoText).X + 10;
            }

            _spriteBatch.Draw(GlobalThings.enemyTexture, new Rectangle((int)pos.X + 10 , (int)pos.Y + 10, offsetX + 20, (int)GlobalThings.font.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 20), Color.Black * 0.5f);
            _spriteBatch.Draw(GlobalThings.enemyTexture, new Rectangle((int)pos.X - 12, (int)pos.Y - 12, offsetX + 24, (int)GlobalThings.font.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 24), Color.White);
            _spriteBatch.Draw(GlobalThings.frameTexture, new Rectangle((int)pos.X - 10, (int)pos.Y - 10, offsetX + 20, (int)GlobalThings.font.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 20), Color.White);
            _spriteBatch.DrawString(GlobalThings.font, infoTitle, pos, Color.White);
            _spriteBatch.DrawString(GlobalThings.smallerFont, infoText, new Vector2(pos.X + 10, pos.Y + 50), PlayGround.pgColor2);
        }
    }
}
