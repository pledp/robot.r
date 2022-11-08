using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;

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

            // Regex to find expressions within parentheses
            string[] funcExpressions = Regex.Matches(expression, @"\((.+?)\)").Cast<Match>().Select(m => m.Groups[1].Value).ToArray();

            string[] expressions = expression.Split(' ', ')');
            
            for (int i = 0; i < expressions.Length; i++)
            {
                bool elementFuncFound = false;
                if (variables.ContainsKey(expressions[i]))
                {
                    expressions[i] = variables[expressions[i]].ToString();
                }
                if (Interpreter.ArrayContainsString(Interpreter.builtInFunctions, expressions[i].Split('(')[0]))
                {
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
                    expressions[i] = Regex.Replace(expressions[i], @"[^A-Z]+", String.Empty);
                    Debug.WriteLine(expressions[i] + "test");
                    string[] elementExpressions = funcExpressions[currentFunc].ToString().Split(' ');
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
            foreach(string fun in expressions)
            {
                Debug.WriteLine(fun);
            }
            //double value = Convert.ToDouble(dt.Compute(string.Join(" ", expressions), ""));

            return 5;
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
    }
}
