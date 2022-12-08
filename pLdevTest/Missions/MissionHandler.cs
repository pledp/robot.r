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
            "Declaration of ?",
            "If only...",
            "Only if else...",
            "Repetetitition",
            "If only... (Ft. Loops)",
            "Goal"
        };
        public readonly static String[,][] MissionsInfoText =
        {
            {
                new[]
                {
                    "PASS: Move the robot vertically.",
                    "You can Move The Robot vertically with:",
                    "robot.y = 3"

                }
            },
            {
                new[]
                {
                    "PASS: Move the robot horizontally.",
                    "Additionally, you can also Move The Robot horizontally with:",
                    "robot.x = 4"

                }
            },
            {
                new[]
                {
                    "PASS: Declare a variable.",
                    "You can Declare a Variable like this:",
                    "helloWorld = 5"

                }
            },
            {
                new[]
                {
                    "PASS: Use a conditional statement.",
                    "Conditional statements execute the lines within the curly brackets ONLY if a certain condition is met.",
                    "You can use a Conditional Statement like this:",
                    "if(helloWorld == 5) {\n   print(\"hello world!\")\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Use any secondary conditional statement.",
                    "Conditional statements also have additional functionality:",
                    "if(helloWorld != 2 + 2 + 1) {\n   print(\"hello world!\")\n}",
                    "elseif(helloWorld < 5) {\n   print(sqrt(5))\n}",
                    "else {\n   print(helloWord)\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Use a loop.",
                    "Loops repeat the lines within the curly brackets as many times as the arguments say.",
                    "You can use a Loop like this:",
                    "loop(10) {\n   robot.x = robot.x + 1\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Use a while loop.",
                    "While loops repeat lines withing the curly brackets as long as a conditional statement is true.",
                    "You can use a While-loop like this:",
                    "while(robot.x != 10) {\n   robot.x = robot.x + 1\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Reach the flag by any means.",
                }
            }
        };
        public readonly static Color[,][] MissionsInfoColor =
{
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Game1.orange,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Game1.orange,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Game1.orange,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Color.Black,
                    Game1.orange,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Game1.orange,
                    Game1.orange,
                    Game1.orange,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Color.Black,
                    Game1.orange,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Color.Black,
                    Game1.orange,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
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
            false,
            false,
            false,
            false
        };

        public static void CheckForMission()
        {
            Debug.WriteLine("missioncheck");
            bool missionComplete = false;

            switch(mission)
            {
                case 7:
                    if(Game1.playground.player.playerX == Game1.playground.finishFlag.flagX && Game1.playground.player.playerY == Game1.playground.finishFlag.flagY)
                    {
                        MissionsComplete[mission] = true;
                    }
                    break;
            }

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
            formattedStrings = new string[MissionsInfoText[mission, 0].Length];

            for (int i = 0; i < MissionsInfoText[mission, 0].Length; i++)
            {
                if (MissionsInfoColor[mission, 0][i] == Game1.orange)
                {
                    formattedStrings[i] = MissionsInfoText[mission, 0][i];
                }
                else
                {
                    int lineWidth = 0;
                    string[] splitBySpaces = MissionHandler.MissionsInfoText[MissionHandler.Mission, 0][i].Split(" ");
                    string formattedString = "";
                    for (int y = 0; y < splitBySpaces.Length; y++)
                    {
                        int stringWidth = (int)Game1.smallerFont.MeasureString(splitBySpaces[y]).X;
                        lineWidth = lineWidth + stringWidth;
                        if (lineWidth > 460)
                        {
                            formattedString = formattedString + "\n";
                            lineWidth = stringWidth;
                        }
                        formattedString = formattedString + splitBySpaces[y] + " ";
                    }
                    formattedStrings[i] = formattedString;
                }
            }
        }
    }
}
