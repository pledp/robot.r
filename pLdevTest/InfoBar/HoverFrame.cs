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
        public string infoTitle;
        public string infoText;
        string infoExample;
        Texture2D borderTexture;
        public HoverFrame(string title, string text, string example, Color frameColor)
        {
            infoText = text;
            infoTitle = title;
            infoExample = example;

            borderTexture = new Texture2D(Game1._graphics.GraphicsDevice, 1, 1);
            this.borderTexture.SetData(new[] { frameColor });
        }

        public void Update(GameTime gameTime, int scrollOffset)
        {
            MouseState mouseState = Mouse.GetState();

            if(mouseState.Y < ((int)GlobalThings.smallerFont.MeasureString(infoExample).Y + (int)GlobalThings.smallerFont.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 24 + 50))
            {
                pos = new Vector2(mouseState.X + 30, mouseState.Y + 30 - scrollOffset);
            }
            else
            {
                pos = new Vector2(mouseState.X + 30, mouseState.Y + 30 - scrollOffset - ((int)GlobalThings.smallerFont.MeasureString(infoExample).Y + (int)GlobalThings.smallerFont.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 24 + 50));
            }
            
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

            _spriteBatch.Draw(GlobalThings.enemyTexture, new Rectangle((int)pos.X + 10 , (int)pos.Y + 10, offsetX + 20, (int)GlobalThings.smallerFont.MeasureString(infoExample).Y + (int)GlobalThings.smallerFont.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 20 + 20), Color.Black * 0.5f);
            _spriteBatch.Draw(this.borderTexture, new Rectangle((int)pos.X - 12, (int)pos.Y - 12, offsetX + 24, (int)GlobalThings.smallerFont.MeasureString(infoExample).Y + (int)GlobalThings.smallerFont.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 24 + 20), Color.White);
            _spriteBatch.Draw(GlobalThings.frameTexture, new Rectangle((int)pos.X - 10, (int)pos.Y - 10, offsetX + 20, (int)GlobalThings.smallerFont.MeasureString(infoExample).Y + (int)GlobalThings.smallerFont.MeasureString(infoText).Y + (int)GlobalThings.font.MeasureString(infoTitle).Y + 20 + 20), Color.White);
            _spriteBatch.DrawString(GlobalThings.font, infoTitle, pos, Color.White);
            _spriteBatch.DrawString(GlobalThings.smallerFont, infoText, new Vector2(pos.X + 10, pos.Y + 50), PlayGround.pgColor2);
            _spriteBatch.DrawString(GlobalThings.smallerFont, infoExample, new Vector2(pos.X + 10, pos.Y + (int)GlobalThings.smallerFont.MeasureString(infoText).Y + (int)GlobalThings.smallerFont.MeasureString(infoTitle).Y + 30), GlobalThings.orangeColor);

        }
    }
}
