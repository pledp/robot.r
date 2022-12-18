using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.r
{
    public static class HandleBuiltInVariables
    {
        public static bool GetResults(string key, double value, int lineIndex)
        {

            // Check if built in variable exists, if it does, set its value to the expression
            string builtInVariableKey = key.Split(".")[0];
            double index = 0;
            if(key.Contains('['))
            {
                string splitBySquareBrackets = key.Split(new string[] { "[", "]" }, StringSplitOptions.None)[1];
                builtInVariableKey = builtInVariableKey.Split("[")[0];

                index = HandleExpression.GetResults(splitBySquareBrackets, Interpreter.variables, lineIndex);
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
                                    GameScene.playground.player.posX = (int)Interpreter.builtInVariables["robot"]["x"][0];
                                    if (MissionHandler.Mission == 1)
                                    {
                                        MissionHandler.MissionComplete = true;
                                    }
                                    break;

                                case "y":
                                    GameScene.playground.player.posY = (int)Interpreter.builtInVariables["robot"]["y"][0];
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
                            try
                            {
                                Interpreter.builtInVariables["enemy"][variableName][(int)index] = (int)value;
                                switch (variableName)
                                {
                                    case "x":
                                        if (GameScene.playground.enemies[(int)index] != null)
                                        {
                                            GameScene.playground.enemies[(int)index].posX = (int)Interpreter.builtInVariables["enemy"]["x"][(int)index];
                                        }
                                        break;

                                    case "y":
                                        if (GameScene.playground.enemies[(int)index] != null)
                                        {
                                            GameScene.playground.enemies[(int)index].posY = (int)Interpreter.builtInVariables["enemy"]["y"][(int)index];
                                        }
                                        break;
                                }
                            }
                            catch
                            {
                                codeInput.errorLine = lineIndex;
                                codeInput.errorText = "Indexing error. Index out of range.";
                            }
                        }
                        break;

                    case "gem":
                        Debug.WriteLine("gem");
                        if (Interpreter.builtInVariables["gem"].ContainsKey(variableName))
                        {
                            try
                            {
                                Interpreter.builtInVariables["gem"][variableName][(int)index] = (int)value;
                                switch (variableName)
                                {
                                    case "x":
                                        if (GameScene.playground.gems[(int)index] != null)
                                        {
                                            GameScene.playground.gems[(int)index].posX = (int)Interpreter.builtInVariables["gem"]["x"][(int)index];
                                        }
                                        break;

                                    case "y":
                                        if (GameScene.playground.gems[(int)index] != null)
                                        {
                                            GameScene.playground.gems[(int)index].posY = (int)Interpreter.builtInVariables["gem"]["y"][(int)index];
                                        }
                                        break;
                                }
                            }
                            catch
                            {
                                codeInput.errorLine = lineIndex;
                                codeInput.errorText = "Indexing error. Index out of range.";
                            }
                        }
                        break;

                    case "colorBlock":
                        Debug.WriteLine("block");
                        if (Interpreter.builtInVariables["colorBlock"].ContainsKey(variableName))
                        {
                            try
                            {
                                Interpreter.builtInVariables["colorBlock"][variableName][(int)index] = (int)value;
                                switch (variableName)
                                {
                                    case "x":
                                        if (GameScene.playground.coloredBlocks[(int)index] != null)
                                        {
                                            GameScene.playground.coloredBlocks[(int)index].posX = (int)Interpreter.builtInVariables["colorBlock"]["x"][(int)index];
                                        }
                                        break;

                                    case "y":
                                        if (GameScene.playground.coloredBlocks[(int)index] != null)
                                        {
                                            GameScene.playground.coloredBlocks[(int)index].posY = (int)Interpreter.builtInVariables["colorBlock"]["y"][(int)index];
                                        }
                                        break;
                                }
                            }
                            catch
                            {
                                codeInput.errorLine = lineIndex;
                                codeInput.errorText = "Indexing error. Index out of range.";
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
