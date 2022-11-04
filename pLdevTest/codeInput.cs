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
        private SpriteFont font;
        private string typeWriterString;
        private string typeWriterStringType;
        private string currentCharString;
        private int currentLine;
        private int numberOfLines;
        private int currentChar;
        private int cursorOffset;
        private Dictionary<char, SpriteFont.Glyph> fontGlyphs;

        Texture2D whiteRectangle;
        Texture2D yellowRectangle;

        private List<String> typing;
        private List<int> lineCounter;
        private double typeWriterTimer;
        private double arrowTimer;
        private Keys[] lastPressedKeys;

        private Vector2 codeEditorOffset;

        public codeInput()
        {
            typeWriterString = "pLdev!";
            typeWriterStringType = "";
            currentLine = 0;
            numberOfLines = 1;
            currentChar = 0;
            cursorOffset = 0;

            typing = new List<String>();
            lineCounter = new List<int>();
            lastPressedKeys = new Keys[5];

            typing.Add("");
            lineCounter.Add(numberOfLines);

            codeEditorOffset = new Vector2(10, 10);
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            font = Content.Load<SpriteFont>("font");
            fontGlyphs = font.GetGlyphs();

            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            yellowRectangle = new Texture2D(graphicsDevice, 1, 1);
            yellowRectangle.SetData(new[] { Color.Green });

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();
            arrowTimer += gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key) || arrowTimer > 0.2)
                {
                    switch(key)
                    {
                        case Keys.Up:
                            swapCodeLine(Keys.Up);
                            break;
                        case Keys.Down:
                            swapCodeLine(Keys.Down);
                            break;
                        case Keys.Right:
                            moveOnCodeLine(Keys.Right);
                            break;
                        case Keys.Left:
                            moveOnCodeLine(Keys.Left);
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
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(yellowRectangle, new Rectangle(5 + Convert.ToInt32(codeEditorOffset.X), currentLine * 50 + Convert.ToInt32(codeEditorOffset.Y), Convert.ToInt32(font.MeasureString(lineCounter[currentLine].ToString()).X) +5, Convert.ToInt32(font.MeasureString("A").Y)), Color.White);

            spriteBatch.DrawString(font, typeWriterStringType, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - font.MeasureString(typeWriterString).X / 2, graphics.GraphicsDevice.Viewport.Height / 2 - font.MeasureString(typeWriterString).Y / 2), Color.Black);


            foreach (int line in lineCounter)
            {
                // Draw code
                spriteBatch.DrawString(font, typing[line - 1], new Vector2(60 + codeEditorOffset.X, line * 50 + codeEditorOffset.Y - 50), Color.Black);

                // Draw line counter
                spriteBatch.DrawString(font, line.ToString(), new Vector2(10 + codeEditorOffset.X, (line - 1) * 50 + codeEditorOffset.Y), Color.White);
            }
            // Typing cursor indicator
            switch (typing[currentLine])
            {
                case "":
                    cursorOffset = 0;
                    break;

                default:
                    cursorOffset = Convert.ToInt32(font.MeasureString(currentCharString).X) + Convert.ToInt32(font.MeasureString(typing[currentLine].Remove(currentChar)).X);
                    break;
            }
            spriteBatch.Draw(whiteRectangle, new Rectangle(cursorOffset + 40 + Convert.ToInt32(codeEditorOffset.X), currentLine * 50 + Convert.ToInt32(codeEditorOffset.Y), 10, Convert.ToInt32(font.MeasureString("A").Y)), Color.White);

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
                    currentChar++;
                    currentCharString = typing[currentLine][currentChar-1].ToString();
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
        private void swapCodeLine(Keys key)
        {
            if (key == Keys.Up && currentLine > 0)
            {
                currentLine--;
                currentChar = typing[currentLine].Length;
                Debug.WriteLine(currentChar);
            }
            else if (key == Keys.Down && currentLine < numberOfLines -1)
            {
                currentLine++;
                currentChar = typing[currentLine].Length;
                Debug.WriteLine(currentChar);

            }
        }
        private void moveOnCodeLine(Keys key)
        {
            if(key == Keys.Left && currentChar > 0 )
            {
                currentChar--;
            } else if (key == Keys.Right && currentChar < typing[currentLine].Length)
            {
                currentChar++;
            }
        }
        private void HandleBackspace()
        {
            if (currentLine != 0 && typing[currentLine].Length == 0)
            {
                // If line is empty; delete line, 

                // If current line is the last line, go back 1 line
                
                lineCounter.RemoveAt(currentLine);
                typing.RemoveAt(currentLine);
                numberOfLines--;
                currentLine--;
                currentChar = typing[currentLine].Length;

                // Change line counter for all lines after the deleted line by -1
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
    } 
}
