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
using info.lundin.math;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Schema;

namespace pLdevTest
{
    public class HandleExpression
    {
        private static DataTable dt;
        private static bool elementFuncFound;
        public static double GetResults(string expression, Dictionary<string, double> variables, int lineIndex)
        {
            // Evaluates the value of the expressions.
            double value = EvaluateExpression(expression, variables, lineIndex);
            return value;
        }

        private static double EvaluateExpression(string expression, Dictionary<string, double> variables, int lineIndex)
        {
            dt = new DataTable();
            List<ExpressionElements> elements;
            elements = new List<ExpressionElements>();

            // Splits the expressions into readable segments.
            string[] expressions = FormatExpressions(expression);

            for (int i = 0; i < expressions.Length; i++)
            {
                elementFuncFound = false;

                // If expression is a variable that is declared, set value to variables value
                expressions[i] = GetVariableValue(expressions[i], variables, lineIndex);

                expressions[i] = GetClassValue(expressions[i], variables, lineIndex);

                // Check if text matches built in functions
                string splitArrayContainer = expressions[i].Split('(')[0];
                if (Interpreter.ArrayContainsString(Interpreter.builtInFunctions, splitArrayContainer))
                {
                    switch (splitArrayContainer)
                    {
                        case "sqrt":
                            elements.Add(ExpressionElements.Sqrt);
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
                        funcExpressions = funcExpressions.Substring(funcExpressions.IndexOf("("));
                        funcExpressions = funcExpressions.Substring(1, funcExpressions.Length - 2);
                    }

                    double elementExpressions = EvaluateExpression(funcExpressions, variables, lineIndex);
                    double elementFuncValue = elementExpressions;

                    switch (elements[0])
                    {
                        case ExpressionElements.Sqrt:
                            elementFuncValue = Math.Sqrt(elementExpressions);
                            break;
                        case ExpressionElements.Sin:
                            elementFuncValue = Math.Sin(elementExpressions);
                            break;
                        case ExpressionElements.Tan:
                            elementFuncValue = Math.Tan(elementExpressions);
                            break;
                        case ExpressionElements.Cos:
                            elementFuncValue = Math.Cos(elementExpressions);
                            break;
                    }
                    expressions[i] = elementFuncValue.ToString();
                }
            }
            string joinedString = string.Join(" ", expressions);
            joinedString = joinedString.Replace(",", ".");

            double value = 0;
            try
            {
                value = Convert.ToDouble(dt.Compute(joinedString, ""));
            }
            catch (Exception ex)
            {
                codeInput.errorLine = lineIndex;
                codeInput.errorText = "Assignment error. (Maybe a typo or a unassigned variable?)";
            }

            return value;   
        }

        private static string GetVariableValue(string expression, Dictionary<string, double> variables, int lineIndex)
        {
            if (variables.ContainsKey(expression))
            {
                string value = variables[expression].ToString();
                return value;
            }

            // Check for built in variables, if it exists, get its value
            else if (expression.Contains("."))
            {
                string variableName = expression.Split(".")[0];
                variableName = variableName.Split(new string[] { "[", "]" }, StringSplitOptions.None)[0];

                string variableName2 = expression.Split(".")[1];
                Debug.WriteLine(variableName2);
                if (Interpreter.builtInVariables.ContainsKey(variableName))
                {
                    if (Interpreter.builtInVariables[variableName].ContainsKey(variableName2))
                    {
                        double index = 0;
                        if (expression.Contains('['))
                        {
                            string splitBySquareBrackets = expression.Split(new string[] { "[", "]" }, StringSplitOptions.None)[1];
                            variableName = variableName.Split("[")[0];

                            index = GetResults(splitBySquareBrackets, Interpreter.variables, lineIndex);
                        }
                        string value = Interpreter.builtInVariables[variableName][variableName2][(int)index].ToString();

                        return value;
                    }
                }
            }
            return expression;
        }

        public static string[] FormatExpressions(string expression)
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

        private static string GetClassValue(string expression, Dictionary<string, double> variables, int lineIndex)
        {
            string value = expression;
            if (value.Contains("."))
            {
                string className = expression.Split(".")[0];
                string variableName = expression.Split(".")[1];

                switch (className)
                {
                    case "color":
                        switch (variableName)
                        {
                            case "Red":
                                value = "1";
                                break;
                            case "Green":
                                value = "2";
                                break;
                            case "Blue":
                                value = "3";
                                break;
                        }
                        break;
                }
            } 
            else
            {
                switch (value)
                {
                    case "Red":
                        value = "1";
                        break;
                    case "Green":
                        value = "2";
                        break;
                    case "Blue":
                        value = "3";
                        break;
                }
            }
            return value;
        }
    }
}
