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

namespace robot.r
{
    public class Gem : PlaygroundObject
    {
        public bool PickedUp = false;

        public Gem(int posX, int posY, int initialGridPosX, int initialGridPosY, int index)
        {
            initialPos = new Vector2(posX, posY);
            this.posX = initialGridPosX;
            this.posY = initialGridPosY;
            this.index = index;

            MissionHandler.AmountOfCoins++;
        }
        public void Update(GameTime gameTime)
        {
            HoverTransistion(gameTime);
            if (MissionHandler.MissionPlaying)
            {
                if (Interpreter.builtInVariables["gem"]["x"].Length > (int)index)
                {
                    Interpreter.builtInVariables["gem"]["x"][index] = posX;
                    Interpreter.builtInVariables["gem"]["y"][index] = posY;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(GlobalThings.gemTexture, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.Yellow * opacity);
        }

        public void PickUp()
        {
            if (GameScene.playground.player.posX == posX && GameScene.playground.player.posY == posY)
            {
                Debug.WriteLine("pickup");
                MissionHandler.Coins = MissionHandler.Coins + 1;
                Debug.WriteLine(MissionHandler.Coins);
                PickedUp = true;
            }
        }
    }
}
