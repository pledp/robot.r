using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace pLdevTest
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static GameWindow gw;
        private GameScene gameScene;

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
        }
        public void ProcessTextInput(object sender, TextInputEventArgs e)
        {
            gameScene.ProcessTextInput(sender, e);
        }
        
        protected override void LoadContent()
        {
            gw = Window;
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            gameScene = new GameScene(_graphics);
            gameScene.LoadContent(Content, _graphics);

        }

        protected override void Update(GameTime gameTime)
        {
            //gameScene.Update(gameTime, _graphics);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gameScene.Draw(gameTime, _spriteBatch, _graphics);
            base.Draw(gameTime);
        }
    }
}