using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    public class TextBubble
    {
        double pauseTimer;

        public int currentLine = 0;
        public bool isPlaying = true;

        public string typeWriterStringType = "";
        public string[][] typeWriterString =
        {
            new[]{
                "The Aliens are invading!",
                "The End Of The World is coming!",
                "But, apparently if you defeat all these so called \"Missions\", they will surrender!"
            },
            new[]{
                "Avaruusolennot hyökkäävät!",
                "Maailmanloppu on tulemassa!",
                "Mutta, ilmeisesti jos lyöt kaikki nämä niin sanotut \"Tehtävät\", he antautuvat!"
            }
        };
        private double typeWriterTimer;
        public void Update(GameTime gameTime)
        {
            if(isPlaying)
            {
                if (typeWriterStringType.Length < typeWriterString[LanguageHandler.language][currentLine].Length)
                {
                    typeWriterTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (typeWriterTimer > 0.1)
                    {
                        typeWriterTimer = 0;
                        typeWriterStringType += typeWriterString[LanguageHandler.language][currentLine][typeWriterStringType.Length];
                    }
                }
                else
                {
                    pauseTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (pauseTimer > 2)
                    {
                        if (currentLine < typeWriterString[0].Length-1)
                        {
                            currentLine++;
                            typeWriterStringType = "";
                        }
                        else
                        {
                            typeWriterStringType = "";
                            currentLine = 0;
                            isPlaying = false;
                        }
                        pauseTimer = 0;
                    }
                }
            }
        }
        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDevice _graphics)
        {
            string typeWriterStringPrinted = "";
            for (int x = 0; x < typeWriterStringType.Length; x++)
            {
                _spriteBatch.DrawString(GlobalThings.font, typeWriterStringType[x].ToString(), new Vector2(_graphics.Viewport.Width / 2 - GlobalThings.font.MeasureString(typeWriterString[LanguageHandler.language][currentLine]).X / 2 + GlobalThings.font.MeasureString(typeWriterStringPrinted).X + 10, _graphics.Viewport.Height / 2 - GlobalThings.font.MeasureString(typeWriterString[LanguageHandler.language][currentLine]).Y / 2 + (int)(Math.Sin((gameTime.TotalGameTime.TotalSeconds) * 3) * 5) + 10), Color.Black);
                _spriteBatch.DrawString(GlobalThings.font, typeWriterStringType[x].ToString(), new Vector2(_graphics.Viewport.Width / 2 - GlobalThings.font.MeasureString(typeWriterString[LanguageHandler.language][currentLine]).X / 2 + GlobalThings.font.MeasureString(typeWriterStringPrinted).X, _graphics.Viewport.Height / 2 - GlobalThings.font.MeasureString(typeWriterString[LanguageHandler.language][currentLine]).Y / 2 + (int)(Math.Sin((gameTime.TotalGameTime.TotalSeconds) * 3) * 5)), Color.White);

                typeWriterStringPrinted = typeWriterStringPrinted + typeWriterString[LanguageHandler.language][currentLine][x];
            }

        }
    }
}