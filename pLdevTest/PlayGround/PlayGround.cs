using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pLdevTest
{
    public class PlayGround
    {
        private Rectangle playground;
        private int width;
        private Texture2D pgTexture;
        public Color pgColor;

        public PlaygroundPlayer player;
        public PlayGround(GraphicsDevice _graphics, int playgroundWidth)
        {
            // Create a playground
            width = playgroundWidth;
            playground = new Rectangle(_graphics.Viewport.Width - width - 50, 10, width, 400);
            
            pgTexture = new Texture2D(_graphics, 1, 1);
            pgColor = new Color(153, 225, 217);
            pgTexture.SetData(new[] { pgColor });

            // Create a player on the playground. Move in a 21x15 grid.
            player = new PlaygroundPlayer(_graphics, playground.X, playground.Y);
        }
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            spriteBatch.Draw(pgTexture, playground, pgColor);
            player.Draw(spriteBatch, gameTime, _graphics);
        }
        public void UpdateProportions(GraphicsDevice _graphics)
        {
            playground.X = _graphics.Viewport.Width - width - 50;
            player.UpdateProportions(_graphics, playground.X);
        }
    }
}
