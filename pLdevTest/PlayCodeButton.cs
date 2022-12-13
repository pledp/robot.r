using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pLdevTest
{
    public class PlayCodeButton
    {
        private Rectangle buttonPos;
        private Rectangle cancelPos;

        private Texture2D playButtonTexture;
        private Texture2D cancelButtonTexture;

        public static List<CancellationTokenSource> CancelToken;
        public static List<CancellationToken> cancelToken;
        private int index = 0;

        private MouseState mouseState;
        private MouseState lastMouseState;
        public static bool unpressableButton = false;
        public static bool over = true;

        public static bool RunUpdate = false;
        public static int UpdateStartIndex;
        public static int UpdateEndIndex;


        public PlayCodeButton(GraphicsDevice graphicsDevice, int buttonX, int buttonY)
        {
            playButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            playButtonTexture.SetData(new[] { Color.Green });

            cancelButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            cancelButtonTexture.SetData(new[] { Color.Red });

            buttonPos = new Rectangle(buttonX, buttonY, 30, 30);
            cancelPos = new Rectangle(buttonX, buttonY+20 * 2, 30, 30);

            CancelToken = new List<CancellationTokenSource>();
            cancelToken = new List<CancellationToken>();
        }

        public void Update(GraphicsDeviceManager graphics, GameTime gameTime, codeInput inputText)
        {
            mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(buttonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed && !unpressableButton)
            {
                CancelToken.Add(new CancellationTokenSource());
                cancelToken.Add(CancelToken[index].Token);

                if (!over)
                {
                    CancelToken[index-1].Cancel();
                }

                RunUpdate = false;
                over = false;

                // Reset playground
                GameScene.playground.player.posY = 0;
                GameScene.playground.player.posX = 0;
                MissionHandler.ResetMission();
                MissionHandler.MissionPlaying = true;

                Interpreter.StartInterprete(codeInput.Typing, 0, codeInput.Typing.Count, gameTime, index);

                index++;
                inputText.UpdateEditorProportions(graphics);
            }
            if (GlobalThings.EnterArea(cancelPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed && !unpressableButton)
            {
                if (!over || MissionHandler.MissionPlaying)
                {
                    CancelToken[index - 1].Cancel();
                    RunUpdate = false;
                    over = true;
                    GameScene.playground.player.posY = 0;
                    GameScene.playground.player.posX = 0;
                    MissionHandler.ResetMission();
                }

            }

            if (RunUpdate)
            {
                Interpreter.RunLines(codeInput.Typing, UpdateStartIndex, UpdateEndIndex, gameTime, false);
            }

            lastMouseState = Mouse.GetState();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(playButtonTexture, buttonPos, Color.White);
            spriteBatch.Draw(cancelButtonTexture, cancelPos, Color.White);
        }
    }
}
