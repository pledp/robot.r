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
    public class PlaygroundPlayer : PlaygroundObject
    {
        private Texture2D robotTexture;

        public PlaygroundPlayer(GraphicsDevice _graphics, int posX, int posY)
        {
            initialPos = new Vector2(posX, posY);
        }
        public void Update(GameTime gameTime)
        {
            HoverTransistion(gameTime);
        }
        public void LoadContent(ContentManager Content)
        {
            robotTexture = Content.Load<Texture2D>("robot");
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(robotTexture, new Rectangle((int)objectPos.X + (posX * 25), (int)objectPos.Y + (posY * 25), objectWidth, objectHeight), Color.White * opacity);
        }
        public void UpdateProportions(GraphicsDevice _graphics, int newX)
        {
            initialPos.X = newX;
        }
    }
}
