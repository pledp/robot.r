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
    public class Gem
    {
        public int gemX;
        public int gemY;

        private int gemWidth = 25;
        private int gemHeight = 25;
        public bool PickedUp = false;

        private Vector2 gemPos;

        public Gem(int posX, int posY, int initialGridPosX, int initialGridPosY)
        {
            gemPos = new Vector2(posX, posY);
            gemX = initialGridPosX;
            gemY = initialGridPosY;

            MissionHandler.AmountOfCoins++;
        }
        public void PickUp()
        {
            if (GameScene.playground.player.playerX == gemX && GameScene.playground.player.playerY == gemY)
            {
                MissionHandler.Coins = MissionHandler.Coins + 1;
                PickedUp = true;
                Debug.WriteLine("test");
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(GlobalThings.gemTexture, new Rectangle((int)gemPos.X + (gemX * 25), (int)gemPos.Y + (gemY * 25), gemWidth, gemHeight), Color.Yellow);
        }
        public void UpdateProportions(GraphicsDevice _graphics, int newX)
        {
            gemPos.X = newX;
        }
    }
}
