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
            double index = 0;
            if(key.Contains('['))
            {
                string splitBySquareBrackets = key.Split(new string[] { "[", "]" }, StringSplitOptions.None)[1];
                builtInVariableKey = builtInVariableKey.Split("[")[0];

                index = HandleExpression.GetResults(splitBySquareBrackets, Interpreter.variables);
            }

            Debug.WriteLine(builtInVariableKey);

            if (Interpreter.builtInVariables.ContainsKey(builtInVariableKey))
            {
                string variableName = key.Split(".")[1];

                switch (builtInVariableKey)
                {
                    case "robot":
                        if (Interpreter.builtInVariables["robot"].ContainsKey(variableName))
                        {
                            Interpreter.builtInVariables["robot"][variableName][0] = (int)value;

                            switch (variableName)
                            {
                                case "x":
                                    GameScene.playground.player.posX = Interpreter.builtInVariables["robot"]["x"][0];
                                    if (MissionHandler.Mission == 1)
                                    {
                                        MissionHandler.MissionComplete = true;
                                    }
                                    break;

                                case "y":
                                    GameScene.playground.player.posY = Interpreter.builtInVariables["robot"]["y"][0];
                                    if (MissionHandler.Mission == 0)
                                    {
                                        MissionHandler.MissionComplete = true;
                                    }
                                    break;
                            }    
                        }
                        break;

                    case "enemy":
                        if (Interpreter.builtInVariables["enemy"].ContainsKey(variableName))
                        {
                            Interpreter.builtInVariables["enemy"][variableName][(int)index] = (int)value;
                            switch (variableName)
                            {
                                case "x":
                                    if (GameScene.playground.enemies[(int)index] != null)
                                    {
                                        GameScene.playground.enemies[(int)index].posX = Interpreter.builtInVariables["enemy"]["x"][(int)index];
                                    }
                                    break;

                                case "y":
                                    if (GameScene.playground.enemies[(int)index] != null)
                                    {
                                        GameScene.playground.enemies[(int)index].posY = Interpreter.builtInVariables["enemy"]["y"][(int)index];
                                    }
                                    break;
                            }
                        }
                        break;

                    case "gem":
                        Debug.WriteLine("gem");
                        if (Interpreter.builtInVariables["gem"].ContainsKey(variableName))
                        {
                            Interpreter.builtInVariables["gem"][variableName][(int)index] = (int)value;
                            switch (variableName)
                            {
                                case "x":
                                    if (GameScene.playground.gems[(int)index] != null)
                                    {
                                        GameScene.playground.gems[(int)index].posX = Interpreter.builtInVariables["gem"]["x"][(int)index];
                                    }
                                    break;

                                case "y":
                                    if (GameScene.playground.gems[(int)index] != null)
                                    {
                                        GameScene.playground.gems[(int)index].posY = Interpreter.builtInVariables["gem"]["y"][(int)index];
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
