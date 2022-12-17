using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
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
                    "Conditional statements check if a certain condition is met."
                },
                new[]
                {
                    "Ehtolauseet",
                    "Ehtolauseet tarkistavat jos tietty ehto täyttyy."
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
        };
    }
}
