using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Text;
using System.IO;


namespace pLdevTest
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static GameWindow gw;
        public static PlayGround playground;

        MouseState currentMouseState;
        public static SpriteFont font;

        codeInput codeTextBar;
        Color background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {

            };
            /*_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();*/
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            

            IsMouseVisible = true;
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
            codeTextBar.UpdateEditorProportions(_graphics);
            playground.UpdateProportions(_graphics.GraphicsDevice);
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
            font = Content.Load<SpriteFont>("font");
            gw = Window;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = new Color(50, 41, 47);

            codeTextBar = new codeInput(font);
            codeTextBar.LoadContent(Content, GraphicsDevice);

            playground = new PlayGround(_graphics.GraphicsDevice, 550);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            codeTextBar.Update(gameTime, _graphics);
            playground.Update(gameTime);
            Camera.Instance.Update();
            base.Update(gameTime);
            currentMouseState = Mouse.GetState();

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.Instance.ViewMatrix);

            playground.Draw(_spriteBatch, gameTime, _graphics);
            codeTextBar.Draw(_spriteBatch, gameTime, _graphics);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}