using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    public class MissionInfo
    {
        Texture2D whiteRectangle;
        string[] missionCounterText = { "Mission: ", "", "/", ""};
        Color[] colors = { Color.Black, PlayGround.pgColor, Color.Black, PlayGround.pgColor };

        private RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        private Vector2 _scrollOffset = Vector2.Zero;
        private Matrix _matrix = Matrix.CreatePerspective(500, 50, 1, 2);
        private MouseState mouseState;
        private MouseState lastMouseState;
        private Rectangle area;
        private Texture2D clipboard;
        private Color clipboardColor;

        public MissionInfo(GraphicsDevice _graphics)
        {
            whiteRectangle = new Texture2D(_graphics, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            area = new Rectangle(_graphics.Viewport.Width - 600, 465, 600, _graphics.Viewport.Height - (140 + 465));
            clipboardColor = new Color(139, 97, 64);

        }

        public void Update(GameTime gameTime, GraphicsDevice _graphics)
        {
            _matrix = Matrix.CreateTranslation(new Vector3(_scrollOffset, 0));
            mouseState = Mouse.GetState();
            // Check if scroll wheel value has updated
            
            if (mouseState.ScrollWheelValue != lastMouseState.ScrollWheelValue)
            {
                // Check if mouse is inside scroll area
                if (GlobalThings.EnterArea(new Rectangle(_graphics.Viewport.Width - 590, 525, 530, _graphics.Viewport.Height - 525), mouseState))
                {
                    // Check if new scroll wheel value is negative or positive
                    if (mouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
                    {
                        _scrollOffset.Y -= 20;
                    }
                    else if (mouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue && _scrollOffset.Y < 0)
                    {
                        _scrollOffset.Y += 20;
                    }

                }
            }

            lastMouseState = mouseState;
        }
        public void LoadContet(ContentManager Content)
        {
            clipboard = Content.Load<Texture2D>("clipboard");
        }

        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            Vector2 offset = Vector2.Zero;
            int missionFormat = MissionHandler.CurrWorldMission;
            missionCounterText[1] = missionFormat.ToString();
            missionCounterText[3] = MissionHandler.WorldMissionCount[MissionHandler.World].ToString();

            // Draw frame
            _spriteBatch.Draw(whiteRectangle, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 650, 0, 650, _graphics.GraphicsDevice.Viewport.Height), Color.White);

            _spriteBatch.DrawString(GlobalThings.font, MissionHandler.Missions[MissionHandler.Mission], new Vector2(_graphics.GraphicsDevice.Viewport.Width - 600, 410), PlayGround.pgColor);

            // Draw clipboard and paper
            _spriteBatch.Draw(whiteRectangle, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 625, 490, 600, _graphics.GraphicsDevice.Viewport.Height - 490), clipboardColor);

            // Paper shadow
            _spriteBatch.Draw(whiteRectangle, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 600 + 15, 525 + 15, 550, _graphics.GraphicsDevice.Viewport.Height - 525), new Color(123, 52, 11));

            _spriteBatch.Draw(whiteRectangle, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 600, 525, 550, _graphics.GraphicsDevice.Viewport.Height - 525), Color.White);
            _spriteBatch.Draw(clipboard, new Vector2((_graphics.GraphicsDevice.Viewport.Width - 650) + (650 / 2) - (clipboard.Width / 2), 450), Color.White);

            for (int x = 0; x < missionCounterText.Length; x++)
            {
                int waveOffset = 0;
                if(x == 1)
                {
                    waveOffset = (int)(Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 300) * 5);
                }
                else if(x == 3)
                {
                    waveOffset = (int)(Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 300) * 5);
                }
                _spriteBatch.DrawString(GlobalThings.font, missionCounterText[x], new Vector2((_graphics.GraphicsDevice.Viewport.Width) - 280 + offset.X, 443 + waveOffset), colors[x]);
                offset.X += GlobalThings.font.MeasureString(missionCounterText[x]).X;
            }
            _spriteBatch.End();

            // Create scrollable info segment
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, rasterizerState: _rasterizerState, transformMatrix: _matrix);
            _spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(_graphics.GraphicsDevice.Viewport.Width - 600, 555, 550, (_graphics.GraphicsDevice.Viewport.Height - 555) - 60);
            int overallOffset = 0;

            for (int x = 0; x < MissionHandler.MissionsInfoText[MissionHandler.Mission, 0].Length; x++)
            {
                int breakLineOffset = 0;
                int breakLineSize = 20;
                if (x > 0)
                {
                    breakLineOffset = MissionHandler.formattedStrings[x - 1].Count(t => t == '\n');
                    if (MissionHandler.MissionsInfoColor[MissionHandler.Mission, 0][x - 1] != Color.Black)
                    {
                        breakLineSize = 20;
                    }
                }
                overallOffset += breakLineOffset * breakLineSize;


                if (MissionHandler.MissionsInfoColor[MissionHandler.Mission, 0][x] != GameScene.orange)
                {
                    _spriteBatch.DrawString(GlobalThings.smallerFont, "o", new Vector2(_graphics.GraphicsDevice.Viewport.Width - 583, 556 + overallOffset), Color.Red);
                }
                _spriteBatch.DrawString(MissionHandler.MissionsInfoColor[MissionHandler.Mission, 0][x] == Color.Black ? GlobalThings.smallerFont : GlobalThings.smallerFont, MissionHandler.formattedStrings[x], new Vector2(_graphics.GraphicsDevice.Viewport.Width - 550, 560 + overallOffset), MissionHandler.MissionsInfoColor[MissionHandler.Mission, 0][x]);

                if (x == 0)
                {
                    overallOffset += 60;
                    if (MissionHandler.Mission == 9)
                    {
                        _spriteBatch.DrawString(GlobalThings.font, MissionHandler.Coins + "/" + MissionHandler.AmountOfCoins, new Vector2(_graphics.GraphicsDevice.Viewport.Width - 560, 560 + overallOffset), Color.Black);
                        overallOffset += 50;
                    }
                }
                else
                {
                    overallOffset += 50;
                }
            }
            _spriteBatch.End();
            _spriteBatch.Begin();
            
        }

        public void UpdateProportions(GraphicsDevice _graphics)
        {
            area = new Rectangle(_graphics.Viewport.Width - 600, 465, 600, _graphics.Viewport.Height - (140 + 465));
        }
    }
}
