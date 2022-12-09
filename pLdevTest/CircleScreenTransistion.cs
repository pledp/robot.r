using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public class CircleScreenTransistion
    {

        private Effect maskEffect;
        private Texture2D transistionMaskTexture;
        private Texture2D transistionTexture;
        RenderTarget2D transistionMask;
        RenderTarget2D mainTarget;
        Rectangle transistionPos;

        double elapsedTime;
        double desiredDuration = 3;
        Vector2 startingSize;

        Vector2 openingTransistion;

        bool transistionComplete;
        Color transistionColor;
        public static bool playTransistion = true;
        public static bool keepScreen = false;

        public CircleScreenTransistion(GraphicsDevice _graphics)
        {
            transistionTexture = new Texture2D(_graphics, 1, 1);
            transistionTexture.SetData(new[] { Color.White });

            transistionPos = new Rectangle(_graphics.Viewport.Width / 2 - 100, _graphics.Viewport.Height / 2 - 100, 200, 200);
            startingSize = new Vector2(transistionPos.Width, transistionPos.Height);
            openingTransistion = new Vector2(_graphics.Viewport.Width + _graphics.Viewport.Height, _graphics.Viewport.Width + _graphics.Viewport.Height);

            ResizeRenderTarget(_graphics);
            transistionColor = new Color(153, 243, 159);
        }
        public void ResizeRenderTarget(GraphicsDevice _graphics)
        {
            var pp = _graphics.PresentationParameters;
            transistionMask = new RenderTarget2D(
                _graphics, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
               _graphics, pp.BackBufferWidth, pp.BackBufferHeight);

            openingTransistion = new Vector2(_graphics.Viewport.Width + _graphics.Viewport.Height, _graphics.Viewport.Width + _graphics.Viewport.Height);
            if(transistionComplete)
            {
                transistionPos.Width = (int)openingTransistion.X;
                transistionPos.Height = (int)openingTransistion.Y;
            }
        }

        public void LoadContent(ContentManager Content, GraphicsDevice _graphics)
        {
            transistionMaskTexture = Content.Load<Texture2D>("transistionMask");
            maskEffect = Content.Load<Effect>("MaskShader");
        }

        public void Update(GameTime gameTime, GraphicsDevice _graphics)
        {
            if (playTransistion)
            {
                if (!transistionComplete)
                {
                    OpenTransistion(_graphics, openingTransistion, gameTime);
                }
                else
                {
                    OpenTransistion(_graphics, new Vector2(0, 0), gameTime);
                }
            }

            transistionPos.Y = _graphics.Viewport.Height / 2 - transistionPos.Height / 2;
            transistionPos.X = _graphics.Viewport.Width / 2 - transistionPos.Width / 2;
        }
        public void DrawTransistionRenderTarget(SpriteBatch _spriteBatch, GameTime gameTime, GraphicsDevice _graphics)
        {
            // Create mask for circle transistion
            _spriteBatch.End();
            _graphics.SetRenderTarget(transistionMask);
            _graphics.Clear(Color.Transparent);

            // Create light mask
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            _spriteBatch.Draw(transistionMaskTexture, transistionPos, Color.White);
            _spriteBatch.End();

            // Draw to render texture
            _graphics.SetRenderTarget(mainTarget);
            _graphics.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height), transistionColor);
            _spriteBatch.End();

            _graphics.SetRenderTarget(null);
            _spriteBatch.Begin();
        }

        public void Draw(SpriteBatch _spriteBatch, GraphicsDevice _graphics)
        {
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, 0, _graphics.Viewport.Width, transistionPos.Y), transistionColor);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, transistionPos.Y + transistionPos.Height, _graphics.Viewport.Width, _graphics.Viewport.Width - (transistionPos.Y + transistionPos.Height)), transistionColor);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, transistionPos.Y, transistionPos.X, transistionPos.Height), transistionColor);
            _spriteBatch.Draw(transistionTexture, new Rectangle(transistionPos.X + transistionPos.Width, transistionPos.Y, _graphics.Viewport.Width - (transistionPos.X + transistionPos.Width), transistionPos.Height), transistionColor);

            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            maskEffect.Parameters["Mask"].SetValue(transistionMask);
            maskEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(mainTarget, new Vector2(0, 0), Color.White);
        }

        public void OpenTransistion(GraphicsDevice _graphics, Vector2 newScale, GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            float percentageComplete = (float)elapsedTime / (float)desiredDuration;
            transistionPos.Width = (int)Vector2.Lerp(startingSize, newScale, MathHelper.SmoothStep(0, 1, percentageComplete)).X;
            transistionPos.Height = (int)Vector2.Lerp(startingSize, newScale, MathHelper.SmoothStep(0, 1, percentageComplete)).Y;

            if(transistionPos.Width == newScale.X)
            {
                if(!transistionComplete)
                {
                    transistionComplete = true;
                } 
                else
                {
                    transistionComplete = false;
                }
                playTransistion = false;
                elapsedTime = 0;
                startingSize = new Vector2(transistionPos.Width, transistionPos.Height);
            }
        }
    }
}
