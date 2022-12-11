using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using System.Transactions;

namespace pLdevTest
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static GameWindow gw;
        public GameScene gameScene;
        private MainMenu mainMenu;
        public static CircleScreenTransistion transistion;
        public static bool menuScene = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {

            };
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            // Cap FPS to 60 FPS
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);

            /*_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();*/

            Window.TextInput += ProcessTextInput;
            Window.ClientSizeChanged += ProcessWindowSizeChange;
        }

        private void ProcessWindowSizeChange(object sender, EventArgs e)
        {
            gameScene.UpdateProprtions(sender, e, _graphics);
            transistion.ResizeRenderTarget(_graphics.GraphicsDevice);
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
            gw = Window;
            GlobalThings.LoadContent(Content);
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            mainMenu = new MainMenu(_graphics.GraphicsDevice);
            transistion = new CircleScreenTransistion(_graphics.GraphicsDevice);

            transistion.LoadContent(Content, _graphics.GraphicsDevice);
            gameScene = new GameScene(_graphics.GraphicsDevice);
            gameScene.LoadContent(Content, _graphics.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if(!menuScene)
            {
                gameScene.Update(gameTime, _graphics);
            }
            else if(menuScene)
            {
                mainMenu.Update(_graphics.GraphicsDevice, Content, gameTime);
            }
            
            transistion.Update(gameTime, _graphics.GraphicsDevice);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            if (CircleScreenTransistion.playTransistion || CircleScreenTransistion.keepScreen)
            {
                transistion.DrawTransistionRenderTarget(_spriteBatch, gameTime, _graphics.GraphicsDevice);
            }

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

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}