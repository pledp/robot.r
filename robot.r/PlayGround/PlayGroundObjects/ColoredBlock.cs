﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace robot.r
{
    public class ColoredBlock : PlaygroundObject
    {
        public ColorBlocksEnum blockColor;
        Texture2D color;
        public bool sorted = false;
        public ColoredBlock(int x, int y, int initialGridPosX, int initialGridPosY, int index)
        {
            posX = initialGridPosX;
            posY = initialGridPosY;
            this.index = index;

            initialPos = new Vector2(x, y);


            // Give object random color.
            Random rand = new Random();
            int randomInt = rand.Next(0, 3);
            if(randomInt == 0)
            {
                blockColor = ColorBlocksEnum.Red;
                color = GlobalThings.enemyTexture;
            }
            else if (randomInt == 1)
            {
                blockColor = ColorBlocksEnum.Green;
                color = GlobalThings.greenTexture;
            }
            else if (randomInt == 2)
            {
                blockColor = ColorBlocksEnum.Blue;
                color = GlobalThings.blueTexture;
            }
        }
        public void Update(GameTime gameTime)
        {
            if (MissionHandler.MissionPlaying)
            {
                if (Interpreter.builtInVariables["colorBlock"]["x"].Length > (int)index)
                {
                    Interpreter.builtInVariables["colorBlock"]["x"][(int)index] = posX;
                    Interpreter.builtInVariables["colorBlock"]["y"][(int)index] = posY;

                }

                if (MissionHandler.MissionCategory[MissionHandler.Mission] == MissionTypes.SortLevel && !sorted)
                {
                    CheckForCollision();
                }
            }
            
            HoverTransistion(gameTime);
        }
        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(color, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.White * opacity);
        }

        public void CheckForCollision()
        {
            if (MissionHandler.MissionPlaying)
            {
                bool foundCollision = false;
                if (blockColor == ColorBlocksEnum.Red)
                {
                    if (posY == 1)
                    {
                        foundCollision = true;
                    }
                }
                else if (blockColor == ColorBlocksEnum.Green)
                {
                    if (posY == 2)
                    {
                        foundCollision = true;
                    }
                }
                else if (blockColor == ColorBlocksEnum.Blue)
                {
                    if (posY == 3)
                    {
                        foundCollision = true;
                    }
                }

                if(foundCollision)
                {
                    MissionHandler.SortedColorBlocks++;
                    sorted = true;
                    Debug.WriteLine(MissionHandler.SortedColorBlocks + "/" + MissionHandler.AmountOfColorBlocks);

                    if(MissionHandler.SortedColorBlocks == MissionHandler.AmountOfColorBlocks)
                    {
                        MissionHandler.CheckForMission();
                    }
                }
            }
        }
    }
}
