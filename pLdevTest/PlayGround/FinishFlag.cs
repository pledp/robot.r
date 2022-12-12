using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public class FinishFlag
    {
        public int flagX;
        public int flagY;

        private int flagWidth = 25;
        private int flagHeight = 25;

        private Texture2D flagTexture;
        private Vector2 flagPos;

        public FinishFlag(GraphicsDevice _graphics, int posX, int posY, int initialGridPosX, int initialGridPosY)
        {
            flagPos = new Vector2(posX, posY);
            flagTexture = new Texture2D(_graphics, 1, 1);
            flagTexture.SetData(new[] { Color.Green });
            flagX = initialGridPosX;
            flagY = initialGridPosY;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(flagTexture, new Rectangle((int)flagPos.X + (flagX * 25), (int)flagPos.Y + (flagY * 25), flagWidth, flagHeight), Color.Green);
        }
        public void UpdateProportions(GraphicsDevice _graphics, int newX)
        {
            flagPos.X = newX;
        }
    }
}
