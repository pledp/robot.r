using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public static class MissionHandler
    {
        public static string[] formattedStrings;
        public static int lineBreaks;

        private static int mission = 0;
        public static int Mission
        {
            get { return mission; }
        }

        private readonly static string[] missions =
        {
            "Up and Down!",
            "Left and Right!",
            "Declaration of...",
            "If only...",
            "Else:",
            "Only if else..."
        };
        public readonly static String[,][] MissionsInfoText =
        {
            {
                new[]
                {
                    "You can move the robot vertically with:",
                    "robot.y = 3"
                }
            },
            {
                new[]
                {
                    "Additionally, you can also move the robot horizontally with:",
                    "robot.x = 4"
                }
            },
            {
                new[]
                {
                    "You can declare a variable like this:",
                    "helloWorld = 5"
                }
            }
        };
        public readonly static Color[,][] MissionsInfoColor =
{
            {
                new[]
                {
                    Color.Black,
                    PlayGround.pgColor
                }
            },
            {
                new[]
                {
                    Color.Black,
                    PlayGround.pgColor
                }
            },
            {
                new[]
                {
                    Color.Black,
                    PlayGround.pgColor
                }
            }
        };

        public static string[] Missions
        {
            get { return missions; }
        }

        public static bool[] MissionsComplete =
        {
            false,
            false,
            false,
            false,
        };

        public static void CheckForMission()
        {
            bool missionComplete = false;
            if (MissionsComplete[mission])
            {
                missionComplete = true;
            }

            if(missionComplete)
            {
                mission++;
                FormatMissionText();
            }
        }
        public static void FormatMissionText()
        {
            lineBreaks = 0;
            formattedStrings = new string[MissionsInfoText[mission, 0].Length];
            for (int i = 0; i < MissionsInfoText[mission, 0].Length; i++)
            {
                int lineWidth = 0;
                string[] splitBySpaces = MissionHandler.MissionsInfoText[MissionHandler.Mission, 0][i].Split(" ");
                string formattedString = "";
                for (int y = 0; y < splitBySpaces.Length; y++)
                {
                    int stringWidth = (int)Game1.smallerFont.MeasureString(splitBySpaces[y]).X;
                    lineWidth = lineWidth + stringWidth;
                    if (lineWidth > 500)
                    {
                        formattedString = formattedString + "\n";
                        lineWidth = stringWidth;
                        lineBreaks++;
                    }
                    formattedString = formattedString + splitBySpaces[y] + " ";
                }
                formattedStrings[i] = formattedString;
            }
        }
    }
}
