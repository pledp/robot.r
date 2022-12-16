using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public class InfoWidget
    {
        public string widgetTitle;
        public string widgetText;

        bool drawFrame;
        public bool DrawFrame
        {
            get { return drawFrame; }
        }

        HoverFrame hoverFrame;
        Color textColor;

        Vector2 pos;
        Rectangle widget;
        public InfoWidget(string title, string text, string example)
        {
            widgetText = text;
            widgetTitle = title;
            hoverFrame = new HoverFrame(title, text, example, GlobalThings.orangeColor);
            textColor = new Color(240, 247, 244);
        }
        public void Update(GameTime gameTime, Vector2 helpBar)
        {
            MouseState mouseState = Mouse.GetState();
            pos = new Vector2(helpBar.X + 30, helpBar.Y + 50);
            widget = new Rectangle((int)pos.X, (int)pos.Y, 440, (int)GlobalThings.font.MeasureString(widgetTitle).Y + 30);
            if (GlobalThings.EnterArea(new Rectangle((int)widget.X, (int)widget.Y + (int)HelpBar._scrollOffset.Y, widget.Width, widget.Height), mouseState))
            {
                drawFrame = true;
            }
            else
            {
                drawFrame = false;
            }

            if (drawFrame)
            {
                hoverFrame.infoText = widgetText;
                hoverFrame.infoTitle = widgetTitle;
                hoverFrame.Update(gameTime, (int)HelpBar._scrollOffset.Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GlobalThings.frameTexture,widget, Color.White);
            spriteBatch.DrawString(GlobalThings.font, widgetTitle, new Vector2(widget.X + 10, widget.Y + 10), textColor);
        }
        public void DrawHoverFrame(SpriteBatch spriteBatch)
        {
            if (drawFrame)
            {
                hoverFrame.Draw(spriteBatch);
            }
        }
    }
}
