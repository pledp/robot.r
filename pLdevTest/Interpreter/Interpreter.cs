using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

DEMO: 
    foo = 50
    dev = sqrt(sqrt(50))
*/

namespace pLdevTest
{
    public class Interpreter
    {
        private static List<string> lines;

        // Built in functions, need to list built in functions in ExpressionElements.cs aswell.
        public static readonly string[] builtInFunctions =
        {
            "sqrt",
            "sin",
            "tan",
            "cos"
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
            string[] segments = lines[lineIndex].Split(" ");
            if (segments.Length > 1)
            {
                if (segments[1] == "=")
                {
                    HandleAssignment(lineIndex);
                }

                if (segments[0] == "if" || segments[0] == "elseif" || segments[0] == "else")
                {
                    HandleCondition(lineIndex, stopIndex);
                }
            }
            if (lineIndex + 1 < stopIndex && lineIndex + 1 < lines.Count)
            {
                RunLines(lines, lineIndex + 1, stopIndex);
            }
        }

        private static void HandleCondition(int lineIndex, int stopIndex)
        {
            Debug.WriteLine(lineIndex + " " + stopIndex);
        }
        private static void HandleAssignment(int lineIndex)
        {
            string line = lines[lineIndex];
            string varName = line.Split(" = ")[0];
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
    }
}
