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
    public class Gem : PlaygroundObject
    {
        public bool PickedUp = false;

        public Gem(int posX, int posY, int initialGridPosX, int initialGridPosY)
        {
            initialPos = new Vector2(posX, posY);
            this.posX = initialGridPosX;
            this.posY = initialGridPosY;

            MissionHandler.AmountOfCoins++;
        }
        public void Update(GameTime gameTime)
        {
            HoverTransistion(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(GlobalThings.gemTexture, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.Yellow * opacity);
        }
        public void UpdateProportions(GraphicsDevice _graphics, int newX)
        {
            initialPos.X = newX;
        }
        public void PickUp()
        {
            if (GameScene.playground.player.posX == posX && GameScene.playground.player.posY == posY)
            {
                MissionHandler.Coins = MissionHandler.Coins + 1;
                PickedUp = true;
            }
        }
    }
}
