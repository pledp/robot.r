using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pLdevTest
{
    public class PlayGround
    {
        private Rectangle playground;
        private int width;
        private Texture2D pgTexture;
        public static Color pgColor;

        Texture2D lightMask;

        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;

        Effect lightingEffect;

        Texture2D blackRectangle;

        public PlaygroundPlayer player;
        public FinishFlag finishFlag;
        public PlayGround(GraphicsDevice _graphics, int playgroundWidth)
        {
            // Create a playground
            width = playgroundWidth;
            playground = new Rectangle(_graphics.Viewport.Width - width - 50, 10, width, 400);
            
            pgTexture = new Texture2D(_graphics, 1, 1);
            pgColor = new Color(153, 225, 217);
            pgTexture.SetData(new[] { pgColor });

            // Create a player on the playground. Move in a 21x15 grid.
            player = new PlaygroundPlayer(_graphics, playground.X, playground.Y);
            finishFlag = new FinishFlag(_graphics, playground.X, playground.Y, 10,10);
            blackRectangle = new Texture2D(_graphics, 1, 1);
            blackRectangle.SetData(new[] { Color.Black });
        }
        public void LoadContent(ContentManager Content, GraphicsDevice _graphics)
        {
            lightMask = Content.Load<Texture2D>("lightmask");
            lightingEffect = Content.Load<Effect>("effect1");

            var pp = _graphics.PresentationParameters;
            lightsTarget = new RenderTarget2D(
                _graphics, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
                _graphics, pp.BackBufferWidth, pp.BackBufferHeight);

            player.LoadContent(Content);
            finishFlag.LoadContent(Content);
        }
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            finishFlag.Update(gameTime);
        }

        public void DrawBoard(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(pgTexture, playground, pgColor);
            finishFlag.Draw(_spriteBatch, gameTime, _graphics);
            player.Draw(_spriteBatch, gameTime, _graphics);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            spriteBatch.End();
            _graphics.GraphicsDevice.SetRenderTarget(lightsTarget);
            _graphics.GraphicsDevice.Clear(Color.Transparent);

            // Create light mask
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Vector2 lightSize = new Vector2(1000, 1000);
            spriteBatch.Draw(lightMask, new Rectangle(playground.X + (player.playerX * 25) - ((int)lightSize.X/2) + 12, playground.Y + (player.playerY * 25) - ((int)lightSize.Y / 2) + 6, (int)lightSize.X, (int)lightSize.Y),  Color.White);
            spriteBatch.End();

            // Draw to render texture
            _graphics.GraphicsDevice.SetRenderTarget(mainTarget);
            _graphics.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            DrawBoard(spriteBatch, gameTime, _graphics);
            spriteBatch.End();

            _graphics.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();

        }
        public void Draw2(SpriteBatch spriteBatch, GameTime gameTime, GraphicsDeviceManager _graphics)
        {
            spriteBatch.Draw(blackRectangle, playground, Color.Black);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            lightingEffect.Parameters["lightMask"].SetValue(lightsTarget);
            lightingEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainTarget, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public void UpdateProportions(GraphicsDevice _graphics)
        {
            playground.X = _graphics.Viewport.Width - width - 50;
            player.UpdateProportions(_graphics, playground.X);
            finishFlag.UpdateProportions(_graphics, playground.X);

            lightsTarget = new RenderTarget2D(
                _graphics,_graphics.Viewport.Width, _graphics.Viewport.Height);
            mainTarget = new RenderTarget2D(
                _graphics, _graphics.Viewport.Width, _graphics.Viewport.Height);
        }
    }
}
