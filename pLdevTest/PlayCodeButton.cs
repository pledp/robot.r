using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace pLdevTest
{
    public class PlayCodeButton
    {
        private Vector2 buttonPos;
        private Texture2D playButtonTexture;

        private MouseState mouseState;
        private MouseState lastMouseState;
        public PlayCodeButton(GraphicsDevice graphicsDevice, int buttonX, int buttonY)
        {
            int rectX = 30;
            int rectY = 30;
            playButtonTexture = new Texture2D(graphicsDevice, rectX, rectY);
            Color[] data = new Color[rectX * rectY];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.Green;

            playButtonTexture.SetData(data);


            buttonPos.X = buttonX;
            buttonPos.Y = buttonY;
        }

        public bool enterButton()
        {
            if (mouseState.X < buttonPos.X + playButtonTexture.Width &&
                mouseState.X > buttonPos.X &&
                mouseState.Y < buttonPos.Y + playButtonTexture.Height &&
                mouseState.Y > buttonPos.Y)
            {
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (enterButton() && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                Interpreter.RunLines(codeInput.Typing);
            }
            lastMouseState = Mouse.GetState();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(playButtonTexture, buttonPos, Color.White);
        }
    }
}
