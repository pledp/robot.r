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
        public static bool MissionFailed = false;
        public static bool MissionPlaying;
        public static double Timer;

        public static string[] formattedStrings;
        public static int lineBreaks;

        public static int Coins;
        public static int AmountOfCoins;

        public static int KilledEnemies;
        public static int AmountOfEnemies;

        public static int AmountOfColorBlocks;
        public static int SortedColorBlocks;

        private static int mission = 0;
        public static int CurrWorldMission = 1;
        public static int World = 0;
        public static int[] WorldMissionCount =
        {
            9,
            6
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
            "(Inverted) Pyramid Scheme",
            "Survival <",
            "Survival ^",
            "Covered",
            "Where?",
            "Pew Pew",
            "Sorter",
            "BOSS"
        };
        public static string[] Missions
        {
            get { return missions; }
        }

        public readonly static MissionTypes[] MissionCategory =
        {
            MissionTypes.Default,
            MissionTypes.Default,
            MissionTypes.Default,
            MissionTypes.Default,
            MissionTypes.Default,
            MissionTypes.Default,
            MissionTypes.Default,
            MissionTypes.Default,
            MissionTypes.FlagLevel,
            MissionTypes.CoinLevel,
            MissionTypes.EnemyLevel,
            MissionTypes.EnemyLevel,
            MissionTypes.CoinLevel,
            MissionTypes.FlagLevel,
            MissionTypes.KillLevel,
            MissionTypes.SortLevel,
            MissionTypes.Default,
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
                    "Variables are values that can be changed whenever.",
                    "helloWorld = 5"
                }
            },
            {
                new[]
                {
                    "PASS: Use a conditional statement.",
                    "Conditional statements execute the lines ONLY if a certain condition is met.",
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
                    "Loops repeats lines as many times as the arguments say.",
                    "loop(10) {\n   robot.x = robot.x + 1\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Use a while loop.",
                    "While loops repeat lines as long as a conditional statement is true.",
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
                    "gem[1].x = 5"
                }
            },
            {
                new[]
                {
                    "PASS: Survive.",
                    "The Update() method runs 60 times a second until the program is stopped.",
                    "x = 0\nUpdate() {\n   x = x + 1\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Survive",
                    "enemy[2].y = 5"
                }
            },
            {
                new[]
                {
                    "PASS: Collect all the gems",
                }
            },
            {
                new[]
                {
                    "PASS: Reach the flag.",
                }
            },
            {
                new[]
                {
                    "PASS: DESTROY all the enemies.",
                    "Use the shoot() method to fire a projectile.",
                    "shoot()"
                }
            },
            {
                new[]
                {
                    "PASS: Sort the colors.",
                    "Sort red to Y=1, green to Y=2, blue to Y=3",
                    "if(colorBlock[1].color == color.Red) {"
                }
            },
            {
                new[]
                {
                    "PASS: Kill the boss.",
                }
            },
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
                }
            },
            {
                new[]
                {
                    PlayGround.pgColor,
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
                }
            },
        };

        public static void CheckForMission()
        {

            Debug.WriteLine("missioncheck");
            bool missionComplete = false;

            if (MissionCategory[Mission] == MissionTypes.EnemyLevel)
            {
                if (!MissionFailed)
                {
                    missionComplete = true;
                }
            }

            if (MissionCategory[Mission] == MissionTypes.SortLevel)
            {
                if (AmountOfColorBlocks == SortedColorBlocks)
                {
                    missionComplete = true;
                }
            }

            if (MissionCategory[Mission] == MissionTypes.CoinLevel)
            {
                if (AmountOfCoins == Coins)
                {
                    missionComplete = true;
                }
            }
            if (MissionCategory[mission] == MissionTypes.KillLevel)
            {
                if(AmountOfEnemies == KilledEnemies)
                {
                    missionComplete = true;
                }
            }

            if (MissionCategory[mission] == MissionTypes.FlagLevel)
            {
                if (GameScene.playground.player.posX == GameScene.playground.finishFlag.posX && GameScene.playground.player.posY == GameScene.playground.finishFlag.posY)
                {
                    missionComplete = true;
                }
            }

            if (MissionComplete)
            {
                missionComplete = true;
            }

            if(missionComplete)
            {
                if(mission == 8)
                {
                    WorldTransistion();
                }

                mission++;

                CurrWorldMission++;
                FormatMissionText();

                GameScene.playground.CreateTiles();

                ResetMission();
            }
        }
        public static void FormatMissionText()
        {
            formattedStrings = new string[MissionsInfoText[mission, 0].Length];

            for (int i = 0; i < formattedStrings.Length; i++)
            {
                if (MissionsInfoColor[mission, 0][i] == GlobalThings.orangeColor)
                {
                    formattedStrings[i] = MissionsInfoText[mission, 0][i];
                } else
                {
                    formattedStrings[i] = GlobalThings.FormatLineBreak(MissionsInfoText[mission, 0][i], 440);

                }
            }
        }
        private static async void WorldTransistion()
        {
            World = World + 1;
            LevelCompleteTypewriter.play = true;
            await Task.Delay(3000);
            CircleScreenTransistion.keepScreen = true;
            CircleScreenTransistion.playTransistion = true;

            await Task.Delay(5000);
            LevelCompleteTypewriter.play = false;
            CurrWorldMission = 1;
            CircleScreenTransistion.playTransistion = true;

        }

        public static void ResetMission()
        {
            GameScene.playground.bullets = new List<Bullet>();
            MissionPlaying = false;
            MissionComplete = false;
            MissionFailed = false;

            PlayCodeButton.RunUpdate = false;

            AmountOfCoins = 0;
            Coins = 0;

            AmountOfEnemies = 0;
            KilledEnemies = 0;

            SortedColorBlocks = 0;
            AmountOfColorBlocks = 0;

            GameScene.playground.gems = null;
            GameScene.playground.enemies = null;
            GameScene.playground.coloredBlocks = null;

            if (MissionCategory[mission] == MissionTypes.EnemyLevel)
            {
                Timer = 15;
                GameScene.playground.CreateLevel(mission);
            }
            else if (MissionCategory[mission] == MissionTypes.CoinLevel)
            {
                GameScene.playground.CreateLevel(mission);
            }
            else if(MissionCategory[mission] == MissionTypes.FlagLevel)
            {
                GameScene.playground.CreateLevel(mission);
            }
            else if (MissionCategory[mission] == MissionTypes.KillLevel)
            {
                GameScene.playground.CreateLevel(mission);
            }
            else if (MissionCategory[mission] == MissionTypes.SortLevel)
            {
                GameScene.playground.CreateLevel(mission);
            }
        }
    }
}
