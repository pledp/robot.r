using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pLdevTest
{
    public class PlayCodeButton
    {
        private Rectangle buttonPos;
        private Rectangle cancelPos;

        private Texture2D playButtonTexture;
        private Texture2D playButtonTexturePressed;
        private Texture2D cancelButtonTexture;
        private Texture2D cancelButtonTexturePressed;

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
            buttonPos = new Rectangle(buttonX, buttonY, 60, 60);
            cancelPos = new Rectangle(buttonX, buttonY + 70, 60, 60);

            CancelToken = new List<CancellationTokenSource>();
            cancelToken = new List<CancellationToken>();
        }
        public void LoadContent(ContentManager Content)
        {
            playButtonTexture = Content.Load<Texture2D>("playButton");
            playButtonTexturePressed = Content.Load<Texture2D>("playButtonPressed");

            cancelButtonTexture = Content.Load<Texture2D>("StopButton");
            cancelButtonTexturePressed = Content.Load<Texture2D>("StopButtonPressed");

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

                codeInput.errorLine = -1;

                RunUpdate = false;
                over = false;

                // Reset playground
                GameScene.playground.player.posY = 0;
                GameScene.playground.player.posX = 0;
                codeInput.madeAlready = false;
                codeInput.errorIndicator = null;

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
                    try
                    {
                        CancelToken[index - 1].Cancel();
                        RunUpdate = false;
                        over = true;
                        GameScene.playground.player.posY = 0;
                        GameScene.playground.player.posX = 0;
                        MissionHandler.ResetMission();
                    }
                    catch
                    {
                        Debug.WriteLine("test");
                    }
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
            Texture2D playButton;
            Texture2D cancelButton;
            if (!over)
            {
                playButton = playButtonTexturePressed;
                cancelButton = cancelButtonTexture;
            }
            else
            {
                playButton = playButtonTexture;
                cancelButton = cancelButtonTexturePressed;
            }

            spriteBatch.Draw(playButton, buttonPos, Color.White);
            spriteBatch.Draw(cancelButton, cancelPos, Color.White);
        }
    }
}
