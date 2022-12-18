using Microsoft.Xna.Framework;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
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

        public static int CurrWorldMission = 1;
        public static int World = 0;
        public static int[] WorldMissionCount =
        {
            9,
            8
        };

        private static int completedMissions = 0;
        public static int CompletedMissions
        {
            set { completedMissions = value; }
            get { return completedMissions; }
        }

        private static int mission = 0;
        public static int Mission
        {
            set { mission = value; }
            get { return mission; }
        }

        public static bool MissionComplete;

        private readonly static string[,] missions =
        {
            {
                "Up and Down!",
                "Ylös ja Alas!"
            },
            {
                "Left and Right!",
                "Vasemalle ja Oikealle",
            },
            {
                "Declaration of ?",
                "?:n julistus",
            },
            {
                "If only...",
                "Jos vain...",
            },
            {
                "Only if else...",
                "Jos vain muuten...",
            },
            {
                "Repetetitition",
                "Tototoisto",
            },
            {
                "If only... (Ft. Loops)",
                "Jo vain... (Ft. Solmukat)",
            },
            {
                "Invasive delay",
                "Tunkeutuva viive",
            },
            {
                "Goals (Singular",
                "Maaleja (Yksikkö)",
            },
            {
                "(Inverted) Pyramid Scheme",
                "(Ylösalain) Pyramidihuijaus",
            },
            {
                "Groundmarch",
                "Maamarssi",
            },
            {
                "Airmarch",
                "Ilmamarssi",
            },
            {
                "Payday",
                "Palkkapäivä",
            },
            {
                "Hide-out",
                "Piilopaikka",
            },
            {
                "Pew Pew (Ft. VIOLENCE)",
                "Piu Piu (Ft. VÄKIVALTA)",
            },
            {
                "Unpaid Alien internship",
                "Palkaton työharjoittelu",
            },
            {
                "BOSS, ENDER OF WORLDS.",
                "POMO, MAAILMANLOPPU",
            },
        };
        public static string[,] Missions
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
            MissionTypes.KillLevel,
        };

        public readonly static String[,][] MissionsInfoText =
        {
            {
                new[]
                {
                    "PASS: Move the robot vertically.",
                    "You can Move The Robot vertically with:",
                    "robot.y = 3"

                },
                new[]
                {
                    "LÄPÄISY: Liikuta roboottia pystysuorassa.",
                    "Voit liikuttaa roboottia pystysuorassa komennolla:",
                    "robot.x = 3"

                }
            },
            {
                new[]
                {
                    "PASS: Move the robot horizontally.",
                    "Additionally, you can also Move The Robot horizontally with:",
                    "robot.x = 4"

                },
                new[]
                {
                    "LÄPÄISY: Liikuta roboottia vaakasuorassa.",
                    "Voit liikuttaa roboottia vaakasuorassa komennolla:",
                    "robot.y = 3"
                }
            },
            {
                new[]
                {
                    "PASS: Declare a variable.",
                    "Variables are values that can be changed whenever.",
                    "helloWorld = 5"
                },
                new[]
                {
                    "LÄPÄISY: Julkaise variaabeli.",
                    "Variaabelit ovat arvoja, joita voi kutsua tai muuttaa milloin tahansa.",
                    "helloWorld = 5"
                }
            },
            {
                new[]
                {
                    "PASS: Use a conditional statement.",
                    "Conditional statements execute the lines ONLY if a certain condition is met.",
                    "if(helloWorld == 5) {\n   print(\"hello world!\")\n}"
                },
                new[]
                {
                    "LÄPÄISY: Käytä ehtolausetta.",
                    "Ehtolauseet tarkistavat jos tietty ehto täyttyy, ja vetävät linjat sen sisällä vain jos ehto täyttyy.",
                    "if(helloWorld == 5) {\n   print(\"hello world!\")\n}"
                },

            },
            {
                new[]
                {
                    "PASS: Use any secondary conditional statement.",
                    "Conditional statements also have additional functionality:",
                    "if(helloWorld != 2 + 2 + 1) {\n   print(\"hello world!\")\n}",
                    "elseif(helloWorld < 5) {\n   print(sqrt(5))\n}",
                    "else {\n   print(helloWord)\n}"
                },
                new[]
                {
                    "LÄPÄISY: Käytä toissijaista ehtolausetta.",
                    "Ehtolauseilla on myös lisä-toiminallisuutta:",
                    "if(helloWorld != 2 + 2 + 1) {\n   print(\"hello world!\")\n}",
                    "elseif(helloWorld < 5) {\n   print(sqrt(5))\n}",
                    "else {\n   print(helloWord)\n}"
                },
            },
            {
                new[]
                {
                    "PASS: Use a loop.",
                    "Loops repeats lines several times.",
                    "loop(10) {\n   robot.x = robot.x + 1\n}"
                },
                new[]
                {
                    "LÄPÄISY: Käytä silmukkaa.",
                    "Silmukat toistavat linjoja useita kertoja.",
                    "loop(10) {\n   robot.x = robot.x + 1\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Use a while loop.",
                    "While loops repeat lines as long as a conditional statement is true.",
                    "while(robot.y != 10) {\n   robot.y = robot.y + 1\n}"
                },
                new[]
                {
                    "LÄPÄISTY: Käytä while-silmukkaa.",
                    "While-silmukat toistavat linjoja useita kertoja kunnes ehtolauseen ehto ei täyty.",
                    "while(robot.y != 10) {\n   robot.y = robot.y + 1\n}"
                }
            },
            {
                new[]
                {
                    "PASS: Use the sleep() method.",
                    "The sleep() method adds a delay. ",
                    "You can use it in loops to slow things down and see more clearly whats going on.",
                    "loop(10) {\n   sleep(100)\n   robot.x = robot.x + 1\n   robot.y = robot.y + 1\n}"

                },
                new[]
                {
                    "LÄPÄISY: Käytä sleep() metoodia.",
                    "sleep() metoodi lisää viiveen. ",
                    "Voit käyttää sitä silmukoissa hidastaakseen toimintoa, ja nähdäkseen selvemmin mitä tapahtuu.",
                    "loop(10) {\n   sleep(100)\n   robot.x = robot.x + 1\n   robot.y = robot.y + 1\n}"

                }
            },
            {
                new[]
                {
                    "PASS: Reach the flag by any means.",
                },
                new[]
                {
                    "PASS: Tavoita lipulle kaikin keinoin.",
                },
            },
            {
                new[]
                {
                    "PASS: Collect all the gems.",
                    "gem[1].x = 5"
                },
                new[]
                {
                    "LÄPÄISY: Kerää kaikki jalokivet.",
                    "gem[1].x = 5"
                }
            },
            {
                new[]
                {
                    "PASS: Survive.",
                    "The Update() method runs 100 times a second until the program is stopped.",
                    "x = 0\nUpdate() {\n   x = x + 1\n}"
                },
                 new[]
                {
                    "LÄPÄISY: Selviä.",
                    "Update() metoodii vetää linjat sen sisällä 100 kertaa sekunissa, kunnes ohjelma loppuu.",
                    "x = 0\nUpdate() {\n   x = x + 1\n}"
                },

            },
            {
                new[]
                {
                    "PASS: Survive.",
                    "enemy[2].y = 5"
                },
                new[]
                {
                    "LÄPÄISY: Selviä.",
                    "enemy[2].y = 5"
                }
            },
            {
                new[]
                {
                    "PASS: Collect all the gems",
                },
                new[]
                {
                    "LÄPÄISY: Kerää kaikki jalokivet.",
                }
            },
            {
                new[]
                {
                    "PASS: Reach the flag.",
                },
                new[]
                {
                    "PASS: Tavoita lipulle kaikin keinoin.",
                },
            },
            {
                new[]
                {
                    "PASS: DESTROY all the aliens.",
                    "Use the shoot() method to fire a projectile.",
                    "shoot()"
                },
                new[]
                {
                    "LÄPÄISY: TUHOA kaikki avaruusolennot.",
                    "Käytä shoot() metoodia ampumakseen ammuksen.",
                    "shoot()"
                }
            },
            {
                new[]
                {
                    "PASS: Sort the colors.",
                    "Sort red to Y=1, green to Y=2 and blue to Y=3.",
                    "if(colorBlock[1].color == color.Red) {"
                },
                new[]
                {
                    "LÄPÄISY: Lajittele värit.",
                    "Lajittele punainen Y=1:een, vihreä Y=2:een ja sininen Y=3:een.",
                    "if(colorBlock[1].color == color.Red) {"
                }
            },
            {
                new[]
                {
                    "PASS: DESTROY The Alien's final BOSS, JACK.",
                },
                new[]
                {
                    "LÄPÄISY: TUHOA Avaruusolentojen POMO, JORMA.",
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

            if (missionComplete)
            {
                if (mission != 16)
                {
                    if (mission == 8)
                    {
                        WorldTransistion();
                    }

                    if (mission > completedMissions)
                    {
                        completedMissions++;
                    }
                    mission++;

                    CurrWorldMission++;

                    GameScene.playground.CreateTiles();

                    ResetMission();
                } else
                {
                    int minutes = (int)GlobalThings.playTime / 60;
                    int seconds = (int)GlobalThings.playTime % 60;

                    LanguageHandler.GameEndString[0][2] = "PLAYTIME: " + minutes +"."+ seconds + "s";
                    LanguageHandler.GameEndString[1][2] = "PELIAIKA: " + minutes +"."+ seconds + "s";


                    GameScene.textBubble.typeWriterStringType = "";
                    GameScene.textBubble.currentLine = 0;
                    GameScene.textBubble.isPlaying = true;
                    GameScene.textBubble.typeWriterString = LanguageHandler.GameEndString;
                }
            }
        }
        public static void FormatMissionText()
        {
            formattedStrings = new string[MissionsInfoText[mission, LanguageHandler.language].Length];

            for (int i = 0; i < formattedStrings.Length; i++)
            {
                if (MissionsInfoColor[mission, 0][i] == GlobalThings.orangeColor)
                {
                    formattedStrings[i] = MissionsInfoText[mission, LanguageHandler.language][i];
                } else
                {
                    formattedStrings[i] = GlobalThings.FormatLineBreak(MissionsInfoText[mission, LanguageHandler.language][i], 440);

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
            FormatMissionText();

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
