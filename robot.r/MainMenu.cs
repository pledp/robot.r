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

namespace robot.r
{
    public class MainMenu
    {
        private Vector2 langSelector;

        private Rectangle buttonPos;
        private Rectangle engButtonPos;
        private Texture2D engTexture;

        private Texture2D pLdevLogo;

        private Rectangle finButtonPos;
        private Texture2D finTexture;

        private Rectangle crtButtonPos;
        private Texture2D crtButtonTexture;

        private Rectangle fullscreenButtonPos;
        private Texture2D fullscreenButtonTexture;

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

            crtButtonPos = new Rectangle(40, 350, 50, 50);
            crtButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            crtButtonTexture.SetData(new[] { Color.LightPink });

            fullscreenButtonPos = new Rectangle(40, 450, 50, 50);
            fullscreenButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            fullscreenButtonTexture.SetData(new[] { Color.White });

            langSelector = new Vector2(engButtonPos.X + 10, engButtonPos.Y);

            minPos = new Vector2((int)GlobalThings.font.MeasureString("PLAY").X + 40, (int)GlobalThings.font.MeasureString("PLAY").Y + 10);
            startingPos = new Vector2(buttonPos.Width, buttonPos.Height);
        }
        public void LoadContent(ContentManager Content)
        {
            pLdevLogo = Content.Load<Texture2D>("pLdevLogo");
        }
        public void Update(GraphicsDeviceManager _graphics, ContentManager Content, GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (GlobalThings.EnterArea(buttonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                GameSceneTransistion(_graphics.GraphicsDevice, Content);
            }


            if (GlobalThings.EnterArea(engButtonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                LanguageHandler.language = 0;
                langSelector = new Vector2(engButtonPos.X+10, engButtonPos.Y);
            }
            if (GlobalThings.EnterArea(finButtonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                LanguageHandler.language = 1;
                langSelector = new Vector2(finButtonPos.X+10, finButtonPos.Y);
            }

            // CRT effect ON and OFF
            if (GlobalThings.EnterArea(crtButtonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                Game1.ToggleCrtEffect = !Game1.ToggleCrtEffect;
            }

            if (GlobalThings.EnterArea(fullscreenButtonPos, mouseState) && lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                Game1.fullscreen = !Game1.fullscreen;
                if (Game1.fullscreen)
                {
                    _graphics.HardwareModeSwitch = false;
                    _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    _graphics.IsFullScreen = true;
                    _graphics.ApplyChanges();
                }
                else
                {
                    _graphics.PreferredBackBufferWidth = 800;
                    _graphics.PreferredBackBufferHeight = 480;
                    _graphics.IsFullScreen = false;
                    _graphics.ApplyChanges();
                }
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

            _spriteBatch.DrawString(GlobalThings.font, "robot.r",new Vector2(_graphics.GraphicsDevice.Viewport.Width - pLdevLogo.Width - 50, _graphics.GraphicsDevice.Viewport.Height - pLdevLogo.Height - 100), Color.White);

            _spriteBatch.Draw(playButtonTexture, new Rectangle(buttonPos.X-20, buttonPos.Y-5, buttonPos.Width, buttonPos.Height), Color.White);

            _spriteBatch.Draw(engTexture, engButtonPos, Color.White);
            _spriteBatch.DrawString(GlobalThings.font, "English", new Vector2(engButtonPos.X + 70, engButtonPos.Y), Color.White);

            _spriteBatch.Draw(finTexture, finButtonPos, Color.White);
            _spriteBatch.DrawString(GlobalThings.font, "Suomi", new Vector2(finButtonPos.X + 70, finButtonPos.Y), Color.White);

            _spriteBatch.Draw(crtButtonTexture, crtButtonPos, Color.White);
            _spriteBatch.DrawString(GlobalThings.font, "CRT-Effect", new Vector2(crtButtonPos.X+ 70, crtButtonPos.Y), Color.White);
            if(Game1.ToggleCrtEffect)
            {
                _spriteBatch.DrawString(GlobalThings.font, "X", new Vector2(crtButtonPos.X+10, crtButtonPos.Y), Color.Black);
            }
            _spriteBatch.DrawString(GlobalThings.font, "X", langSelector, Color.Black);

            _spriteBatch.Draw(fullscreenButtonTexture, fullscreenButtonPos, Color.White);


            if (Game1.fullscreen)
            {
                _spriteBatch.DrawString(GlobalThings.font, "X", new Vector2(fullscreenButtonPos.X + 10, fullscreenButtonPos.Y), Color.Black);
            }
            _spriteBatch.DrawString(GlobalThings.font, "Fullscreen", new Vector2(fullscreenButtonPos.X + 70, fullscreenButtonPos.Y), Color.White);

            _spriteBatch.DrawString(GlobalThings.font, "PLAY", new Vector2(buttonPos.X, buttonPos.Y), Color.White);
            _spriteBatch.Draw(GlobalThings.whiteTexture, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - pLdevLogo.Width - 50, _graphics.GraphicsDevice.Viewport.Height - pLdevLogo.Height - 50, pLdevLogo.Width, pLdevLogo.Height), Color.White);
            _spriteBatch.Draw(pLdevLogo, new Rectangle(_graphics.GraphicsDevice.Viewport.Width - pLdevLogo.Width - 50, _graphics.GraphicsDevice.Viewport.Height - pLdevLogo.Height - 50, pLdevLogo.Width, pLdevLogo.Height), Color.White);
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
