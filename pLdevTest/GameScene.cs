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
    public class GameScene
    {
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
        LevelCompleteTypewriter levelCompleteTypewriter;

        public GameScene(GraphicsDeviceManager _graphics)
        {
            playground = new PlayGround(_graphics.GraphicsDevice, 550);
            missionInfo = new MissionInfo(_graphics.GraphicsDevice);
            transistion = new CircleScreenTransistion(_graphics.GraphicsDevice);
        }

        public void UpdateProprtions(object sender, EventArgs e, GraphicsDeviceManager _graphics)
        {
            codeTextBar.UpdateEditorProportions(_graphics);
            playground.UpdateProportions(_graphics.GraphicsDevice);
            missionInfo.UpdateProportions(_graphics.GraphicsDevice);

            transistion.ResizeRenderTarget(_graphics.GraphicsDevice);
            Debug.WriteLine("test");
        }
        public void ProcessTextInput(object sender, TextInputEventArgs e)
        {
            Debug.WriteLine(Convert.ToChar(e.Character));
            if (e.Key.ToString() == "Enter")
            {
                codeTextBar.typeText('\r');
            }
            else
            {
                codeTextBar.typeText(e.Character);
            }
        }

        public void LoadContent(ContentManager Content ,GraphicsDeviceManager _graphics)
        {
            font = Content.Load<SpriteFont>("font");
            smallerFont = Content.Load<SpriteFont>("smallerFont");
            background = new Color(50, 41, 47);
            orange = new Color(255, 165, 0);
            backgroundTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new[] { background });

            codeTextBar = new codeInput(font);

            levelCompleteTypewriter = new LevelCompleteTypewriter();
            MissionHandler.FormatMissionText();

            playground.LoadContent(Content, _graphics.GraphicsDevice);
            transistion.LoadContent(Content, _graphics.GraphicsDevice);
            codeTextBar.LoadContent(Content, _graphics.GraphicsDevice);
            missionInfo.LoadContet(Content);
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager _graphics)
        {

            // TODO: Add your update logic here
            codeTextBar.Update(gameTime, _graphics);
            playground.Update(gameTime);
            transistion.Update(gameTime, _graphics.GraphicsDevice);
            missionInfo.Update(gameTime, _graphics.GraphicsDevice);
            if (LevelCompleteTypewriter.play)
            {
                levelCompleteTypewriter.Update(gameTime);
            }

            currentMouseState = Mouse.GetState();
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            _graphics.GraphicsDevice.Clear(background);
            _spriteBatch.Begin();
            if (MissionHandler.World == 2)
            {
                playground.Draw(_spriteBatch, gameTime, _graphics);
            }
            if (CircleScreenTransistion.playTransistion || CircleScreenTransistion.keepScreen)
            {
                transistion.DrawTransistionRenderTarget(_spriteBatch, gameTime, _graphics.GraphicsDevice);
            }

            _graphics.GraphicsDevice.Clear(background);

            missionInfo.Draw(_spriteBatch, gameTime, _graphics);

            if (MissionHandler.World == 2)
            {
                playground.Draw2(_spriteBatch, gameTime, _graphics);
            }
            else
            {
                playground.DrawBoard(_spriteBatch, gameTime, _graphics);
            }
            if (LevelCompleteTypewriter.play)
            {
                levelCompleteTypewriter.Draw(_spriteBatch, gameTime, _graphics.GraphicsDevice);
            }

            codeTextBar.Draw(_spriteBatch, gameTime, _graphics);

            if (CircleScreenTransistion.playTransistion || CircleScreenTransistion.keepScreen)
            {
                transistion.Draw(_spriteBatch, _graphics.GraphicsDevice);
            }
            _spriteBatch.End();
        }
    }
}