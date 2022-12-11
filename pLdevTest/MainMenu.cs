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
    public class MainMenu
    {
        private Rectangle buttonPos;
        private Texture2D playButtonTexture;
        private MouseState mouseState;
        private MouseState lastMouseState;
        public MainMenu(GraphicsDevice graphicsDevice)
        {
            playButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            playButtonTexture.SetData(new[] { Color.Green }); 

            buttonPos = new Rectangle(50, 50, (int)GlobalThings.font.MeasureString("PLAY").X + 40, (int)GlobalThings.font.MeasureString("PLAY").Y + 10);
        }
        public void Update(GraphicsDevice _graphics, ContentManager Content)
        {
            mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(buttonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                GameSceneTransistion(_graphics, Content);
            }
            lastMouseState = Mouse.GetState();
        }
        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(playButtonTexture, new Rectangle(buttonPos.X-20, buttonPos.Y-5, buttonPos.Width, buttonPos.Height), Color.White);
            _spriteBatch.DrawString(GlobalThings.font, "PLAY", new Vector2(buttonPos.X, buttonPos.Y), Color.White);
        }
        public async void GameSceneTransistion(GraphicsDevice _graphics, ContentManager Content)
        {
            // Don't look at this please
            CircleScreenTransistion.playTransistion = true;
            CircleScreenTransistion.keepScreen = true;

            await Task.Delay(5000);
            Game1.menuScene = false;

            CircleScreenTransistion.playTransistion = true;
        }
    }
}
