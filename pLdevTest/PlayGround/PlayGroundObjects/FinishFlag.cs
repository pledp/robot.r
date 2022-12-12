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

namespace pLdevTest
{
    public class FinishFlag : PlaygroundObject
    {
        public int flagX;
        public int flagY;

        private Texture2D flagTexture;

        public FinishFlag(GraphicsDevice _graphics, int posX, int posY, int initialGridPosX, int initialGridPosY)
        {
            flagTexture = new Texture2D(_graphics, 1, 1);
            flagTexture.SetData(new[] { Color.Green });

            initialPos = new Vector2(posX, posY);
            this.posX = initialGridPosX;
            this.posY = initialGridPosY;
        }

        public void Update(GameTime gameTime)
        {
            HoverTransistion(gameTime);
            Debug.WriteLine("test");
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(flagTexture, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.Green * opacity);
        }
        public void UpdateProportions(GraphicsDevice _graphics, int newX)
        {
            initialPos.X = newX;
        }
    }
}
