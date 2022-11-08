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
    public static class Interpreter
    {
        public static Dictionary<string, double> variables = new Dictionary<string, double>();
        public static void RunLines(List<String> lines)
        {
            foreach(string line in lines)
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
            foreach (var variable in variables)
            {
                Debug.WriteLine(variable);
            }

        }

        private static void HandleAssignment(string line)
        {
            string varName = line.Split("=")[0];
            string expression = line.Split("=")[1];

            DataTable dt = new DataTable();
            double value = Convert.ToDouble(dt.Compute(expression, ""));
            if (!variables.ContainsKey(varName))
            {
                variables.Add(varName, 0);
            }
            variables[varName] = value;
        }
    }
}
