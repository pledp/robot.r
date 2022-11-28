using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public class PlaygroundPlayer
    {
        public int playerX;
        public int playerY;

        private int playerWidth = 25;
        private int playerHeight = 25;

        private Texture2D playerTexture;
        private Color playerColor;
        private Vector2 playerPos;
        public PlaygroundPlayer(GraphicsDevice _graphics, int posX, int posY)
        {
            playerPos = new Vector2(posX, posY);
            playerTexture = new Texture2D(_graphics, 1, 1);
            playerColor = new Color(255, 255, 255);
            playerTexture.SetData(new[] { playerColor });
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(playerTexture, new Rectangle((int)playerPos.X + (playerX * 25), (int)playerPos.Y + (playerY * 25), playerWidth, playerHeight), playerColor);
        }
        public void UpdateProportions(GraphicsDevice _graphics, int newX)
        {
            playerPos.X = newX;
        }
    }
}
