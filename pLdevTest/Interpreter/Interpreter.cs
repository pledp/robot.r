using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/* 
Semi-scuffed "Programming language" I've made for a internship project, I've named it pLang. The language does not currently have any error handling.
The language works with the interpreter below. Written in C# with XNA (MonoGame).

Indentation in the programming language does not matter whatsoever.


INCLUDES (WORK IN PROGRESS):

- Memory 
    All variables are technically doubles (floats). But all variables can be used as integers also.
    All variables are stored in a dictionary of <strings as keys, doubles as values>.
    A variable can be set to another variable, setting a variable to another variable will return the other variables value.

- Built in functions
    - Square root, sqrt(arguments)
    - Sin, sin(arguments)
    - Cos, cos(arguments)
    - Tan, tan(arguments)

- Built in methods
    - print (arguments, arguments, ...), prints things to the console, can print variables, expressions and strings at the same time (split by commas). 

- Expressions
    Expressions within and outside parentheses are split with spaces. Outside parentheses expressions might work without a space, but not inside.
    Recursion within arguments works. For example: foo = sqrt(sqrt(sqrt(20))) is a valid argument.

- Conditional statements
    Conditional statements check arguments within parentheses and returns true or false, according to the arguments. If true, runs lines within the curly brackets { }.
    - if (arguments)
    - elseif (arguments), only runs conditional argument if all other "if" and "elseif" statements were false.
    - else (arguments), only runs if all other "if" and "elseif" were false.

    - Logical operators
        - ||, the OR operator checks if 2 or more arguments are true, if ANY of them are true, return true for the entire segment.
            DEMO:
                if (5 == 1 || 6 == 6) {
                    print ("Hello, World!")
                }

        - &&, the AND operator splits an statement into 2 "segments", then checks if BOTH of the "segments" return true. These "segments" can contain OR operators as in the DEMO below.
            DEMO:
                if (5 == 5 && 6 == 7 || 1 == 1) {
                    print ("Hello, World!")
                }
    
- Operators
    - == (Equals)
    - != (Does not equal)
    - < (Smaller than)
    - > (Bigger than)

- Built in variables (Sort of classes) 
    - Built in variables have a GET and SET property (They can be assigned and expressed). Currently they can only be SET to integers. They are callable by naming the class, then the variable, split by dots.
    
    - robot, controls the robot (player) on the playground.
        - x, x cordinate of the robot.
        - y, y cordinate of the robot.

    DEMO: 
        robot.x = 5
        test = robot.y

DEMO: 
    foo = 50
    dev = sqrt(sqrt(50))
    if (foo != 50) {
        x = dev
    }
    elseif (foo == 50) {
        x = dev * 5
    }
*/

namespace pLdevTest
{
    public class Interpreter
    {
        private static List<string> lines;
        private static bool ifConclusion;
        private static bool ifStarted;

        // Built in functions, need to list built in functions in ExpressionElements.cs aswell.
        public static readonly string[] builtInFunctions =
        {
            "sqrt",
            "sin",
            "tan",
            "cos"
        };

        public static readonly string[] buildInMethods =
        {
            "print"
        };

        public static Dictionary<string, Dictionary<string, int>> builtInVariables;

        public static readonly string[] operators =
        {
            "== ",
            "!= ",
            "< ",
            "> "
        };
        public static readonly string[] initialSplit =
        {
            "(",
            " "
        };

        public static Dictionary<string, double> variables;
        public static Dictionary<string, int> robot;
        public static List<string> consoleText;

        public static void StartInterprete(List<string> typedLines, int lineIndex, int stopIndex)
        {
            variables = new Dictionary<string, double>();
            Game1.playground.player.playerY = 0;
            Game1.playground.player.playerX = 0;

            // Create a dictionary of dictionaries for built in static classes. Key of the dictionary holds dictionaries of the built in variables for the classes.
            builtInVariables = new Dictionary<string, Dictionary<string, int>>();

            robot = new Dictionary<string, int>();
            robot.Add("x", 0);
            robot.Add("y", 0);
            builtInVariables.Add("robot", robot);

            consoleText = new List<string>();
            lines = typedLines;

            RunLines(lines, lineIndex, stopIndex);

            MissionHandler.CheckForMission();
        }
        private static void RunLines(List<string> lines, int lineIndex, int stopIndex)
        {
            // Interprate every line, split segment by spaces.
            string[] segments = lines[lineIndex].Split(initialSplit, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 1)
            {
                if (segments[1] == "=")
                {
                    HandleAssignment(lineIndex, segments[0]);
                }

                if (segments[0] == "if" || segments[0] == "elseif" || segments[0] == "else")
                {
                    HandleConditionStatement(lineIndex, stopIndex, segments[0]);
                    return;
                }

                if (segments[0] == "loop")
                {
                    HandleLoop(lineIndex, stopIndex, segments[0]);
                    return;
                }

                if (ArrayContainsString(buildInMethods, segments[0]))
                {
                    HandleBuiltInMethod(lineIndex, stopIndex, segments[0]);
                }
            }
            if (lineIndex + 1 < stopIndex && lineIndex + 1 < lines.Count)
            {
                RunLines(lines, lineIndex + 1, stopIndex);
            }
        }

        private static void HandleBuiltInMethod(int lineIndex, int stopIndex, string version)
        {
            HandleMethod.RunMethod(version, GetInsideParentheses(lines[lineIndex]));
        }

        private static void HandleLoop(int lineIndex, int stopIndex, string version)
        {
            string currLine = GetInsideParentheses(lines[lineIndex]);

            double loops = HandleExpression.GetResults(currLine, variables);
            int bracketEnd = FindBracket(lineIndex);
            for(int i = 0; i < loops; i++)
            {
                RunLines(lines, lineIndex + 1, bracketEnd);
            }

            RunLines(lines, bracketEnd, stopIndex);
        }
        // Handles conditional statements.
        private static void HandleConditionStatement(int lineIndex, int stopIndex, string version)
        {
            bool ifCondition = false;

            // If "if" and other "elseif" conditions were false, run "elseif" condition
            if (version == "elseif" && !ifConclusion && ifStarted)
            {
                ifCondition = HandleCondition.GetResults(lineIndex, stopIndex, lines);
            }

            // If all "if" and "elseif" conditions were false, run "else" condition
            else if(version == "else" && !ifConclusion && ifStarted)
            {
                ifCondition = true;
            } 
            else if(version == "if")
            {
                ifCondition = HandleCondition.GetResults(lineIndex, stopIndex, lines);
                ifConclusion = false;
                ifStarted = true;
            }

            if (ifCondition)
            {
                // If condition was true: Run next line, and DON'T run next "elseif" and "else" conditions
                ifConclusion = true;

                if(MissionHandler.Mission == 3)
                {
                    MissionHandler.MissionsComplete[3] = true;
                }

                RunLines(lines, lineIndex +1, stopIndex);
            } else
            {
                // If condition was false: Find closing brackets and read line after closing brackets.
                int falseStartIndex = FindBracket(lineIndex);
                RunLines(lines, falseStartIndex, stopIndex);
            }
        }

        private static void HandleAssignment(int lineIndex, string varName)
        {
            bool builtInVariableComplete = false;
            string line = lines[lineIndex];
            string key = line.Split(" = ")[0];
            string expression = line.Split(" = ")[1];

            // Handle expressions for numeric variables
            double value = HandleExpression.GetResults(expression, variables);

            if (key.Contains("."))
            {
                builtInVariableComplete = HandleBuiltInVariables.GetResults(key, value);
            }

            // If variable does not exist, make new variable, otherwise update existing variable value.
            if (!builtInVariableComplete)
            {
                if (!variables.ContainsKey(varName))
                {
                    variables.Add(varName, 0);
                }
                variables[varName] = value;
                Debug.WriteLine(variables[varName]);

                if(MissionHandler.Mission == 2)
                {
                    MissionHandler.MissionsComplete[2] = true;
                }
            }
        }

        // Check if string[] and string have any matches. Generally used for built in functions.
        public static bool ArrayContainsString(string[] array, string s)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == s)
                {
                    return true;
                }
            }
            return false;
        }
        public static string GetInsideParentheses(string s)
        {
            string newString = s.Substring(s.IndexOf("(") + 1);
            newString = newString.Substring(0, newString.LastIndexOf(")"));

            return newString;
        }

        private static int FindBracket(int startIndex)
        {
            int bracketEndIndex = startIndex;

            bool insideBracket = false;
            int nestedBracket = 0;
            for(int x = startIndex; x < lines.Count; x++)
            {
                if (lines[x].Contains('{') || insideBracket)
                {
                    if (lines[x].Contains('}'))
                    {
                        nestedBracket--;
                        if (nestedBracket == 0)
                        {
                            insideBracket = false;
                            bracketEndIndex = x;
                            return bracketEndIndex;
                        }
                    }
                    else if (lines[x].Contains('{'))
                    {
                        nestedBracket++;
                        insideBracket = true;
                    }
                }
            }
            return startIndex;
        }
    }
}
