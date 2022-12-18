using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    static class LanguageHandler
    {
        // 0 == English, 1 == Finnish
        public static int language = 0;

        public static string[][][] HelpBarWidgets =
        {
            new[]
            {
                new[]
                {
                    "Variables",
                    "Variables are values that may be called or changed at any time."
                },
                new[]
                {
                    "Variaabelit",
                    "Variaabelit ovat arvoja, joita voi kutsua tai muuttaa milloin tahansa."
                },
            },
            new[]
            {
                new[]
                {
                    "Conditions",
                    "Conditional statements check if a certain condition is met, and only runs the lines within it, if the condition is met."
                },
                new[]
                {
                    "Ehtolauseet",
                    "Ehtolauseet tarkistavat jos tietty ehto täyttyy, ja vetää linjat sen sisällä vain jos ehto täyttyy."
                }
            },
            new[]
            {
                new[]
                {
                    "Operators",
                    "== - Equals.\n!= - Does not equal.\n< - Less Than.\n> - More than."
                },
                new[]
                {
                    "Operattorit",
                    "== - Yhtä suuri.\n!= - Ei yhtä suuri.\n< - Pienempi kuin.\n> - Suurempi kuin."
                }
            },
            new[]
            {
                new[]
                {
                    "Logical Operators",
                    "&& - And.\n|| - Or."
                },
                new[]
                {
                    "Loogiset operaattorit",
                    "&& - Ja.\n|| - Tai."
                }
            },
            new[]
            {
                new[]
                {
                    "Loops",
                    "Loops repeat lines several times."
                },
                new[]
                {
                    "Silmukat",
                    "Silmukat toistavat linjoja useita kertoja."
                }
            },
            new[]
            {
                new[]
                {
                    "While loops",
                    "While loops repeat lines several times until a conditional statement is false."
                },
                new[]
                {
                    "While-silmukka",
                    "While-silmukat toistavat linjoja useita kertoja kunnes ehtolauseen ehto ei täyty."
                }
            },
            new[]
            {
                new[]
                {
                    "Update()",
                    "The Update() method runs all the lines within it 100 times a second, until the program is ended."
                },
                new[]
                {
                    "Update()",
                    "Update() metoodi vetää linjat sen sisällä 100 kertaa sekunnissa, kunnes ohjelma loppuu."
                }
            },
            new[]
            {
                new[]
                {
                    "Built-in Functions",
                    "sqrt(arguments)\ntan(arguments)\ncos(arguments)\nsin(arguments)"
                },
                new[]
                {
                    "Sisäänrakennetut toiminnot",
                    "sqrt()\ntan()\ncos()\nsin()"
                }
            },
            new[]
            {
                new[]
                {
                    "Built-in Methods",
                    "print(arguments) - Prints to the console.\nsleep(arguments) - Adds a delay.\nshoot() - Makes the robot shoot a projectile."
                },
                new[]
                {
                    "Sisäänrakennetut metoodit",
                    "print() - Tulostaa konsoliin.\nsleep() - Lisää viiveen.\nshoot() - Robootti ampuu ammuksen."
                }
            },
            new[]
            {
                new[]
                {
                    "robot",
                    "robot.x - The robot's horizontal posistion.\nrobot.y - The robot's vertical posistion.",
                },
                new[]
                {
                    "robot",
                    "robot.x - Robootin sijainti vaakasuorassa.\nrobot.y Robootin sijainti pystysuorassa."
                }
            },
            new[]
            {
                new[]
                {
                    "gem",
                    "gem[index].x - The gem's horizontal posistion.\ngem[index].y - The gem's vertical posistion.",
                },
                new[]
                {
                    "gem",
                    "gem[index].x - Jalokiven sijainti vaakasuorassa.\ngem[index].y - Jalokiven sijainti pystysuorassa."
                }
            },
            new[]
            {
                new[]
                {
                    "enemy",
                    "enemy[index].x - The enemy's horizontal posistion.\nenemy[index].y - The enemy's vertical posistion.",
                },
                new[]
                {
                    "enemy",
                    "enemy[index].x - Vastustajan sijainti vaakasuorassa.\nenemy[index].y - Vastustajan sijainti pystysuorassa."
                }
            },
            new[]
            {
                new[]
                {
                    "colorBlock",
                    "colorBlock[index].x - The block's horizontal posistion.\ncolorBlock[index].y - The block's vertical posistion.\ncolorBlock[index].color - The block's color.",
                },
                new[]
                {
                    "colorBlock",
                    "colorBlock[index].x - Kuution sijainti vaakasuorassa.\ncolorBlock[index].y - Kuution sijainti pystysuorassa.\ncolorBlock[index].color - Kuution väri."
                }
            },
        };

        public static string[] BackToMenuText =
        {
            "Press ESC to return to menu.",
            "Paina ESC palataksesi valikkoon."
        };

        public static string[][] GameEndString =
        {
            new[]{
                "The Aliens are defeated!",
                "Only thanks to YOU, the Hero this world truly needs!",
                "PLAYTIME: "
            },
            new[]{
                "Avaruusolennot on lyöty!",
                "Kiitokset TEILLE, tämän maailman todellakin tarvitsema sankari!",
                "PELIAIKA: "
            }
        };
    }
}
