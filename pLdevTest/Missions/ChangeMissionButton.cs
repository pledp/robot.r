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
    public class ChangeMissionButton
    {
        Rectangle arrow;
        Rectangle arrowForward;
        MouseState lastMouseState;

        public ChangeMissionButton(GraphicsDevice _graphics)
        {
            arrow = new Rectangle(_graphics.Viewport.Width - 590, _graphics.Viewport.Height - 100, 40, 40);
            arrowForward = new Rectangle(_graphics.Viewport.Width - 100, _graphics.Viewport.Height - 100, 40, 40);
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(arrow, mouseState) && mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                if(MissionHandler.Mission > 0)
                {
                    MissionHandler.Mission = MissionHandler.Mission - 1;
                    MissionHandler.CurrWorldMission--;
                    MissionHandler.ResetMission();
                }
            }
            if (GlobalThings.EnterArea(arrowForward, mouseState) && mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                if(MissionHandler.Mission <= MissionHandler.CompletedMissions && MissionHandler.CompletedMissions != 0)
                {
                    MissionHandler.Mission = MissionHandler.Mission + 1;
                    MissionHandler.CurrWorldMission++;
                    MissionHandler.ResetMission();
                }
                
            }

            lastMouseState = mouseState;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (MissionHandler.Mission > 0)
            {
                _spriteBatch.Draw(GlobalThings.greenTexture, arrow, Color.White);
                _spriteBatch.DrawString(GlobalThings.font, "<", new Vector2(arrow.X + 10, arrow.Y), Color.White);
            }

            if (MissionHandler.Mission <= MissionHandler.CompletedMissions && MissionHandler.CompletedMissions != 0)
            {
                _spriteBatch.Draw(GlobalThings.greenTexture, arrowForward, Color.White);
                _spriteBatch.DrawString(GlobalThings.font, ">", new Vector2(arrowForward.X + 10, arrowForward.Y), Color.White);
            }

        }
        public void UpdateProportions(GraphicsDevice _graphics)
        {
            arrow = new Rectangle(_graphics.Viewport.Width - 590, _graphics.Viewport.Height - 100, 40, 40);
            arrowForward = new Rectangle(_graphics.Viewport.Width - 100, _graphics.Viewport.Height - 100, 40, 40);
        }
    }
}
