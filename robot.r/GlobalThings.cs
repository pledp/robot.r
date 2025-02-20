﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    public static class GlobalThings
    {
        public static Color orangeColor;
        public static SpriteFont smallerFont;
        public static SpriteFont font;
        public static Texture2D whiteTexture;
        public static Texture2D gemTexture;
        public static Texture2D enemyTexture;
        public static Texture2D frameTexture;
        public static Texture2D blueTexture;
        public static Texture2D greenTexture;
        public static Color darkerGrey;

        public static float playTime;

        public static void LoadContent(ContentManager Content, GraphicsDevice _graphics)
        {
            font = Content.Load<SpriteFont>("font");
            smallerFont = Content.Load<SpriteFont>("smallerFont");
            gemTexture = Content.Load<Texture2D>("gem");

            orangeColor = new Color(255, 165, 0);
            darkerGrey = new Color(65, 65, 63);

            enemyTexture = new Texture2D(_graphics, 1, 1);
            enemyTexture.SetData(new[] { Color.Red });
            greenTexture = new Texture2D(_graphics, 1, 1);
            greenTexture.SetData(new[] { Color.Green });
            blueTexture = new Texture2D(_graphics, 1, 1);
            blueTexture.SetData(new[] { Color.Blue });

            whiteTexture = new Texture2D(_graphics, 1, 1);
            whiteTexture.SetData(new[] { Color.White });


            frameTexture = new Texture2D(_graphics, 1, 1);
            frameTexture.SetData(new[] { new Color(50, 41, 47) });
        }
        public static bool EnterArea(Rectangle area, MouseState mouseState)
        {
            if (area.Contains(mouseState.Position))
            {
                return true;
            }
            return false;
        }

        // Format string for line breaks.
        public static string FormatLineBreak(string s, int lineLength)
        {
            int lineWidth = 0;
            string[] splitBySpaces = s.Split(" ");
            string formattedString = "";
            for (int y = 0; y < splitBySpaces.Length; y++)
            {
                int stringWidth = (int)GlobalThings.smallerFont.MeasureString(splitBySpaces[y]).X;
                lineWidth = lineWidth + stringWidth;
                if (lineWidth > lineLength)
                {
                    formattedString = formattedString + "\n";
                    lineWidth = stringWidth;
                }
                formattedString = formattedString + splitBySpaces[y] + " ";
            }
            return formattedString;            
        }
    }
}
