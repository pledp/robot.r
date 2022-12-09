using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    static class HandleMethod
    {
        public static void RunMethod(string method, string arguments)
        {
            List<BuiltInMethods> methods;
            methods = new List<BuiltInMethods>();

            switch (method)
            {
                case "print":
                    methods.Add(BuiltInMethods.Print);
                    break;

                case "sleep":
                    methods.Add(BuiltInMethods.Sleep);
                    break;
            }

            switch (methods[0])
            {
                case BuiltInMethods.Print:
                    string[] printSegments = arguments.Split(",");
                    string formattedPrintString = "";

                    for(int x = 0; x < printSegments.Length; x++)
                    {
                        if (printSegments[x].Contains('"'))
                        {
                            string newString = printSegments[x].Substring(printSegments[x].IndexOf('"') + 1);
                            newString = newString.Substring(0, newString.LastIndexOf('"'));

                            formattedPrintString += newString;
                        }
                        else
                        {
                            formattedPrintString += HandleExpression.GetResults(printSegments[x], Interpreter.variables);
                        }
                    }
                    Interpreter.consoleText.Add(formattedPrintString);
                    //Debug.WriteLine(HandleExpression.GetResults(printString, Interpreter.variables));
                    break;
                case BuiltInMethods.Sleep:

                    if(MissionHandler.Mission == 7 && MissionHandler.World == 1)
                    {
                        MissionHandler.MissionComplete = true;
                    }
                    Interpreter.CurrentDelay = (int)HandleExpression.GetResults(arguments, Interpreter.variables);
                    break;
            }
        }
    }
}
