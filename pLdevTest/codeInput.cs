using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;


namespace pLdevTest
{
    public class codeInput
    {
        public static SpriteFont font;
        public static int readingLine;

        private string typeWriterString;
        private string typeWriterStringType;
        private string currentCharString;
        private int currentLine;
        private int numberOfLines;
        private int currentChar;
        private int cursorOffset;
        private int lineCounterOffset;
        private Dictionary<char, SpriteFont.Glyph> fontGlyphs;
        string indentString = "   ";

        Texture2D whiteRectangle;

        private static List<String> typing;
        public static List <String> Typing
        {
            get { return typing; }
        }
        private List<String> formattedCode;
        private List<int> lineIndent;

        private List<int> lineCounter;
        private double typeWriterTimer;
        private double arrowTimer;
        private Keys[] lastPressedKeys;

        private PlayCodeButton playButton;

        private Vector2 codeEditorOffset;
        private Vector2 size;
        private Vector2 pos;
        private Color darkeyGrey;

        private static GraphicsDevice staticGraphicsDevice;
        private BuildBag variablesBag;
        private BuildBag consoleBag;

        private RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        private Vector2 _scrollOffset = Vector2.Zero;
        private Matrix _matrix = Matrix.CreatePerspective(500, 50, 1, 2);
        private MouseState mouseState;
        private MouseState lastMouseState;

        private int spaceSize;

        public codeInput(SpriteFont pixelFont)
        {
            typeWriterString = "pLdev!";
            typeWriterStringType = "";
            currentLine = 0;
            numberOfLines = 1;
            currentChar = 0;
            cursorOffset = 0;

            font = pixelFont;
            fontGlyphs = font.GetGlyphs();

            size = new Vector2();
            pos = new Vector2();
            typing = new List<String>();
            lineCounter = new List<int>();
            lastPressedKeys = new Keys[5];

            typing.Add("");
            lineCounter.Add(numberOfLines);

            codeEditorOffset = new Vector2(70, 10);

        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            staticGraphicsDevice = graphicsDevice;

            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            playButton = new PlayCodeButton(graphicsDevice, 10, 10);

            // Initialize bag for variables
            variablesBag = new BuildBag(graphicsDevice, 250, "VARIABLES", 0, "variables");
            consoleBag = new BuildBag(graphicsDevice, 250, "CONSOLE", 1, "console");

            darkeyGrey = new Color(65, 65, 63);

            spaceSize = (int)Game1.font.MeasureString(indentString).X;
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();
            arrowTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Check for arrow keys pressed, when pressed call MoveOnCodeLine() or SwapCodeLine()
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key) || arrowTimer > 0.15)
                {
                    switch(key)
                    {
                        
                        case Keys.Up:
                            SwapCodeLine(Keys.Up);
                            break;
                        case Keys.Down:
                            SwapCodeLine(Keys.Down);
                            break;
                        case Keys.Right:
                            MoveOnCodeLine(Keys.Right);
                            break;
                        case Keys.Left:
                            MoveOnCodeLine(Keys.Left);
                            break;
                    }
                    arrowTimer = 0;
                }
            }

            formattedCode = FormatIndenting();

            _matrix = Matrix.CreateTranslation(new Vector3(_scrollOffset, 0));
            mouseState = Mouse.GetState();
            // Check if scroll wheel value has updated
            if (mouseState.ScrollWheelValue != lastMouseState.ScrollWheelValue)
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
            lastMouseState = mouseState;

            lastPressedKeys = kbState.GetPressedKeys();

            if (typeWriterStringType.Length < typeWriterString.Length)
            {
                typeWriterTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (typeWriterTimer > 0.5)
                {
                    typeWriterTimer = 0;
                    typeWriterStringType += typeWriterString[typeWriterStringType.Length];
                }
            }
            playButton.Update(graphics, gameTime, this);
            variablesBag.Update(graphics.GraphicsDevice, gameTime);
            consoleBag.Update(graphics.GraphicsDevice, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            //spriteBatch.DrawString(font, typeWriterStringType, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - font.MeasureString(typeWriterString).X / 2, graphics.GraphicsDevice.Viewport.Height / 2 - font.MeasureString(typeWriterString).Y / 2), Color.Black);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, rasterizerState: _rasterizerState, transformMatrix: _matrix);
            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            pos = new Vector2(10 / 2, 0 /2);
            Vector2 origin = size * 0.5f;
            

            foreach (int line in lineCounter)
            {
                // Align text right
                Rectangle rectangle = new Rectangle(10, 10, 55, 40);
                pos = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
                size = font.MeasureString(lineCounter[line-1].ToString());
                
                origin = size * 0.5f;
                origin.X -= rectangle.X/ 2 - size.X / 2;

                lineCounterOffset = Convert.ToInt32(font.MeasureString(lineCounter[line-1].ToString()).X);
                // Draw code
                spriteBatch.DrawString(font, formattedCode[line - 1], new Vector2(60 + codeEditorOffset.X, line * 50 + codeEditorOffset.Y - 50), Color.White);

                // Draw line counter
                spriteBatch.DrawString(font, line.ToString(), new Vector2(codeEditorOffset.X + pos.X, (line - 1) * 50 + codeEditorOffset.Y + 20), line == readingLine ? Color.Green : darkeyGrey, 0, origin, 1, SpriteEffects.None, 0);
            }

            // Typing cursor indicator
            switch (typing[currentLine])
            {
                case "":
                    cursorOffset = 20;
                    break;

                default:
                    cursorOffset = Convert.ToInt32(font.MeasureString(currentCharString).X) + Convert.ToInt32(font.MeasureString(typing[currentLine].Remove(currentChar)).X);
                    break;
            }
            spriteBatch.Draw(whiteRectangle, new Rectangle((cursorOffset + 40 + Convert.ToInt32(codeEditorOffset.X)) + (lineIndent[currentLine] * spaceSize), currentLine * 50 + Convert.ToInt32(codeEditorOffset.Y), 10, Convert.ToInt32(font.MeasureString("A").Y)), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();

            variablesBag.Draw(spriteBatch, gameTime, graphics, font);
            consoleBag.Draw(spriteBatch, gameTime, graphics, font);
            playButton.Draw(spriteBatch, gameTime, graphics);
        }

        public void typeText(char key)
        {
            if (key == '\r')
            {
                // When enter key is pressed: Create new line for code editor.
                addCodeLine();
            }
            if (key == '')
            {
                // Handles all backspace interactions
                HandleBackspace();
            } else
            {
                // Type text to array
                if (fontGlyphs.ContainsKey(key))
                {
                    typing[currentLine] = typing[currentLine].Insert(currentChar, key.ToString());
                    currentCharString = typing[currentLine][currentChar].ToString();
                    currentChar++;
                }
            }
        }

        private void addCodeLine()
        {
            for (int i = 0; i < lineCounter.Count; i++)
            {
                if (lineCounter[i] > currentLine + 1)
                {
                    lineCounter[i] = lineCounter[i] + 1;
                }
            }

            numberOfLines++;
            currentLine++;
            lineCounter.Insert(currentLine, currentLine+1);
            typing.Insert(currentLine, "");

            currentChar = typing[currentLine].Length;
        }
        private void SwapCodeLine(Keys key)
        {
            if (key == Keys.Up && currentLine > 0)
            {
                currentLine--;
                currentChar = typing[currentLine].Length;
                
            }
            else if (key == Keys.Down && currentLine < numberOfLines -1)
            {
                currentLine++;
                currentChar = typing[currentLine].Length;
                

            }
        }
        private void MoveOnCodeLine(Keys key)
        {
            if(key == Keys.Left && currentChar > 0 )
            {
                if(currentChar < 2)
                {
                    currentCharString = typing[currentLine][currentChar - 1].ToString();
                } else
                {
                    currentCharString = typing[currentLine][currentChar - 2].ToString();
                }
                currentChar--;
            } else if (key == Keys.Right && currentChar < typing[currentLine].Length)
            {
                currentCharString = typing[currentLine][currentChar].ToString();
                Debug.WriteLine(typing[currentLine][currentChar].ToString());
                currentChar++;
            }
        }
        private void HandleBackspace()
        {
            if (currentLine != 0 && typing[currentLine].Length == 0)
            {
                // If line is empty; delete line

                // If current line is the last character, go back 1 line
                
                lineCounter.RemoveAt(currentLine);
                typing.RemoveAt(currentLine);
                numberOfLines--;
                currentLine--;
                currentChar = typing[currentLine].Length;

                // Change line counter for all lines after the deleted line by -1.
                for (int i = 0; i < lineCounter.Count; i++)
                {
                    if (lineCounter[i] > currentLine + 1)
                    {
                        lineCounter[i] = lineCounter[i] - 1;
                    }
                }
            }
            else if (typing[currentLine].Length > 0 && currentChar > 0)
            {
                currentChar--;
                typing[currentLine] = typing[currentLine].Remove(currentChar, 1);
            }
        }
        private List<String> FormatIndenting()
        {
            List<String> formattedCode = new List<String>();
            List<int> lineIndentCounter = new List<int>();

            int indentLevel = 0;
            for (int i = 0; i < typing.Count; i++)
            {
                string line = typing[i];
                if (line.Contains("}"))
                {
                    indentLevel--;
                }

                //int originalLineLength = line.Length;
                //line = line.TrimStart (' ');

                for (int j = 0; j < indentLevel; j++)
                {
                    line = indentString + line;
                }

                formattedCode.Add(line);
                lineIndentCounter.Add(indentLevel);

                if (line.Contains("{"))
                {
                    indentLevel++;
                }
            }

            lineIndent = lineIndentCounter;
            return formattedCode;
        }

        public void UpdateEditorProportions(GraphicsDeviceManager graphicsDevice)
        {
            if(variablesBag != null)
            {
                variablesBag.UpdateProportions(graphicsDevice.GraphicsDevice);
                consoleBag.UpdateProportions(graphicsDevice.GraphicsDevice);
            }
        }
    } 
}
