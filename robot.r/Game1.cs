﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using System.Transactions;

namespace robot.r
{
    public class Game1 : Game
    {
        public static bool fullscreen = false;

        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static GameWindow gw;
        public GameScene gameScene;
        private MainMenu mainMenu;
        public static CircleScreenTransistion transistion;
        public static bool menuScene = true;
        RenderTarget2D renderTarget;
        Effect crtEffect;

        public static bool ToggleCrtEffect = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {

            };
            Content.RootDirectory = "Content";

            Window.AllowUserResizing = true;

            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 100.0f);

            _graphics.ApplyChanges();
            IsMouseVisible = true;

            Window.TextInput += ProcessTextInput;
            Window.ClientSizeChanged += ProcessWindowSizeChange;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            base.Initialize();

        }

        private void ProcessWindowSizeChange(object sender, EventArgs e)
        {
            gameScene.UpdateProprtions(sender, e, _graphics);

            transistion.ResizeRenderTarget(_graphics.GraphicsDevice);
            renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        }
        public void ProcessTextInput(object sender, TextInputEventArgs e)
        {
            if(!menuScene)
            {   
                gameScene.ProcessTextInput(sender, e);
            }
        }
        
        protected override void LoadContent()
        {
            
            crtEffect = Content.Load<Effect>("CRTeffect");

            renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);

            GlobalThings.LoadContent(Content, _graphics.GraphicsDevice);

            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            mainMenu = new MainMenu(_graphics.GraphicsDevice);
            transistion = new CircleScreenTransistion(_graphics.GraphicsDevice);
            gameScene = new GameScene(_graphics.GraphicsDevice);

            mainMenu.LoadContent(Content);
            transistion.LoadContent(Content, _graphics.GraphicsDevice);
            gameScene.LoadContent(Content, _graphics.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            if(kb.IsKeyDown(Keys.Escape))
            {
                menuScene = true;
            }

            if(!menuScene)
            {
                gameScene.Update(gameTime, _graphics);

            }
            else if(menuScene)
            {
                mainMenu.Update(_graphics, Content, gameTime);
            }
            
            transistion.Update(gameTime, _graphics.GraphicsDevice);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            _spriteBatch.Begin();

            if (!menuScene)
            {
                gameScene.Draw(gameTime, _spriteBatch, _graphics);
            }
            else if(menuScene)
            {
                mainMenu.Draw(gameTime, _spriteBatch, _graphics);
            }


            if (CircleScreenTransistion.playTransistion || CircleScreenTransistion.keepScreen)
            {
                transistion.Draw(_spriteBatch, _graphics.GraphicsDevice);
            }

            _spriteBatch.End();
            _graphics.GraphicsDevice.SetRenderTarget(null);

            if(ToggleCrtEffect)
            {
                _spriteBatch.Begin(effect: crtEffect);
            }
            else
            {
                _spriteBatch.Begin();
            }
 
            _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height), Color.White);

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}