using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
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
            _spriteBatch.DrawString(GlobalThings.font, typeWriterStringType, new Vector2(_graphics.Viewport.Width / 2 - GlobalThings.font.MeasureString(typeWriterString).X / 2, _graphics.Viewport.Height / 2 - GlobalThings.font.MeasureString(typeWriterString).Y / 2), CircleScreenTransistion.transistionColor);
        }
    }
}
