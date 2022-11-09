using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Collections;

namespace pLdevTest
{
    public class HandleExpression
    {
        private static List<ExpressionElements> elements;
        private static int currentFunc;
        public static double EvaluateExpression(string expression, Dictionary<string, double> variables)
        {
            // Search dictionary for variables, if found convert expression to value from dictionary
            currentFunc = 0;
            DataTable dt = new DataTable();
            elements = new List<ExpressionElements>();

            string[] expressions = FormatExpressions(expression);
            
            for (int i = 0; i < expressions.Length; i++)
            {
                bool elementFuncFound = false;
                if (variables.ContainsKey(expressions[i]))
                {
                    expressions[i] = variables[expressions[i]].ToString();
                }
                if (Interpreter.ArrayContainsString(Interpreter.builtInFunctions, expressions[i].Split('(')[0]))
                {
                    // Check for element functions 
                    elementFuncFound = true;
                    switch (expressions[i].Split('(')[0])
                    {
                        case "sqrt":
                            elements.Add(ExpressionElements.Sqrt);
                            break;
                        case "sin":
                            elements.Add(ExpressionElements.Sin);
                            break;
                        case "tan":
                            elements.Add(ExpressionElements.Tan);
                            break;
                        case "cos":
                            elements.Add(ExpressionElements.Cos);
                            break;
                    }
                }

                if (elementFuncFound)
                {
                    // Get arguments from parentheses
                    string funcExpressions = expressions[i].Split('(', ')')[1];

                    string[] elementExpressions = FormatExpressions(funcExpressions);

                    switch (elements[0])
                    {
                        case ExpressionElements.Sqrt:
                            for (int x = 0; x < elementExpressions.Length; x++)
                            {
                                elementExpressions[x] = GetVariableValue(elementExpressions[x], variables);
                            }
                            double elementFuncValue = Math.Sqrt(Convert.ToDouble(dt.Compute(string.Join(" ", elementExpressions), "")));
                            expressions[i] = elementFuncValue.ToString();
                            break;
                        case ExpressionElements.Sin:
                            break;
                        case ExpressionElements.Tan:
                            break;
                        case ExpressionElements.Cos:
                            break;
                    }
                    currentFunc++;
                }
            }
            string joinedString = string.Join(" ", expressions);
            joinedString = joinedString.Replace(",", ".");
            Debug.WriteLine(joinedString);
            double value = Convert.ToDouble(dt.Compute(joinedString, ""));

            return value;
        }

        private static string GetVariableValue(string expression, Dictionary<string, double> variables)
        {
            if (variables.ContainsKey(expression))
            {
                string value = variables[expression].ToString();
                return value;
            }
            return expression;
        }

        private static string[] FormatExpressions(string expression)
        {
            string code = expression;
            List<string> sections = new List<string>();
            // string regex = @"(?<=\().+?\)";
            string substring = "";
            string currentChar;

            bool insideParen = false;
            bool breakExpression = false;

            for (int z = 0; z < code.Length; z++)
            {
                currentChar = code[z].ToString();
                if (currentChar == " " || currentChar == "(" || breakExpression)
                {
                    // If inside parenthasese, don't split line at spaces
                    if (currentChar == "(" || insideParen)
                    {
                        if (currentChar == ")")
                        {
                            insideParen = false;
                        } else
                        {
                            insideParen = true;
                        }
                        breakExpression = true;
                        substring += currentChar;
                    }
                    else
                    {
                        sections.Add(substring);
                        substring = "";
                        breakExpression = false;
                    }
                }
                else
                {
                    substring += currentChar;
                }
                if (z == code.Length - 1)
                {
                    sections.Add(substring);
                }
            }
            return sections.ToArray();
        }
    }
}
