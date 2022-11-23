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
            string currLine = Interpreter.GetInsideParentheses(lines[lineIndex]);

            // Split by logical operators (|| and &&)
            string[] splitByLogic = currLine.Split(Interpreter.logicalOperators, StringSplitOptions.TrimEntries);

            // Get the logical operators (|| and &&)
            string[] splitLogics = currLine.Split(splitByLogic, StringSplitOptions.RemoveEmptyEntries);
            List<bool> logicResults = new List<bool>();

            // If logical operators exist, convert values with logical operators
            if (splitLogics.Length > 0)
            {
                bool[] results = new bool[splitByLogic.Length];

                bool orLogic = false;

                Debug.WriteLine(splitByLogic[0]);

                // Check condition for each statement, return TRUE if true, return FALSE if false
                for (int x = 0; x < splitByLogic.Length; x++)
                {
                    results[x] = ConditionHandler(lineIndex, stopIndex, splitByLogic[x]);
                }

                for (int x = 0; x < splitLogics.Length; x++)
                {
                    orLogic = false;
                    if (splitLogics[x] == " || ")
                    {
                        // Find the amount of repetitive || operators in a row, if any any of them are TRUE return 1 TRUE
                        int amountOfANDs = FindRepetitiveOR(splitLogics, x);
                        Debug.WriteLine(amountOfANDs);
                        for (int y = x; y < amountOfANDs + x + 1; y++)
                        {
                            if (results[y])
                            {
                                Debug.WriteLine(splitByLogic[y]);
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
                        x += amountOfANDs;
                    }
                }
            }
            // If no logical operators exist, return currLine's conditional value
            else
            {
                logicResults.Add(ConditionHandler(lineIndex, stopIndex, currLine));
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
        private static int FindRepetitiveOR(string[] splitLogics, int startIndex)
        {
            int amount = 0;

            for(int x = startIndex; x < splitLogics.Length; x++)
            {
                if(splitLogics[x] == " || ")
                {
                    amount++;
                } else
                {
                    return amount;
                }
            }
            return amount;
        }

        public static bool ConditionHandler(int lineIndex, int stopIndex, string currLine)
        {
            string[] splitByArguments = currLine.Split(Interpreter.operators, StringSplitOptions.None);
            List<Double> arguments = new List<Double>();
            List<ArgumentOperators> elements = new List<ArgumentOperators>();

            for (int x = 0; x < splitByArguments.Length; x++)
            {
                double argumentExpression = HandleExpression.GetResults(splitByArguments[x], Interpreter.variables);
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
