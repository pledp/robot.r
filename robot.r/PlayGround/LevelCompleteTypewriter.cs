using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    public class LevelCompleteTypewriter
    {
        string typeWriterStringType = "";
        string typeWriterString = "Tutorial Complete!";
        public static bool play = false;
        private double typeWriterTimer;
        public void Update(GameTime gameTime)
        {
            if (typeWriterStringType.Length < typeWriterString.Length)
            {
                typeWriterTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (typeWriterTimer > 0.2)
                {
                    typeWriterTimer = 0;
                    typeWriterStringType += typeWriterString[typeWriterStringType.Length];
                }
            }
        }
        public void Draw(SpriteBatch _spriteBatch,GameTime gameTime, GraphicsDevice _graphics)
        {
            string typeWriterStringPrinted = "";
            for (int x = 0; x < typeWriterStringType.Length; x++)
            {
                _spriteBatch.DrawString(GlobalThings.font, typeWriterStringType[x].ToString(), new Vector2(_graphics.Viewport.Width / 2 - GlobalThings.font.MeasureString(typeWriterString).X / 2 + GlobalThings.font.MeasureString(typeWriterStringPrinted).X, _graphics.Viewport.Height / 2 - GlobalThings.font.MeasureString(typeWriterString).Y / 2 + (int)(Math.Sin((gameTime.TotalGameTime.TotalSeconds + x) * 5) * 5)), CircleScreenTransistion.transistionColor);
                typeWriterStringPrinted = typeWriterStringPrinted + typeWriterString[x];
            }

        }
    }
}
