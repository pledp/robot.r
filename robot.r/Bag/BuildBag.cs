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

namespace robot.r
{
    public class BuildBag
    {
        private MouseState mouseState;
        private MouseState lastMouseState;
        private Vector2 newBagPos;
        private bool unpressableButton;
        private double elapsedTime = 0;
        private int xOffset;

        private Color darkerGrey;
        private Color customAqua;
        private double desiredDuration = 1f;

        private Rectangle bag;
        private Texture2D bagTexture;
        private int bagWidth;
        private int bagIndex;
        private string bagText;
        private Color bagColor;
        private string bagContent;
        Vector2 startingPos;

        private bool bagState;
        private bool buttonPressed;

        private ScrollBar scrollBar;
        private RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        private Vector2 _scrollOffset = Vector2.Zero;
        private Matrix _matrix = Matrix.CreatePerspective(500, 50, 1, 2);

        public BuildBag(GraphicsDevice _graphics, int width, string text, int index, string content)
        {
            bagContent = content;
            bagWidth = width;
            bagIndex = index;
            bagText = text;
            if(bagIndex > 0)
            {
                xOffset = 50;
            }
            bag = new Rectangle(_graphics.Viewport.Width - ((bagIndex +1) * bagWidth) - 50 - xOffset, _graphics.Viewport.Height - 50, bagWidth, Convert.ToInt32(_graphics.Viewport.Height * 0.75));

            bagTexture = new Texture2D(_graphics, 1, 1);
            bagColor = new Color(240, 247, 244);
            bagTexture.SetData(new[] { bagColor });
            bagState = false;
            unpressableButton = false;

            darkerGrey = new Color(65, 65, 63);
            customAqua = new Color(112, 171, 175);

            scrollBar = new ScrollBar(new Rectangle(bag.X + bag.Width, bag.Y+50, 20, bag.Height-50), bagTexture, bagTexture, 10, 0);
            startingPos = new Vector2(bag.X, bag.Y);
        }
        private bool enterButton()
        {
            if (mouseState.X < bag.X + bag.Width &&
                mouseState.X > bag.X &&
                mouseState.Y < bag.Y + bag.Height &&
                mouseState.Y > bag.Y)
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
                buttonPressed = true;
            }

            if(buttonPressed)
            {
                if (!bagState)
                {
                    newBagPos = new Vector2(bagWidth, Convert.ToInt32(_graphics.Viewport.Height * 0.25));
                    AnimateBag(newBagPos, gameTime);
                }
                else if (bagState)
                {
                    newBagPos = new Vector2(bagWidth, Convert.ToInt32(_graphics.Viewport.Height - 50));
                    AnimateBag(newBagPos, gameTime);
                }
            }
            _matrix = Matrix.CreateTranslation(new Vector3(_scrollOffset, 0));

            // Scrolling for variables bag.
            if (enterButton())
            {
                // Check if scroll wheel value has updated
                if (mouseState.ScrollWheelValue != lastMouseState.ScrollWheelValue)
                {
                    // Check if new scroll wheel value is negative or positive
                    if(mouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
                    {
                        _scrollOffset.Y -= 10;
                    }
                    else if (mouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
                    {
                        _scrollOffset.Y += 10;
                    }

                }

            }
            lastMouseState = Mouse.GetState();
            scrollBar.Update();

        }
        private void AnimateBag(Vector2 newPos, GameTime gameTime)
        {
            unpressableButton = true;
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            float percentageComplete = (float)elapsedTime / (float)desiredDuration;
            bag.Y = (int)Vector2.Lerp(startingPos, newPos, MathHelper.SmoothStep(0, 1, percentageComplete)).Y;
            scrollBar.UpdateProportions(bag);

            if (bag.Y == newPos.Y)
            {
                startingPos = new Vector2(bag.X, bag.Y);
                unpressableButton = false;
                elapsedTime = 0;
                buttonPressed = false;

                bagState = !bagState;
            }
        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics, SpriteFont font)
        {
            spriteBatch.Draw(bagTexture, bag, bagColor);
            int variableIndex = 0;

            Vector2 variableTextPos = new Vector2(bag.X, bag.Y);
            variableTextPos.X = bag.X + (bag.Width - font.MeasureString(bagText).X) / 1.9f;

            spriteBatch.DrawString(font, bagText, new Vector2(variableTextPos.X, bag.Y), Color.Red);
            spriteBatch.DrawString(font, "---------------", new Vector2(bag.X, bag.Y+30), Color.Red);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, rasterizerState: _rasterizerState, transformMatrix: _matrix);
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(bag.X, bag.Y+55, bag.Width, bag.Height);
            if (Interpreter.variables != null && bagContent == "variables")
            {
                Dictionary<string, double> variables = new Dictionary<string, double>(Interpreter.variables);
                foreach (KeyValuePair<string, double> variable in variables)
                {
                    string printableValue = variable.Value.ToString();
                    Rectangle rectangle = bag;
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

                    spriteBatch.DrawString(font, "o", new Vector2(bag.X+10, bag.Y + variableIndex * 100 + 60), Color.Red);
                    spriteBatch.DrawString(font, printThisKey, new Vector2(keyPos.X, bag.Y + variableIndex * 100 + 60), darkerGrey);
                    spriteBatch.DrawString(font, printThisValue, new Vector2(valuePos.X, bag.Y + variableIndex * 100 + 100), customAqua);
                    variableIndex++;
                }
            } 
            else if(Interpreter.consoleText != null && bagContent == "console")
            {
                List<string> console = Interpreter.consoleText;
                foreach (string text in console)
                {
                    Rectangle rectangle = bag;
                    Vector2 textPos = new Vector2(rectangle.X, rectangle.Y);
                    Vector2 textSize = font.MeasureString(text);
                    textPos.X = textPos.X + (rectangle.Width - textSize.X) / 1.9f;
                    spriteBatch.DrawString(font, text, new Vector2(textPos.X, bag.Y + variableIndex * 50 + 60), darkerGrey);

                    variableIndex++;
                }
            }
            spriteBatch.End();
            spriteBatch.Begin();
            scrollBar.Draw(spriteBatch);
        }

        public void UpdateProportions(GraphicsDevice _graphics)
        {
            bag.Height = Convert.ToInt32(_graphics.Viewport.Height * 0.75);
            bag.X = _graphics.Viewport.Width - ((bagIndex + 1) * bagWidth) - 50 - xOffset;

            if (bagState)
            {
                bag.Y = Convert.ToInt32(_graphics.Viewport.Height * 0.25);
            }
            else if (!bagState)
            {
                bag.Y = Convert.ToInt32(_graphics.Viewport.Height - 50);
            }
            scrollBar.UpdateProportions(bag);
            if(!unpressableButton)
            {
                startingPos = new Vector2(bag.X, bag.Y);
            }
        }
    }
}
