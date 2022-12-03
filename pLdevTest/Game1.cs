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

        codeInput codeTextBar;
        public static Color background;
        private Texture2D backgroundTexture;

        private Effect maskEffect;
        private Texture2D transistionMaskTexture;
        private Texture2D transistionTexture;
        RenderTarget2D transistionMask;
        RenderTarget2D mainTarget;
        Rectangle transistionPos;

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


            transistionTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            transistionTexture.SetData(new[] { Color.White });

            transistionPos = new Rectangle(_graphics.GraphicsDevice.Viewport.Width/2 - 100, _graphics.GraphicsDevice.Viewport.Height/2 - 100,200, 200);
        }

        private void ProcessWindowSizeChange(object sender, EventArgs e)
        {
            codeTextBar.UpdateEditorProportions(_graphics);
            playground.UpdateProportions(_graphics.GraphicsDevice);

            var pp = _graphics.GraphicsDevice.PresentationParameters;
            transistionMask = new RenderTarget2D(
                _graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
               _graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
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
            transistionMaskTexture = Content.Load<Texture2D>("transistionMask");
            maskEffect = Content.Load<Effect>("MaskShader");

            var pp = _graphics.GraphicsDevice.PresentationParameters;
            transistionMask = new RenderTarget2D(
                _graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
               _graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

            gw = Window;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = new Color(50, 41, 47);
            backgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new[] { background });

            codeTextBar = new codeInput(font);
            codeTextBar.LoadContent(Content, GraphicsDevice);

            playground = new PlayGround(_graphics.GraphicsDevice, 550);
            missionInfo = new MissionInfo(_graphics.GraphicsDevice);
            playground.LoadContent(Content, GraphicsDevice);

            
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

            transistionPos.Y = _graphics.GraphicsDevice.Viewport.Height / 2 - transistionPos.Height / 2;
            transistionPos.X = _graphics.GraphicsDevice.Viewport.Width/2 - transistionPos.Width / 2;
            transistionPos.Width += 3;
            transistionPos.Height += 3;

        }

        public void DrawTransistionRenderTarget(SpriteBatch _spriteBatch, GameTime gameTime)
        {
            // Create mask for circle transistion
            _spriteBatch.End();
            _graphics.GraphicsDevice.SetRenderTarget(transistionMask);
            _graphics.GraphicsDevice.Clear(Color.Transparent);

            // Create light mask
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            _spriteBatch.Draw(transistionMaskTexture, transistionPos, Color.White);
            _spriteBatch.End();

            // Draw to render texture
            _graphics.GraphicsDevice.SetRenderTarget(mainTarget);
            _graphics.GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0,0, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height), Color.Black);
            _spriteBatch.End();

            _graphics.GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin();

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);
            _spriteBatch.Begin();
            playground.Draw(_spriteBatch, gameTime, _graphics);
            DrawTransistionRenderTarget(_spriteBatch, gameTime);
            _spriteBatch.End();
            _graphics.GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(background);

            _spriteBatch.Begin();
            missionInfo.Draw(_spriteBatch, gameTime, _graphics);
            playground.Draw2(_spriteBatch, gameTime, _graphics);
            codeTextBar.Draw(_spriteBatch, gameTime, _graphics);

            // Draw transistion, TODO: move to class
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width, transistionPos.Y), Color.Black);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, transistionPos.Y + transistionPos.Height, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Width - (transistionPos.Y + transistionPos.Height)), Color.Black);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, transistionPos.Y, transistionPos.X, transistionPos.Height), Color.Black);
            _spriteBatch.Draw(transistionTexture, new Rectangle(transistionPos.X + transistionPos.Width, transistionPos.Y, _graphics.GraphicsDevice.Viewport.Width - (transistionPos.X + transistionPos.Width), transistionPos.Height), Color.Black);

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            maskEffect.Parameters["Mask"].SetValue(transistionMask);
            maskEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(mainTarget, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}