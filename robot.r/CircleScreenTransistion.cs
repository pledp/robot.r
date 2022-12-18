using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    public class CircleScreenTransistion
    {

        private Texture2D transistionMaskTexture;
        private Texture2D transistionTexture;
        Rectangle transistionPos;

        double elapsedTime;
        double desiredDuration = 2.5f;
        Vector2 startingSize;

        Vector2 openingTransistion;

        bool transistionComplete = true;
        public static Color transistionColor;
        public static bool playTransistion = false;
        public static bool keepScreen = false;

        public CircleScreenTransistion(GraphicsDevice _graphics)
        {
            transistionTexture = new Texture2D(_graphics, 1, 1);
            transistionTexture.SetData(new[] { Color.White });

            transistionPos = new Rectangle(_graphics.Viewport.Width / 2 - 100, _graphics.Viewport.Height / 2 - 100, 200, 200);
            openingTransistion = new Vector2(_graphics.Viewport.Width + _graphics.Viewport.Height, _graphics.Viewport.Width + _graphics.Viewport.Height);
            startingSize = openingTransistion;

            transistionColor = new Color(153, 243, 159);
        }
        public void ResizeRenderTarget(GraphicsDevice _graphics)
        {
            openingTransistion = new Vector2(_graphics.Viewport.Width + _graphics.Viewport.Height, _graphics.Viewport.Width + _graphics.Viewport.Height);
            if(transistionComplete)
            {
                transistionPos.Width = (int)openingTransistion.X;
                transistionPos.Height = (int)openingTransistion.Y;

                startingSize = openingTransistion;
            }
        }

        public void LoadContent(ContentManager Content, GraphicsDevice _graphics)
        {
            transistionMaskTexture = Content.Load<Texture2D>("transistionCircle");
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

        public void Draw(SpriteBatch _spriteBatch, GraphicsDevice _graphics)
        {
            _spriteBatch.Draw(transistionMaskTexture, transistionPos, Color.White);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, 0, _graphics.Viewport.Width, transistionPos.Y), transistionColor);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, transistionPos.Y + transistionPos.Height, _graphics.Viewport.Width, _graphics.Viewport.Width - (transistionPos.Y + transistionPos.Height)), transistionColor);
            _spriteBatch.Draw(transistionTexture, new Rectangle(0, transistionPos.Y, transistionPos.X, transistionPos.Height), transistionColor);
            _spriteBatch.Draw(transistionTexture, new Rectangle(transistionPos.X + transistionPos.Width, transistionPos.Y, _graphics.Viewport.Width - (transistionPos.X + transistionPos.Width), transistionPos.Height), transistionColor);
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
