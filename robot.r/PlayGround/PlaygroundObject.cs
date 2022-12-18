using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    public class PlaygroundObject
    {
        // Blueprint for playground objects
        public int posX;
        public int posY;

        public double elapsedTime;
        public float opacity;

        public Vector2 initialPos;
        public Vector2 objectPos;

        public int objectWidth = 25;
        public int objectHeight = 25;

        public int index;

        public void HoverTransistion(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            objectPos = GameScene.playground.TileTransistion(new Vector2(initialPos.X, initialPos.Y), gameTime, new Vector2(initialPos.X, initialPos.Y - 20), elapsedTime, posX*2, posY*2);
            opacity = GameScene.playground.TileTransistion(new Vector2(1, 0), gameTime, new Vector2(0, 0), elapsedTime, posX, posY).X;
        }

        public void UpdateProportions(GraphicsDevice _graphics, int newX)
        {
            initialPos.X = newX;
        }
    }
}
