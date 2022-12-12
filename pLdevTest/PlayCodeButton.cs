using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pLdevTest
{
    public class PlayCodeButton
    {
        private Rectangle buttonPos;
        private Texture2D playButtonTexture;

        private MouseState mouseState;
        private MouseState lastMouseState;
        public static bool unpressableButton = false;
        public PlayCodeButton(GraphicsDevice graphicsDevice, int buttonX, int buttonY)
        {
            playButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            playButtonTexture.SetData(new[] { Color.Green });

            buttonPos = new Rectangle(buttonX, buttonY, 30, 30);
        }

        public void Update(GraphicsDeviceManager graphics, GameTime gameTime, codeInput inputText)
        {
            mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(buttonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed && !unpressableButton)
            {
                unpressableButton = true;

                // Reset playground
                GameScene.playground.player.playerY = 0;
                GameScene.playground.player.playerX = 0;

                MissionHandler.AmountOfCoins = 0;
                MissionHandler.Coins = 0;
                if (MissionHandler.Mission == 9)
                {
                    GameScene.playground.CreateGems(9);
                }

                Interpreter.StartInterprete(codeInput.Typing, 0, codeInput.Typing.Count, gameTime);
                inputText.UpdateEditorProportions(graphics);
            }
            lastMouseState = Mouse.GetState();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(playButtonTexture, buttonPos, Color.White);
        }
    }
}
