using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Myra;
using Myra.Graphics2D.UI;
using System.Runtime.InteropServices;
using System.IO;
using FontStashSharp;
using Myra.Assets;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI.Styles;
using Myra.Graphics2D.Brushes;

namespace pLdevTest
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static GameWindow gw;
        MouseState currentMouseState;

        codeInput codeTextBar;
        Color background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1200,
                PreferredBackBufferHeight = 800
            };

            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;

            IsMouseVisible = true;

            codeTextBar = new codeInput();

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.TextInput += ProcessTextInput;
            Window.ClientSizeChanged += ProcessWindowSizeChange;
            base.Initialize();
            Camera.Instance.SetFocalPoint(new Vector2(50, 0), _graphics);

            // Cap FPS to 144 FPS
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 144.0f);

        }

        private void ProcessWindowSizeChange(object sender, EventArgs e)
        {
            codeTextBar.UpdateMemoryText(_graphics);
        }
        public void ProcessTextInput(object sender, TextInputEventArgs e)
        {
            Debug.WriteLine(Convert.ToChar(e.Character));
            if(e.Key.ToString() == "Enter")
            {
                codeTextBar.typeText('\r');
            } else
            {
                codeTextBar.typeText(e.Character); 
            }
        }
        
        protected override void LoadContent()
        {
            MyraEnvironment.Game = this;

            codeTextBar.LoadContent(Content, GraphicsDevice);
            gw = Window;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            background = new Color(50, 41, 47);
            // Load stylesheet


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            codeTextBar.Update(gameTime, _graphics);
            Camera.Instance.Update();
            base.Update(gameTime);
            currentMouseState = Mouse.GetState();

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.Instance.ViewMatrix);
            codeTextBar.Draw(_spriteBatch, gameTime, _graphics);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}