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

namespace pLdevTest
{
    public class VariablesBag
    {
        private Rectangle variableBag;
        private Texture2D bagTexture;
        private MouseState mouseState;
        private MouseState lastMouseState;
        private Color bagColor;
        private bool bagState;
        private int newBagY;
        private bool unpressableButton;

        private Color darkerGrey;
        private Color customAqua;

        private int varBagWidth;
        private ScrollBar scrollBar;
        private RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        private Vector2 _scrollOffset = Vector2.Zero;
        private Matrix _matrix = Matrix.CreatePerspective(500, 50, 1, 2);

        public VariablesBag(GraphicsDevice _graphics)
        {
            varBagWidth = 250;
            variableBag = new Rectangle(_graphics.Viewport.Width - varBagWidth - 50, _graphics.Viewport.Height - 50, varBagWidth, Convert.ToInt32(_graphics.Viewport.Height * 0.75));

            bagTexture = new Texture2D(_graphics, 1, 1);
            bagColor = new Color(240, 247, 244);
            bagTexture.SetData(new[] { bagColor });
            bagState = false;
            unpressableButton = false;

            darkerGrey = new Color(65, 65, 63);
            customAqua = new Color(112, 171, 175);

            scrollBar = new ScrollBar(new Rectangle(variableBag.X + variableBag.Width, variableBag.Y+50, 20, variableBag.Height-50), bagTexture, bagTexture, 10, 0);
        }
        public bool enterButton()
        {
            if (mouseState.X < variableBag.X + variableBag.Width &&
                mouseState.X > variableBag.X &&
                mouseState.Y < variableBag.Y + variableBag.Height &&
                mouseState.Y > variableBag.Y)
            {
                return true;
            }
            return false;
        }

        public void Update(GraphicsDevice _graphics, GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (enterButton() && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed && !unpressableButton)
            {
                if (!bagState)
                {
                    newBagY = Convert.ToInt32(_graphics.Viewport.Height * 0.25);
                    bagState = true;
                    AnimateBagY(newBagY);
                }
                else if (bagState)
                {
                    newBagY = Convert.ToInt32(_graphics.Viewport.Height - 50);
                    bagState = false;
                    AnimateBagY(newBagY);
                }
            }
            _matrix = Matrix.CreateTranslation(new Vector3(_scrollOffset, 0));
            if (enterButton())
            {
                if (mouseState.ScrollWheelValue > 0)
                {
                        _scrollOffset.Y -= 1;
                }
                else if (mouseState.ScrollWheelValue < 0)
                {
                        _scrollOffset.Y += 1;
                }
            }
            lastMouseState = Mouse.GetState();
            scrollBar.Update();
        }
        private async void AnimateBagY(int newY)
        {
            unpressableButton = true;
            if(newY < variableBag.Y)
            {
                while (newY < variableBag.Y)
                {
                    await Task.Delay(1);
                    variableBag.Y -= 10;
                    scrollBar.UpdateProportions(variableBag);
                }
            } else
            {
                while (variableBag.Y < newY)
                {
                    await Task.Delay(1);
                    variableBag.Y += 10;
                    scrollBar.UpdateProportions(variableBag);
                }
            }
            unpressableButton = false;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics, SpriteFont font)
        {
            spriteBatch.Draw(bagTexture, variableBag, bagColor);
            int variableIndex = 0;

            Vector2 variableTextPos = new Vector2(variableBag.X, variableBag.Y);
            variableTextPos.X = variableBag.X + (variableBag.Width - font.MeasureString("VARIABLES").X) / 1.9f;

            spriteBatch.DrawString(font, "VARIABLES", new Vector2(variableTextPos.X, variableBag.Y), Color.Red);
            spriteBatch.DrawString(font, "---------------", new Vector2(variableBag.X, variableBag.Y+30), Color.Red);
            spriteBatch.End();
            RasterizerState oldState = spriteBatch.GraphicsDevice.RasterizerState;
            Rectangle currentScissorRect = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, rasterizerState: _rasterizerState, transformMatrix: _matrix);
            spriteBatch.GraphicsDevice.ScissorRectangle = variableBag;
            if (Interpreter.variables != null)
            {
                Dictionary<string, double> variables = new Dictionary<string, double>(Interpreter.variables);
                foreach (KeyValuePair<string, double> variable in variables)
                {
                    string printableValue = variable.Value.ToString();
                    Rectangle rectangle = variableBag;
                    Vector2 keyPos = new Vector2(rectangle.X, rectangle.Y);
                    Vector2 valuePos = new Vector2(rectangle.X, rectangle.Y);

                    string valueTooLong = "";
                    string valueStringFormatter = "";

                    if (printableValue.Contains(','))
                    {
                        valueStringFormatter = "#.00000";
                    }
                    if (printableValue.Length > 5)
                    {
                        valueTooLong = "...";
                        printableValue = printableValue.Substring(0, 6);
                    }
                    string printThisKey = variable.Key + ":";
                    string printThisValue = String.Format(printableValue, valueStringFormatter) + valueTooLong;
                    Vector2 keySize = font.MeasureString(printThisKey);
                    Vector2 valueSize = font.MeasureString(printThisValue);

                    keyPos.X = keyPos.X + (rectangle.Width - keySize.X) / 1.9f;
                    valuePos.X = valuePos.X + (rectangle.Width - valueSize.X) / 1.9f;

                    spriteBatch.DrawString(font, "o", new Vector2(variableBag.X+10, variableBag.Y + variableIndex * 100 + 60), Color.Red);
                    spriteBatch.DrawString(font, printThisKey, new Vector2(keyPos.X, variableBag.Y + variableIndex * 100 + 60), darkerGrey);
                    spriteBatch.DrawString(font, printThisValue, new Vector2(valuePos.X, variableBag.Y + variableIndex * 100 + 100), customAqua);
                    variableIndex++;
                }
            }
            spriteBatch.End();
            spriteBatch.Begin();
            scrollBar.Draw(spriteBatch);
        }

        public void UpdateProportions(GraphicsDevice _graphics)
        {
            variableBag.Height = Convert.ToInt32(_graphics.Viewport.Height * 0.75);
            variableBag.X = _graphics.Viewport.Width - varBagWidth - 50;

            if (bagState)
            {
                variableBag.Y = Convert.ToInt32(_graphics.Viewport.Height * 0.25);
            }
            else if (!bagState)
            {
                variableBag.Y = Convert.ToInt32(_graphics.Viewport.Height - 50);
            }
            scrollBar.UpdateProportions(variableBag);
        }
    }
}
