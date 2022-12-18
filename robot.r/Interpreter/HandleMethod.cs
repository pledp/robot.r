using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    static class HandleMethod
    {
        public static void RunMethod(string method, string arguments, int lineIndex)
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

                case "shoot":
                    methods.Add(BuiltInMethods.Shoot);
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
                            try
                            {
                                newString = newString.Substring(0, newString.LastIndexOf('"'));
                            }
                            catch
                            {
                                codeInput.errorLine = lineIndex;
                                codeInput.errorText = "Expected: \"";
                            }

                            formattedPrintString += newString;
                        }
                        else
                        {
                            formattedPrintString += HandleExpression.GetResults(printSegments[x], Interpreter.variables, lineIndex);
                        }
                    }
                    Interpreter.consoleText.Add(formattedPrintString);
                    //Debug.WriteLine(HandleExpression.GetResults(printString, Interpreter.variables));
                    break;
                case BuiltInMethods.Sleep:

                    if(MissionHandler.Mission == 7)
                    {
                        MissionHandler.MissionComplete = true;
                    }
                    Interpreter.CurrentDelay = (int)HandleExpression.GetResults(arguments, Interpreter.variables, lineIndex);
                    break;

                case BuiltInMethods.Shoot:
                    GameScene.playground.bullets.Add(new Bullet(GameScene.playground.player.posX, GameScene.playground.player.posY, GameScene.playground.playground.X, GameScene.playground.playground.Y));
                    break;
            }
        }
    }
}
