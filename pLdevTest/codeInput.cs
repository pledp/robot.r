using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using Myra;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Myra.Graphics2D.Brushes;
using FontStashSharp.RichText;
using System.Globalization;
using Myra.Graphics2D.UI.Styles;
using Myra.Graphics2D;
using Myra.Assets;

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
        private Vector2 origin;
        private Color darkeyGrey;
        private Desktop _desktop;

        private Myra.Graphics2D.UI.Label memoryText;
        private const int Labels = 2;
        private ScrollViewer textboxScroll;
        private string startingMemoryText;

        private SplitPane _splitPane;
        private HorizontalSplitPane topPanel;

        private static GraphicsDevice staticGraphicsDevice;
        public codeInput()
        {
            startingMemoryText = "/c[red]MEMORY/cd\n" +
                                 "/c[red]----/cd\n";

            typeWriterString = "pLdev!";
            typeWriterStringType = "";
            currentLine = 0;
            numberOfLines = 1;
            currentChar = 0;
            cursorOffset = 0;

            size = new Vector2();
            pos = new Vector2();
            origin = new Vector2();
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
            darkeyGrey = new Color(65, 65, 63);

            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8
            };
            // Make text that displays every variable in MEMORY TODO: MOVE THIS TO OTHER CLASS.
            Label phLabel = new Label();

            memoryText = new Label();
            memoryText.Wrap = true;
            memoryText.Text = startingMemoryText;
            memoryText.TextAlign = TextHorizontalAlignment.Center;
            memoryText.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Stretch;
            memoryText.Id = "memoryText";
            byte[] ttfData = File.ReadAllBytes("Retro Gaming.ttf");
            FontSystem fontSystem = new FontSystem();
            fontSystem.AddFont(ttfData);
            memoryText.Font = fontSystem.GetFont(32);

            // Make scrollbar for text and attach text to scrollbar.
            textboxScroll = new ScrollViewer();
            textboxScroll.Content = memoryText;

            topPanel = new HorizontalSplitPane();

            topPanel.Widgets.Add(phLabel);
            topPanel.Widgets.Add(textboxScroll);
            topPanel.Widgets[1].Width = Convert.ToInt32(graphicsDevice.Viewport.Width * 0.25);
            memoryText.Width = topPanel.Widgets[1].Width - 5;

            topPanel.ProportionsChanged += SplitPaneOnProportionsChanged;
            topPanel.SetSplitterPosition(0, 0.75f);
            topPanel.Widgets[1].Background = new SolidBrush("#99E1D9");

            // Add it to the desktop
            _desktop = new Desktop
            {
                FocusedKeyboardWidget = memoryText,
            };
            _desktop.Root = topPanel;

        }
        private void SplitPaneOnProportionsChanged(object sender, EventArgs eventArgs)
        {
            UpdateMemoryBar();
        }
        private void UpdateMemoryBar()
        {
            // Measures size of sidepanel, centers MEMORY text to middle of sidepanel.
            topPanel.Widgets[1].Width = Convert.ToInt32(topPanel.GetProportion(1) * staticGraphicsDevice.Viewport.Width * 0.5);
            memoryText.Width = topPanel.Widgets[1].Width - 5;
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();
            arrowTimer += gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key) || arrowTimer > 0.15)
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
            playButton.Update(graphics, gameTime, this);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.DrawString(font, typeWriterStringType, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - font.MeasureString(typeWriterString).X / 2, graphics.GraphicsDevice.Viewport.Height / 2 - font.MeasureString(typeWriterString).Y / 2), Color.Black);

            
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

            _desktop.Render();
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
                
            }
            else if (key == Keys.Down && currentLine < numberOfLines -1)
            {
                currentLine++;
                currentChar = typing[currentLine].Length;
                

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
                // If line is empty; delete line.

                // If current line is the last line, go back 1 line
                
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

        public void UpdateMemoryText(GraphicsDeviceManager graphicsDevice)
        {
            staticGraphicsDevice = graphicsDevice.GraphicsDevice;
            UpdateMemoryBar();
            memoryText.Text = startingMemoryText;
            int variableIndex = 0;
            if (Interpreter.variables != null)
            {
                Dictionary<string, double> variables = new Dictionary<string, double>(Interpreter.variables);
                foreach (KeyValuePair<string, double> variable in variables)
                {
                    string printableValue = variable.Value.ToString();

                    variableIndex++;
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
                    memoryText.Text += "\n" + String.Format(variable.Key, valueStringFormatter) + ": " + printableValue + valueTooLong;
                }
            }
        }
    } 
}
