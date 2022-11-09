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
        private static DataTable dt;
        private static bool elementFuncFound;
        public static double GetResults(string expression, Dictionary<string, double> variables)
        {
            // Evaluates the value of the expressions.
            double value = EvaluateExpression(expression, variables);
            return value;
        }

        private static double EvaluateExpression(string expression, Dictionary<string, double> variables)
        {
            dt = new DataTable();
            List<ExpressionElements> elements;
            elements = new List<ExpressionElements>();

            // Splits the expressions into readable segments.
            string[] expressions = FormatExpressions(expression);
            foreach(string s in expressions)
            {
                Debug.WriteLine("EXPRESSIONS 1: " + s);
            }

            for (int i = 0; i < expressions.Length; i++)
            {
                elementFuncFound = false;

                // If expression is a variable that is declared, set value to variables value
                expressions[i] = GetVariableValue(expressions[i], variables);

                // Check if text matches built in functions
                string splitArrayContainer = expressions[i].Split('(')[0];
                Debug.WriteLine(splitArrayContainer);
                if (Interpreter.ArrayContainsString(Interpreter.builtInFunctions, splitArrayContainer))
                {
                    switch (splitArrayContainer)
                    {
                        case "sqrt":
                            elements.Add(ExpressionElements.Sqrt);
                            Debug.WriteLine("test: ");
                            elementFuncFound = true;
                            break;
                        case "sin":
                            elements.Add(ExpressionElements.Sin);
                            elementFuncFound = true;
                            break;
                        case "tan":
                            elements.Add(ExpressionElements.Tan);
                            elementFuncFound = true;
                            break;
                        case "cos":
                            elements.Add(ExpressionElements.Cos);
                            elementFuncFound = true;
                            break;
                    }
                }
                if (elementFuncFound)
                {
                    string funcExpressions = expressions[i];

                    // Get arguments from parentheses
                    if (expressions[i].Contains("(") || expressions[i].Contains(")"))
                    {
                        Debug.WriteLine(funcExpressions);
                        funcExpressions = funcExpressions.Substring(funcExpressions.IndexOf("("));
                        Debug.WriteLine("Expressin1: " + funcExpressions);
                        funcExpressions = funcExpressions.Substring(1, funcExpressions.Length - 2);
                        Debug.WriteLine("Expression: " + funcExpressions);

                    }

                    double elementExpressions = EvaluateExpression(funcExpressions, variables);

                    switch (elements[0])
                    {
                        case ExpressionElements.Sqrt:
                            double elementFuncValue = Math.Sqrt(elementExpressions);
                            expressions[i] = elementFuncValue.ToString();
                            break;
                        case ExpressionElements.Sin:
                            break;
                        case ExpressionElements.Tan:
                            break;
                        case ExpressionElements.Cos:
                            break;
                    }
                }
            }
            string joinedString = string.Join(" ", expressions);
            joinedString = joinedString.Replace(",", ".");
            Debug.WriteLine("String: " + joinedString);
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

            // NOTE TO SELF: Don't use regex.
            // string regex = @"(?<=\().+?\)";
            string substring = "";
            string currentChar;

            int nestedParen = 0;

            bool insideParen = false;
            bool breakExpression = false;

            // Run through all charachters, generally at spaces, break the line.
            for (int z = 0; z < code.Length; z++)
            {
                currentChar = code[z].ToString();
                if (currentChar == " " || currentChar == "(" || breakExpression)
                {
                    // If inside parenthesese, or nested within another parenthesese, don't break line.
                    if (currentChar == "(" || insideParen)
                    {
                        if (currentChar == ")")
                        {
                            nestedParen--;
                            if(nestedParen == 0)
                            {
                                insideParen = false;
                            }
                        } else if(currentChar == "(")
                        {
                            nestedParen++;
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
