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
Semi-scuffed "Programming language" I've made for a internship project, I've named it pLang. The language does not currently have any error handling, but will come eventually.
The language works with the interpreter below. Written in C# with XNA (MonoGame).

INCLUDES (WORK IN PROGRESS):

- Memory 
    All variables are technically doubles (floats). But all variables can be used as integers also.
    All variables are stored in a dictionary of <strings as keys, doubles as values>.
    A variable can be set to another variable, setting a variable to another variable will return the other variables value.

- Built in functions
    - Square root
    - Sin
    - Cos
    - Tan

- Expressions
    Expressions within and outside parentheses are split with spaces. Outside parentheses expressions might work without a space, but not inside.
    RECURSION within arguments works. For example: foo = sqrt(sqrt(sqrt(20))) is a valid argument.

- Conditional statements
    if (arguments), elseif (arguments), else (arguments) is a work in progress, currently simple version functional. 
    
- Operators
    - == (Equals)
    - != (Does not equal)
    - < (Smaller than)
    - > (Bigger than)

DEMO: 
    foo = 50
    dev = sqrt(sqrt(50))
*/

namespace pLdevTest
{
    public class Interpreter
    {
        private static List<string> lines;
        private static bool ifConclusion;

        // Built in functions, need to list built in functions in ExpressionElements.cs aswell.
        public static readonly string[] builtInFunctions =
        {
            "sqrt",
            "sin",
            "tan",
            "cos"
        };

        public static readonly string[] operators =
        {
            "== ",
            "!= ",
            "< ",
            "> "
        };

        public static Dictionary<string, double> variables;

        public static void StartInterprete(List<string> typedLines, int lineIndex, int stopIndex)
        {
            variables = new Dictionary<string, double>();
            lines = typedLines;

            RunLines(lines, lineIndex, stopIndex);
        }
        private static void RunLines(List<string> lines, int lineIndex, int stopIndex)
        {
            // Interprate every line, split segment by spaces.
            string[] segments = lines[lineIndex].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 1)
            {
                if (segments[1] == "=")
                {
                    HandleAssignment(lineIndex, segments[0]);
                }

                if (segments[0] == "if" || segments[0] == "elseif" || segments[0] == "else")
                {
                    HandleConditionStatement(lineIndex, stopIndex);
                    return;
                }
            }
            if (lineIndex + 1 < stopIndex && lineIndex + 1 < lines.Count)
            {
                RunLines(lines, lineIndex + 1, stopIndex);
            }
        }

        // Handles conditional statements.
        private static void HandleConditionStatement(int lineIndex, int stopIndex)
        {

            /* TODO
             * Add && and || support
             * Make else () and elseif () properly functional
            */

            bool ifCondition = HandleCondition.ConditionHandler(lineIndex, stopIndex, lines);
            if (ifCondition)
            {
                Debug.WriteLine("TRUE");
                RunLines(lines, lineIndex +1, stopIndex);
            } else
            {
                // Find where brackets end.
                Debug.WriteLine("test");
                int falseStartIndex = FindBracket(lineIndex);
                RunLines(lines, falseStartIndex, stopIndex);
            }
            

        }
        private static void HandleAssignment(int lineIndex, string varName)
        {
            string line = lines[lineIndex];
            string expression = line.Split(" = ")[1];

            // Handle expressions for numeric variables
            double value = HandleExpression.GetResults(expression, variables);
            
            // If variable does not exist, make new variable, otherwise update existing variable value.
            if (!variables.ContainsKey(varName))
            {
                variables.Add(varName, 0);
            }
            variables[varName] = value;
            Debug.WriteLine(variables[varName]);
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
