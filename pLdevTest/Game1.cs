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
        private MissionInfo missionInfo;

        MouseState currentMouseState;
        public static SpriteFont font;
        public static SpriteFont smallerFont;

        codeInput codeTextBar;
        public static Color background;
        public static Color orange;
        private Texture2D backgroundTexture;

        CircleScreenTransistion transistion;

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
            Window.TextInput += ProcessTextInput;
            Window.ClientSizeChanged += ProcessWindowSizeChange;
            base.Initialize();
            Camera.Instance.SetFocalPoint(new Vector2(50, 0), _graphics);

            // Cap FPS to 60 FPS
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);
            /*_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();*/
        }

        private void ProcessWindowSizeChange(object sender, EventArgs e)
        {
            codeTextBar.UpdateEditorProportions(_graphics);
            playground.UpdateProportions(_graphics.GraphicsDevice);
            missionInfo.UpdateProportions(_graphics.GraphicsDevice);

            transistion.ResizeRenderTarget(_graphics.GraphicsDevice);
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
            smallerFont = Content.Load<SpriteFont>("smallerFont");
         
            gw = Window;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = new Color(50, 41, 47);
            orange = new Color(255, 165, 0);
            backgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new[] { background });

            codeTextBar = new codeInput(font);

            playground = new PlayGround(_graphics.GraphicsDevice, 550);
            missionInfo = new MissionInfo(_graphics.GraphicsDevice);
            transistion = new CircleScreenTransistion(_graphics.GraphicsDevice);
            MissionHandler.SetWorld();
            MissionHandler.FormatMissionText();

            playground.LoadContent(Content, GraphicsDevice);
            transistion.LoadContent(Content, GraphicsDevice);
            codeTextBar.LoadContent(Content, GraphicsDevice);
            missionInfo.LoadContet(Content);
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            codeTextBar.Update(gameTime, _graphics);
            playground.Update(gameTime);
            transistion.Update(gameTime, _graphics.GraphicsDevice);
            missionInfo.Update(gameTime);
            Camera.Instance.Update();
            base.Update(gameTime);
            currentMouseState = Mouse.GetState();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);
            _spriteBatch.Begin();
            if(MissionHandler.World == 2)
            {
                playground.Draw(_spriteBatch, gameTime, _graphics);
            }
            if(CircleScreenTransistion.playTransistion || CircleScreenTransistion.keepScreen)
            {
                transistion.DrawTransistionRenderTarget(_spriteBatch, gameTime, _graphics.GraphicsDevice);
            }

            GraphicsDevice.Clear(background);

            missionInfo.Draw(_spriteBatch, gameTime, _graphics);

            if(MissionHandler.World == 2)
            {
                playground.Draw2(_spriteBatch, gameTime, _graphics);
            } 
            else
            {
                playground.DrawBoard(_spriteBatch, gameTime, _graphics);
            }
            
            codeTextBar.Draw(_spriteBatch, gameTime, _graphics);

            if (CircleScreenTransistion.playTransistion || CircleScreenTransistion.keepScreen)
            {
                transistion.Draw(_spriteBatch, _graphics.GraphicsDevice);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}