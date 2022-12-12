using Microsoft.Xna.Framework;
using StbImageSharp;
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

        public static int Coins;
        public static int AmountOfCoins;

        private static int mission = 8;
        public static int CurrWorldMission = 1;
        public static int World = 0;
        public static int[] WorldMissionCount =
        {
            9,
            2
        };

        public static int Mission
        {
            get { return mission; }
        }

        public static bool MissionComplete;

        private readonly static string[] missions =
        {
            "Up and Down!",
            "Left and Right!",
            "Declaration of ?",
            "If only...",
            "Only if else...",
            "Repetetitition",
            "If only... (Ft. Loops)",
            "Delay",
            "Goal",
            "Pyramid Scheme",
            "Placeholder"
        };
        public static string[] Missions
        {
            get { return missions; }
        }
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
                    "while(robot.y != 10) {\n   robot.y = robot.y + 1\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Use the sleep() function.",
                    "The sleep() function adds a delay. ",
                    "You can use it in loops to slow things down and see more clearly whats going on.",
                    "loop(10) {\n   sleep(100)\n   robot.x = robot.x + 1\n   robot.y = robot.y + 1\n}"

                }
            },
            {
                new[]
                {
                    "PASS: Reach the flag by any means.",
                }
            },
            {
                new[]
                {
                    "PASS: Collect all the gems.",
                }
            },
            {
                new[]
                {
                    "PLACEHGOLDER",
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
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Color.Black,
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    GlobalThings.orangeColor,
                    GlobalThings.orangeColor,
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Color.Black,
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Color.Black,
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                    Color.Black,
                    Color.Black,
                    GlobalThings.orangeColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
                }
            }
        };

        public static void CheckForMission()
        {
            Debug.WriteLine("missioncheck");
            bool missionComplete = false;

            switch(mission)
            {
                case 8:
                    if(GameScene.playground.player.playerX == GameScene.playground.finishFlag.flagX && GameScene.playground.player.playerY == GameScene.playground.finishFlag.flagY)
                    {
                        missionComplete = true;
                        WorldTransistion();
                    }
                    break;
                case 9:
                    if (AmountOfCoins == Coins)
                    {
                        missionComplete = true;
                    }
                    break;
            }

            if (MissionComplete)
            {
                missionComplete = true;
            }

            if(missionComplete)
            {
                mission++;
                MissionComplete = false;
                missionComplete = false;
                CurrWorldMission++;
                FormatMissionText();
                switch (mission)
                {
                    case 9:
                        GameScene.playground.CreateGems(9);
                        break;
                }
            }
        }
        public static void FormatMissionText()
        {
            formattedStrings = new string[MissionsInfoText[mission, 0].Length];

            for (int i = 0; i < MissionsInfoText[mission, 0].Length; i++)
            {
                if (MissionsInfoColor[mission, 0][i] == GlobalThings.orangeColor)
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
                        int stringWidth = (int)GlobalThings.smallerFont.MeasureString(splitBySpaces[y]).X;
                        lineWidth = lineWidth + stringWidth;
                        if (lineWidth > 440)
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
        private static async void WorldTransistion()
        {
            LevelCompleteTypewriter.play = true;
            await Task.Delay(3000);
            CircleScreenTransistion.keepScreen = true;
            CircleScreenTransistion.playTransistion = true;

            await Task.Delay(5000);
            LevelCompleteTypewriter.play = false;
            World++;
            CurrWorldMission = 1;
            CircleScreenTransistion.playTransistion = true;

        }
    }
}
