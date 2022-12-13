using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using info.lundin.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pLdevTest
{
    public class EnemyBlock : PlaygroundObject
    {
        double moveElapsedTime;
        public EnemyBlock(int x, int y, int initialGridPosX, int initialGridPosY, int index)
        {
            posX = initialGridPosX;
            posY = initialGridPosY;
            this.index = index;

            initialPos = new Vector2(x, y);
        }

        public void Update(GameTime gameTime)
        {
            if(MissionHandler.MissionPlaying)
            {
                moveElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (moveElapsedTime > 0.1)
                {
                    if(MissionHandler.Mission == 10)
                    {
                        posX = posX - 1;
                        if (posX < 0)
                        {
                            posX = 21;
                        }
                    }
                    else if(MissionHandler.Mission == 11)
                    {
                        posY = posY - 1;
                        if (posY < 0)
                        {
                            posY = 16;
                        }
                    }
                    moveElapsedTime = 0;
                    CheckForCollision();
                }
            }
            HoverTransistion(gameTime);
        }

        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(GlobalThings.enemyTexture, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.Green * opacity);
        }

        public void CheckForCollision()
        {
            if (Interpreter.builtInVariables["enemy"]["x"].Length > (int)index)
            {
                Interpreter.builtInVariables["enemy"]["x"][(int)index] = posX;
                Interpreter.builtInVariables["enemy"]["y"][(int)index] = posY;
            }

            if (GameScene.playground.player.posX == posX && GameScene.playground.player.posY == posY)
            {
                MissionHandler.MissionFailed = true;
            }
        }
    }
}
