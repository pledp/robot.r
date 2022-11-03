using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace pLdevTest
{
    public class codeInput
    {
        private SpriteFont font;
        private string typeWriterString;
        private string typeWriterStringType;
        private int currentLine;
        private int numberOfLines;
        private Dictionary<char, SpriteFont.Glyph> fontGlyphs;
        Texture2D whiteRectangle;
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
                    if (kbState.IsKeyDown(Keys.Up))
                    {
                        swapCodeLine(Keys.Up);
                    }
                    else if (kbState.IsKeyDown(Keys.Down))
                    {
                        swapCodeLine(Keys.Down);
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
            spriteBatch.DrawString(font, typeWriterStringType, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - font.MeasureString(typeWriterString).X / 2, graphics.GraphicsDevice.Viewport.Height / 2 - font.MeasureString(typeWriterString).Y / 2), Color.Black);


            foreach (int line in lineCounter)
            {
                // Draw code
                
                spriteBatch.DrawString(font, typing[line - 1], new Vector2(50 + codeEditorOffset.X, line * 50 + codeEditorOffset.Y - 50), Color.Black);

                // Draw line counter
                spriteBatch.DrawString(font, line.ToString(), new Vector2(10 + codeEditorOffset.X, (line - 1) * 50 + codeEditorOffset.Y), Color.White);
            }
            // Typing cursor indicator
            spriteBatch.Draw(whiteRectangle, new Rectangle(Convert.ToInt32(font.MeasureString(typing[currentLine]).X) + 60 + Convert.ToInt32(codeEditorOffset.X), currentLine * 50 + Convert.ToInt32(codeEditorOffset.Y), 20, Convert.ToInt32(font.MeasureString("A").Y)), Color.White);
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
                    typing[currentLine] += key.ToString();
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
        }
        private void swapCodeLine(Keys key)
        {
            if (key == Keys.Up && currentLine > 0)
            {
                currentLine--;
            }
            else if (key == Keys.Down && currentLine < numberOfLines -1)
            {
                currentLine++;
            }
        }
        private void HandleBackspace()
        {
            if (typing[currentLine].Length > 0)
            {
                typing[currentLine] = typing[currentLine].Remove(typing[currentLine].Length - 1);
            }
            else if (currentLine != 0 && typing[currentLine].Length == 0)
            {
                // If line is empty; delete line, 

                // If current line is the last line, go back 1 line
                
                lineCounter.RemoveAt(currentLine);
                typing.RemoveAt(currentLine);
                numberOfLines--;
                currentLine--;

                // Change line counter for all lines after the deleted line by -1
                for (int i = 0; i < lineCounter.Count; i++)
                {
                    if (lineCounter[i] > currentLine + 1)
                    {
                        lineCounter[i] = lineCounter[i] - 1;
                    }
                }
            }
        }
    } 
}
