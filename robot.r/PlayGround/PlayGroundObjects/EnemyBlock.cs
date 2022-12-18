using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using info.lundin.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace robot.r
{
    public class EnemyBlock : PlaygroundObject
    {
        double moveElapsedTime;
        public bool killed = false;
        bool alreadyFound = false;
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
                if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.EnemyLevel)
                {
                    moveElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (moveElapsedTime > 0.2)
                    {
                        if (MissionHandler.Mission == 10)
                        {
                            posX = posX - 1;
                            if (posX < 0)
                            {
                                posX = 21;
                            }
                        }
                        else if (MissionHandler.Mission == 11)
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
                else if(MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.KillLevel)
                {
                    moveElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (moveElapsedTime > 0.2)
                    {
                        posX = posX - 1;
                        moveElapsedTime = 0;
                    }
                    CheckForCollision();
                }
            }
            HoverTransistion(gameTime);
        }

        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(GlobalThings.enemyTexture, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.Red * opacity);
        }

        public void CheckForCollision()
        {
            if(MissionHandler.MissionPlaying)
            {
                if (Interpreter.builtInVariables["enemy"]["x"].Length > (int)index)
                {
                    Interpreter.builtInVariables["enemy"]["x"][(int)index] = posX;
                    Interpreter.builtInVariables["enemy"]["y"][(int)index] = posY;

                    if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.KillLevel)
                    {
                        foreach (Bullet bullet in GameScene.playground.bullets)
                        {
                            if (bullet.posX == posX && bullet.posY == posY && !bullet.spent && !alreadyFound)
                            {
                                killed = true;

                                MissionHandler.KilledEnemies++;

                                bullet.spent = true;

                                if (MissionHandler.KilledEnemies == MissionHandler.AmountOfEnemies)
                                {
                                    MissionHandler.CheckForMission();
                                }
                                alreadyFound = true;
                            }
                        }
                    }
                }
            }

            if(MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.EnemyLevel)
            {
                if (GameScene.playground.player.posX == posX && GameScene.playground.player.posY == posY)
                {
                    MissionHandler.MissionFailed = true;
                }
            }
        }
    }
}
