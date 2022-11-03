using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Text;

namespace pLdevTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static GameWindow gw;

        codeInput codeTextBar;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;

            IsMouseVisible = true;

            codeTextBar = new codeInput();

            static int makeInt()
            {
                return(5);
            }
            Debug.WriteLine(makeInt());
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.TextInput += ProcessTextInput;
            base.Initialize();
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
            codeTextBar.LoadContent(Content, GraphicsDevice);
            gw = Window;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            codeTextBar.Update(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            codeTextBar.Draw(_spriteBatch, gameTime, _graphics);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}