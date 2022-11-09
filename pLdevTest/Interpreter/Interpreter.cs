using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pLdevTest
{
    public class Interpreter
    {
        public static readonly string[] builtInFunctions =
        {
            "sqrt",
            "sin",
            "tan",
            "cos"
        };

        public static Dictionary<string, double> variables;
        public static void RunLines(List<string> lines)
        {
            variables = new Dictionary<string, double>();
            foreach (string line in lines)
            {
                // Interprate every line, split line by spaces.
                string[] segments = line.Split(" ");
                if (segments.Length > 2)
                {
                    if (segments[1] == "=")
                    {
                        HandleAssignment(line);
                    }
                }
            }  
        }

        private static void HandleAssignment(string line)
        {
            string varName = line.Split(" = ")[0];
            string expression = line.Split(" = ")[1];

            double value = HandleExpression.GetResults(expression, variables);
            
            if (!variables.ContainsKey(varName))
            {
                variables.Add(varName, 0);
            }
            variables[varName] = value;
            Debug.WriteLine(variables[varName]);
        }

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
