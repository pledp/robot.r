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
        private string typeWriterString;
        private string typeWriterStringType;
        private string currentCharString;
        private int currentLine;
        private int numberOfLines;
        private int currentChar;
        private int cursorOffset;
        private int lineCounterOffset;
        private Dictionary<char, SpriteFont.Glyph> fontGlyphs;

        Texture2D whiteRectangle;

        private static List<String> typing;
        public static List <String> Typing
        {
            get { return typing; }
        }
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
        public codeInput()
        {
            typeWriterString = "pLdev!";
            typeWriterStringType = "";
            currentLine = 0;
            numberOfLines = 1;
            currentChar = 0;
            cursorOffset = 0;

            size = new Vector2();
            pos = new Vector2();
            typing = new List<String>();
            lineCounter = new List<int>();
            lastPressedKeys = new Keys[5];

            typing.Add("");
            lineCounter.Add(numberOfLines);

            codeEditorOffset = new Vector2(50, 10);

        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            staticGraphicsDevice = graphicsDevice;
            font = Content.Load<SpriteFont>("font");
            fontGlyphs = font.GetGlyphs();


            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            playButton = new PlayCodeButton(graphicsDevice, 10, 10);

            // Initialize bag for variables
            variablesBag = new BuildBag(graphicsDevice, 250, "VARIABLES", 0, "variables");
            consoleBag = new BuildBag(graphicsDevice, 250, "CONSOLE", 1, "console");

            darkeyGrey = new Color(65, 65, 63);
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
                spriteBatch.DrawString(font, typing[line - 1], new Vector2(60 + codeEditorOffset.X, line * 50 + codeEditorOffset.Y - 50), Color.White);

                // Draw line counter
                spriteBatch.DrawString(font, line.ToString(), new Vector2(codeEditorOffset.X + pos.X, (line - 1) * 50 + codeEditorOffset.Y + 20), darkeyGrey, 0, origin, 1, SpriteEffects.None, 0);
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
            spriteBatch.Draw(whiteRectangle, new Rectangle(cursorOffset + 40 + Convert.ToInt32(codeEditorOffset.X), currentLine * 50 + Convert.ToInt32(codeEditorOffset.Y), 10, Convert.ToInt32(font.MeasureString("A").Y)), Color.White);

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
            currentChar = 0;
            lineCounter.Insert(currentLine, currentLine+1);
            typing.Insert(currentLine, "");
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
