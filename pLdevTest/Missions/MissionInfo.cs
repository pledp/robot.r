using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public class MissionInfo
    {
        Texture2D whiteRectangle;
        string[] missionCounterText = { "Mission: ", "", "/", ""};
        Color[] colors = { Color.Black, PlayGround.pgColor, Color.Black, PlayGround.pgColor };
        public MissionInfo(GraphicsDevice _graphics)
        {
            whiteRectangle = new Texture2D(_graphics, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }
        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            Vector2 offset = Vector2.Zero;
            int missionFormat = MissionHandler.Mission + 1;
            missionCounterText[1] = missionFormat.ToString();
            missionCounterText[3] = MissionHandler.Missions.Length.ToString();

            // Draw frame
            _spriteBatch.Draw(whiteRectangle, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 650, 0, 650, _graphics.GraphicsDevice.Viewport.Height), Color.White);

            _spriteBatch.Draw(whiteRectangle, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 600, 460, 550, 5), Game1.background);
            _spriteBatch.Draw(whiteRectangle, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 600, _graphics.GraphicsDevice.Viewport.Height - 140, 550, 5), Game1.background);
            _spriteBatch.DrawString(Game1.font, MissionHandler.Missions[MissionHandler.Mission], new Vector2(_graphics.GraphicsDevice.Viewport.Width - 600, 410), PlayGround.pgColor);

            for(int x = 0; x < missionCounterText.Length; x++)
            {
                _spriteBatch.DrawString(Game1.font, missionCounterText[x], new Vector2((_graphics.GraphicsDevice.Viewport.Width) - 600 + offset.X, _graphics.GraphicsDevice.Viewport.Height - 115), colors[x]);
                offset.X += Game1.font.MeasureString(missionCounterText[x]).X;
            }
        }
    }
}
