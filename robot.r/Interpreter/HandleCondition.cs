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
        public static bool GetResults(int lineIndex, int stopIndex, List<string> lines)
        {
            string currLine = "";
            currLine = Interpreter.GetInsideParentheses(lines[lineIndex], lineIndex);

            if(currLine == null)
            {
                return false;
            }

            // Split by AND (&&) operator
            string[] splitANDs = currLine.Split(" && ", StringSplitOptions.TrimEntries);

            List<bool> logicResults = new List<bool>();

            for (int z = 0; z < splitANDs.Length; z++)
            {
                bool orLogic = false;

                // Split by OR || operators
                string[] splitByLogic = splitANDs[z].Split(" || ", StringSplitOptions.TrimEntries);

                bool[] results = new bool[splitByLogic.Length];

                // Get results for each argument split by ||
                for (int x = 0; x < splitByLogic.Length; x++)
                {
                    results[x] = ConditionHandler(lineIndex, stopIndex, splitByLogic[x]);
                }
                for(int x = 0; x < splitByLogic.Length; x++)
                {
                    if (results[x])
                    {
                        orLogic = true;
                    }
                }
                if (orLogic)
                {
                    logicResults.Add(true);
                }
                else
                {
                    logicResults.Add(false);
                }
            }
           
            // If ANY of the logical operations were false, return FALSE. Otherwise return TRUE
            if (logicResults.Contains(false))
            {
                return false;
            } else
            {
                return true;
            }
        }

        public static bool ConditionHandler(int lineIndex, int stopIndex, string currLine)
        {
            string[] splitByArguments = currLine.Split(Interpreter.operators, StringSplitOptions.None);
            List<Double> arguments = new List<Double>();
            List<ArgumentOperators> elements = new List<ArgumentOperators>();

            for (int x = 0; x < splitByArguments.Length; x++)
            {
                double argumentExpression = HandleExpression.GetResults(splitByArguments[x], Interpreter.variables, lineIndex);
                arguments.Add(argumentExpression);
            }

            switch (currLine)
            {
                case string a when currLine.Contains("=="):
                    elements.Add(ArgumentOperators.Equals);
                    break;

                case string a when currLine.Contains("!="):
                    elements.Add(ArgumentOperators.NotEquals);
                    break;

                case string a when currLine.Contains("<"):
                    elements.Add(ArgumentOperators.SmallerThan);
                    break;

                case string a when currLine.Contains(">"):
                    elements.Add(ArgumentOperators.BiggerThan);
                    break;

                default:
                    // Error handling
                    codeInput.errorLine = lineIndex;
                    codeInput.errorText = "Operator error. (Maybe a misstyped operator?)";
                    return false;
                    break;
            }
            if (arguments.Count == 1)
            {
                codeInput.errorLine = lineIndex;
                codeInput.errorText = "Spacing error. Use spaces between expressions.";
                return false;
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
