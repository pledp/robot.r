using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public static class HandleBuiltInVariables
    {
        public static bool GetResults(string key, double value)
        {

            // Check if built in variable exists, if it does, set its value to the expression
            string builtInVariableKey = key.Split(".")[0];
            if (Interpreter.builtInVariables.ContainsKey(builtInVariableKey))
            {
                string variableName = key.Split(".")[1];
                switch (builtInVariableKey)
                {
                    case "robot":
                        if (Interpreter.builtInVariables["robot"].ContainsKey(variableName))
                        {
                            Interpreter.builtInVariables["robot"][variableName] = (int)value;

                            switch (variableName)
                            {
                                case "x":
                                    Game1.playground.player.playerX = Interpreter.builtInVariables["robot"]["x"];
                                    if (MissionHandler.Mission == 1 && MissionHandler.World == 1)
                                    {
                                        MissionHandler.MissionComplete = true;
                                    }
                                    break;

                                case "y":
                                    Game1.playground.player.playerY = Interpreter.builtInVariables["robot"]["y"];
                                    if (MissionHandler.Mission == 0 && MissionHandler.World == 1)
                                    {
                                        MissionHandler.MissionComplete = true;
                                    }
                                    break;
                            }    
                        }
                        break;
                }
                return true;
            } else
            {
                return false;
            }
        }
    }
}
