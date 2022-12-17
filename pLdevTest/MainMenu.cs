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
        private Rectangle engButtonPos;
        private Texture2D engTexture;

        private Rectangle finButtonPos;
        private Texture2D finTexture;

        private Rectangle selectedLangPos;
        private Vector2 startingPos;
        Vector2 minPos;
        private Texture2D playButtonTexture;
        private MouseState mouseState;
        private MouseState lastMouseState;
        private double elapsedTime = 0;
        private double desiredDuration = 1f;

        bool aniDone = false;
        public MainMenu(GraphicsDevice graphicsDevice)
        {
            playButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            playButtonTexture.SetData(new[] { Color.Green }); 

            buttonPos = new Rectangle(50, 50, (int)GlobalThings.font.MeasureString("PLAY").X + 40, (int)GlobalThings.font.MeasureString("PLAY").Y + 10);
            engButtonPos = new Rectangle(40, 150, 50, 50);
            engTexture = new Texture2D(graphicsDevice, 1, 1);
            engTexture.SetData(new[] { Color.Red });

            finButtonPos = new Rectangle(40, 250, 50, 50);
            finTexture = new Texture2D(graphicsDevice, 1, 1);
            finTexture.SetData(new[] { Color.Blue });

            selectedLangPos = new Rectangle(30, 140, 70, 70);

            minPos = new Vector2((int)GlobalThings.font.MeasureString("PLAY").X + 40, (int)GlobalThings.font.MeasureString("PLAY").Y + 10);
            startingPos = new Vector2(buttonPos.Width, buttonPos.Height);
        }
        public void Update(GraphicsDevice _graphics, ContentManager Content, GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(buttonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                GameSceneTransistion(_graphics, Content);
            }

            if (GlobalThings.EnterArea(engButtonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                LanguageHandler.language = 0;
                selectedLangPos = new Rectangle(30, 140, 70, 70);
            }
            if (GlobalThings.EnterArea(finButtonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                LanguageHandler.language = 1;
                selectedLangPos = new Rectangle(30, 240, 70, 70);
            }

            if (GlobalThings.EnterArea(buttonPos, mouseState))
            {
                AnimateMenuItem(new Vector2(minPos.X + 100, buttonPos.Height), gameTime);
                aniDone = true;
            }
            else
            {
                if(aniDone)
                {
                    elapsedTime = 0;
                    startingPos = new Vector2(buttonPos.Width, buttonPos.Height);
                    aniDone = false;
                }
                AnimateMenuItem(minPos, gameTime);
            }
            lastMouseState = Mouse.GetState();
        }
        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(playButtonTexture, new Rectangle(buttonPos.X-20, buttonPos.Y-5, buttonPos.Width, buttonPos.Height), Color.White);
            _spriteBatch.Draw(GlobalThings.enemyTexture, selectedLangPos, Color.White);
            _spriteBatch.Draw(engTexture, engButtonPos, Color.White);
            _spriteBatch.Draw(finTexture, finButtonPos, Color.White);

            _spriteBatch.DrawString(GlobalThings.font, "PLAY", new Vector2(buttonPos.X, buttonPos.Y), Color.White);
        }
        private void AnimateMenuItem(Vector2 newPos, GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            float percentageComplete = (float)elapsedTime / (float)desiredDuration;
            buttonPos.Width = (int)Vector2.Lerp(startingPos, newPos, MathHelper.SmoothStep(0, 1, percentageComplete)).X;

            if (buttonPos.Width == newPos.X)
            {
                if(aniDone)
                {
                    aniDone = false;
                }
                else
                {
                    aniDone = true;
                }
                startingPos = new Vector2(buttonPos.Width, buttonPos.Height);
                elapsedTime = 0;
            }
        }
        public async void GameSceneTransistion(GraphicsDevice _graphics, ContentManager Content)
        {
            // Start transistion
            CircleScreenTransistion.playTransistion = true;
            CircleScreenTransistion.keepScreen = true;

            await Task.Delay(5000);
            Game1.menuScene = false;
            MissionHandler.FormatMissionText();

            CircleScreenTransistion.playTransistion = true;
        }
    }
}
