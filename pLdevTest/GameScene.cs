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

        LevelCompleteTypewriter levelCompleteTypewriter;

        public GameScene(GraphicsDevice _graphics)
        {
            playground = new PlayGround(_graphics, 550);
            missionInfo = new MissionInfo(_graphics);

            background = new Color(50, 41, 47);
            backgroundTexture = new Texture2D(_graphics, 1, 1);
            backgroundTexture.SetData(new[] { background });

            codeTextBar = new codeInput();

            levelCompleteTypewriter = new LevelCompleteTypewriter();
            MissionHandler.FormatMissionText();
        }

        public void UpdateProprtions(object sender, EventArgs e, GraphicsDeviceManager _graphics)
        {
            codeTextBar.UpdateEditorProportions(_graphics);
            playground.UpdateProportions(_graphics.GraphicsDevice);
            missionInfo.UpdateProportions(_graphics.GraphicsDevice);

            Debug.WriteLine("propupdate");
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

        public void LoadContent(ContentManager Content ,GraphicsDevice _graphics)
        {
            playground.LoadContent(Content, _graphics);
            codeTextBar.LoadContent(Content, _graphics);
            missionInfo.LoadContet(Content);
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager _graphics)
        {

            // TODO: Add your update logic here
            codeTextBar.Update(gameTime, _graphics);
            playground.Update(gameTime);
            missionInfo.Update(gameTime, _graphics.GraphicsDevice);
            if (LevelCompleteTypewriter.play)
            {
                levelCompleteTypewriter.Update(gameTime);
            }

            currentMouseState = Mouse.GetState();
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            if (MissionHandler.World == 2)
            {
                playground.Draw(_spriteBatch, gameTime, _graphics);
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

        }
    }
}