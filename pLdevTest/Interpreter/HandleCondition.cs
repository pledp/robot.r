using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public static class HandleCondition
    {
        public static bool ConditionHandler(int lineIndex, int stopIndex, List<String> lines)
        {
            string currLine = Interpreter.GetInsideParentheses(lines[lineIndex]);
            string[] splitByArguments = currLine.Split(Interpreter.operators, StringSplitOptions.None);
            List<Double> arguments = new List<Double>();
            List<ArgumentOperators> elements = new List<ArgumentOperators>();

            for (int x = 0; x < splitByArguments.Length; x++)
            {
                double argumentExpression = HandleExpression.GetResults(splitByArguments[x], Interpreter.variables);
                arguments.Add(argumentExpression);
            }

            switch (lines[lineIndex])
            {
                case string a when lines[lineIndex].Contains("=="):
                    elements.Add(ArgumentOperators.Equals);
                    Debug.WriteLine("==");
                    break;

                case string a when lines[lineIndex].Contains("!="):
                    elements.Add(ArgumentOperators.NotEquals);
                    break;

                case string a when lines[lineIndex].Contains("<"):
                    elements.Add(ArgumentOperators.SmallerThan);
                    break;

                case string a when lines[lineIndex].Contains(">"):
                    elements.Add(ArgumentOperators.BiggerThan);
                    break;
            }
            bool ifCondition = false;
            switch (elements[0])
            {
                case ArgumentOperators.Equals:
                    if (arguments[0] == arguments[1])
                    {
                        ifCondition = true;
                    }
                    break;

                case ArgumentOperators.NotEquals:
                    if (arguments[0] != arguments[1])
                    {
                        ifCondition = true;
                    }
                    break;

                case ArgumentOperators.SmallerThan:
                    if (arguments[0] < arguments[1])
                    {
                        ifCondition = true;
                    }
                    break;

                case ArgumentOperators.BiggerThan:
                    if (arguments[0] > arguments[1])
                    {
                        ifCondition = true;
                    }
                    break;
            }
            if (ifCondition)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
