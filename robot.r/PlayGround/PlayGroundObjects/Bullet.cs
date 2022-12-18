using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pLdevTest
{
    public class Bullet : PlaygroundObject
    {
        double moveElapsedTime;
        public bool spent = false;

        public Bullet(int gridPosX, int gridPosY, int posX, int posY)
        {
            this.posX = gridPosX;
            this.posY = gridPosY;

            initialPos = new Vector2(posX, posY);
        }
        public void Update(GameTime gameTime)
        {
            if (MissionHandler.MissionPlaying)
            {
                moveElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (moveElapsedTime > 0.2)
                {
                    posX = posX + 1;
                    moveElapsedTime = 0;
                }
            }
            HoverTransistion(gameTime);
        }

        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(GlobalThings.enemyTexture, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.Gray * opacity);
        }
    }
}
