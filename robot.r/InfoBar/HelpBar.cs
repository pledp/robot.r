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
using System.Threading;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Myra.Graphics2D.UI;
using static System.Net.Mime.MediaTypeNames;

namespace robot.r
{
    public class HelpBar
    {
        private MouseState mouseState;
        private MouseState lastMouseState;
        private Vector2 newhelpBarPos;
        private bool unpressableButton;
        private double elapsedTime = 0;
        private int xOffset;

        private Color darkerGrey;
        private Color customAqua;
        private double desiredDuration = 1f;

        InfoWidget infoWidget;
        InfoWidget[] infoWidgets;

        private static Rectangle helpBar;
        public static Rectangle HelpBarProperty
        {
            get { return helpBar; }
        }

        private Texture2D helpBarTexture;
        private int helpBarWidth;
        private int helpBarIndex;
        private string helpBarText;
        private Color helpBarColor;
        private string helpBarContent;
        Vector2 startingPos;

        private bool helpBarState;
        private bool buttonPressed;

        private RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        public static Vector2 _scrollOffset = Vector2.Zero;
        private Matrix _matrix = Matrix.CreatePerspective(500, 50, 1, 2);

        private readonly string[] codeExamples =
        {
            "x = 5\ny = x",
            "if(5 == 2) {\n   x = 1\n}\nelseif(5 == 3){\n   x = 2\n}\nelse {\n   x = 3\n}",
            "",
            "if(6 == 3 || 6 == 6 && 1 == 1){\n   x = 1\n}",
            "loop(5){\n   print(5)\n}",
            "x = 0\nwhile(x < 5) {\n   x = x + 1\n}",
            "x = 0\nUpdate(){\n   x = x + 1\n}",
            "x = sqrt(5)",
            "x = 1\nprint(x)\nsleep(500)\nprint(x + 1)",
            "robot.x = 3\nrobot.y = robot.x",
            "gem[0].x = 3\ngem[5].y = 6",
            "enemy[0].x = 3\nenemy[5].y = 6",
            "colorBlock[0].x = 3\ncolorBlock[5].y = robot.x\nif(colorBlock[1].color == color.Red) {\n   print(\"red\")\n}",
        };

        public HelpBar(GraphicsDevice _graphics, int width, string text, int index, string content)
        {
            helpBarContent = content;
            helpBarWidth = width;
            helpBarIndex = index;
            helpBarText = text;

            helpBar = new Rectangle(20, _graphics.Viewport.Height - 50, 500, Convert.ToInt32(_graphics.Viewport.Height * 0.75));

            helpBarTexture = new Texture2D(_graphics, 1, 1);
            helpBarColor = new Color(240, 247, 244);
            helpBarTexture.SetData(new[] { helpBarColor });
            helpBarState = false;
            unpressableButton = false;

            darkerGrey = new Color(65, 65, 63);
            customAqua = new Color(112, 171, 175);

            startingPos = new Vector2(helpBar.X, helpBar.Y);

            infoWidgets = new InfoWidget[LanguageHandler.HelpBarWidgets.Length];
            for(int x = 0; x < infoWidgets.Length; x++)
            {
                infoWidgets[x] = new InfoWidget(LanguageHandler.HelpBarWidgets[x][LanguageHandler.language][0], LanguageHandler.HelpBarWidgets[x][LanguageHandler.language][1], codeExamples[x]);
            }
        }

        public void Update(GraphicsDevice _graphics, GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(helpBar, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed && !unpressableButton)
            {
                buttonPressed = true;
            }


            if (buttonPressed)
            {
                if (!helpBarState)
                {
                    newhelpBarPos = new Vector2(helpBarWidth, Convert.ToInt32(_graphics.Viewport.Height * 0.25));
                    AnimatehelpBar(newhelpBarPos, gameTime);
                }
                else if (helpBarState)
                {
                    newhelpBarPos = new Vector2(helpBarWidth, Convert.ToInt32(_graphics.Viewport.Height - 50));
                    AnimatehelpBar(newhelpBarPos, gameTime);
                }
            }
            _matrix = Matrix.CreateTranslation(new Vector3(_scrollOffset, 0));

            // Scrolling for variables helpBar.
            if (GlobalThings.EnterArea(helpBar, mouseState))
            {
                // Check if scroll wheel value has updated
                if (mouseState.ScrollWheelValue != lastMouseState.ScrollWheelValue)
                {
                    // Check if new scroll wheel value is negative or positive
                    if (mouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
                    {
                        _scrollOffset.Y -= 20;
                    }
                    else if (mouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
                    {
                        _scrollOffset.Y += 20;
                    }
                }
            }
            lastMouseState = Mouse.GetState();

        }
        private void AnimatehelpBar(Vector2 newPos, GameTime gameTime)
        {
            unpressableButton = true;
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            float percentageComplete = (float)elapsedTime / (float)desiredDuration;
            helpBar.Y = (int)Vector2.Lerp(startingPos, newPos, MathHelper.SmoothStep(0, 1, percentageComplete)).Y;

            if (helpBar.Y == newPos.Y)
            {
                startingPos = new Vector2(helpBar.X, helpBar.Y);
                unpressableButton = false;
                elapsedTime = 0;
                buttonPressed = false;

                helpBarState = !helpBarState;
            }
        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            int offsetY = 0;
            for (int x = 0; x < infoWidgets.Length; x++)
            {
                infoWidgets[x].widgetTitle = LanguageHandler.HelpBarWidgets[x][LanguageHandler.language][0];
                infoWidgets[x].widgetText = LanguageHandler.HelpBarWidgets[x][LanguageHandler.language][1];

                infoWidgets[x].Update(gameTime, new Vector2(helpBar.X, helpBar.Y + 10 + offsetY + (x * 50)));
                offsetY += (int)GlobalThings.font.MeasureString(GlobalThings.FormatLineBreak(infoWidgets[x].widgetTitle, 280)).Y;
            }
            spriteBatch.Draw(helpBarTexture, new Rectangle(helpBar.X + 20, helpBar.Y + 20, helpBar.Width, helpBar.Height), Color.Black * 0.5f);

            spriteBatch.Draw(helpBarTexture, helpBar, helpBarColor);

            Vector2 variableTextPos = new Vector2(helpBar.X, helpBar.Y);
            variableTextPos.X = helpBar.X + (helpBar.Width - GlobalThings.font.MeasureString(helpBarText).X) / 1.9f;

            spriteBatch.DrawString(GlobalThings.font, helpBarText, new Vector2(variableTextPos.X, helpBar.Y), Color.Red);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, rasterizerState: _rasterizerState, transformMatrix: _matrix);
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(0, helpBar.Y + 55, graphics.GraphicsDevice.Viewport.Width, helpBar.Height);


            for(int x = 0; x < infoWidgets.Length; x++)
            {
                infoWidgets[x].Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin();
            for (int x = 0; x < infoWidgets.Length; x++)
            {
                if (infoWidgets[x].DrawFrame)
                {
                    infoWidgets[x].DrawHoverFrame(spriteBatch);
                }
            }
        }

        public void UpdateProportions(GraphicsDevice _graphics)
        {
            helpBar.Height = Convert.ToInt32(_graphics.Viewport.Height * 0.75);
            helpBar.X = 20;

            if (helpBarState)
            {
                helpBar.Y = Convert.ToInt32(_graphics.Viewport.Height * 0.25);
            }
            else if (!helpBarState)
            {
                helpBar.Y = Convert.ToInt32(_graphics.Viewport.Height - 50);
            }
            if (!unpressableButton)
            {
                startingPos = new Vector2(helpBar.X, helpBar.Y);
            }
        }
    }
}
