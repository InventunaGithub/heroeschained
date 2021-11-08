using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using CommandTerminal;
using System.IO;

//Author: Mert Karavural
//Date: 14 Oct 2020


public class ConsoleManager : MonoBehaviour
{
    #region Command callbacks
    [RegisterCommand(Help = "Resets Current Level")]
    static void CommandResetLevel(CommandArg[] args)
    {
        if (Terminal.IssuedError)
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   


    [RegisterCommand(Help = "Shows Version")]
    static void CommandVer(CommandArg[] args)
    {
        if (Terminal.IssuedError)
        {
            return;
        }

        Version ver = Transform.FindObjectOfType<Version>();
        if(ver == null)
        {
            Terminal.Log("Version could not be determined");        }
        else
        {
            Terminal.Log(ver.GetVersion());
        }
    }

    [RegisterCommand(Name = "Variables", Help = "Variables command", MinArgCount = 1, MaxArgCount = 3)]
    static void CommandVariables(CommandArg[] args)
    {
        if(args.Length == 0)
        {
            Terminal.Log("Not enough arguments");
            return;
        }

        string command = args[0].String;
        if(command.ToLower() == "add")
        {
            if(args.Length == 3)
            {
                string variableName = args[1].String;
                object value = args[2];

                try
                {
                    VariableManager.Instance.AddVariable(variableName, value);
                    if (Terminal.IssuedError)
                    {
                        return;
                    }

                    Terminal.Log(variableName + " added with value :  " + value.ToString());
                } catch(Exception ex)
                {
                    Terminal.Log(ex.Message);
                }
            }
            else
            {
                Terminal.Log("Not enough arguments");
            }
        }
        else if (command.ToLower() == "set")
        {
            if (args.Length == 3)
            {
                string variableName = args[1].String;
                object value = args[2];

                try
                {
                    VariableManager.Instance.SetVariable(variableName, value);

                    if (Terminal.IssuedError)
                    {
                        return;
                    }

                    Terminal.Log(variableName + " value changed to :  " + value.ToString());
                } catch(Exception ex)
                {
                    Terminal.Log(ex.Message);
                }
            }
            else
            {
                Terminal.Log("Not enough arguments");
            }
        }
        else if (command.ToLower() == "get")
        {
            if (args.Length == 2)
            {
                string variableName = args[1].String;
                if (Terminal.IssuedError)
                {
                    return;
                }

                try
                {
                    Terminal.Log("Value : " + VariableManager.Instance.GetVariable(variableName).ToString());
                } catch(Exception ex)
                {
                    Terminal.Log(ex.Message);
                }
            }
            else
            {
                if(args.Length == 3)
                {
                    Terminal.Log("Too Many arguments.");
                }
                if (args.Length == 1)
                {
                    Terminal.Log("Not enough arguments");
                }
            }
        }
        else if (command.ToLower() == "list")
        {
            if (args.Length == 1)
            {
                if (Terminal.IssuedError)
                {
                    return;
                }

                foreach (string item in VariableManager.Instance.ListVariables())
                {
                    Terminal.Log(item);
                }
            }
            else
            {
                Terminal.Log("Too Many arguments");
            }
        }
        else if (command.ToLower() == "dump")
        {
            if (args.Length == 1)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/dump.txt";
                string allVariables = "";
                foreach (string item in VariableManager.Instance.ListVariables())
                {
                    allVariables += item + " \n";
                }

                File.WriteAllText(path, allVariables);

                if (Terminal.IssuedError)
                {
                    return;
                }

                Terminal.Log("Dump.txt created at " + path);
            }
            else
            {
                Terminal.Log("Too Many arguments");
            }
        }
        else if (command.ToLower() == "reset")
        {
            if (args.Length == 1)
            {
                VariableManager.Instance.ResetVariables();

                if (Terminal.IssuedError)
                {
                    return;
                }

                Terminal.Log("All variables have been deleted");
            }
            else
            {
                Terminal.Log("Too Many arguments");
            }
        }
        else if (command.ToLower() == "import")
        {
            if (args.Length == 1)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/dump.txt";
                if(File.Exists(path))
                {
                    string[] Lines = File.ReadAllLines(path);

                    foreach (string Line in Lines)
                    {
                        VariableManager.Instance.AddVariable(Line.Split('-')[0].Trim(), Line.Split('-')[1].Trim());
                    }
                }
                else
                {
                    Terminal.Log("Dump.txt does not exist");
                }

                if (Terminal.IssuedError)
                {
                    return;
                }

                Terminal.Log("Dump.txt read at " + path);
            }
            else
            {
                Terminal.Log("Too Many arguments");
            }
        }
        else
        {
            Terminal.Log("Command does not exist");
        }
    }

    [RegisterCommand(Name = "Locals", Help = "Locals command", MinArgCount = 1, MaxArgCount = 3)]
    static void CommandLocals(CommandArg[] args)
    {
        if (args.Length == 0)
        {
            Terminal.Log("Not enough arguments");
            return;
        }

        string command = args[0].String;
        if (command.ToLower() == "add")
        {
            if (args.Length == 3)
            {
                string variableName = args[1].String;
                string value = args[2].ToString();
                VariableManager.Instance.AddLocal(variableName, value);
                if (Terminal.IssuedError)
                {
                    return;
                }

                Terminal.Log("Local " + variableName + " added with value :  " + value.ToString());
            }
            else
            {
                Terminal.Log("Not enough arguments");
            }
        }
        else if (command.ToLower() == "set")
        {
            if (args.Length == 3)
            {
                string variableName = args[1].String;
                string value = args[2].ToString();

                try
                {
                    VariableManager.Instance.SetLocal(variableName, value);

                    if (Terminal.IssuedError)
                    {
                        return;
                    }

                    Terminal.Log("Local " + variableName + " value changed to :  " + value.ToString());
                } catch(Exception ex)
                {
                    Terminal.Log(ex.Message);
                }
            }
            else
            {
                Terminal.Log("Not enough arguments");
            }
        }
        else if (command.ToLower() == "get")
        {
            if (args.Length == 2)
            {
                string variableName = args[1].String;
                if (Terminal.IssuedError)
                {
                    return;
                }

                try
                {
                    Terminal.Log("Local Value : " + VariableManager.Instance.GetLocal(variableName).ToString());
                } catch(Exception ex)
                {
                    Terminal.Log(ex.Message);
                }
            }
            else
            {
                if (args.Length == 3)
                {
                    Terminal.Log("Too Many arguments");
                }
                if (args.Length == 1)
                {
                    Terminal.Log("Not enough arguments");
                }
            }
        }
        else if (command.ToLower() == "reset")
        {
            if (args.Length == 1)
            {
                VariableManager.Instance.ResetLocals();

                if (Terminal.IssuedError)
                {
                    return;
                }

                Terminal.Log("All locals have been deleted");
            }
            else
            {
                Terminal.Log("Too Many arguments");
            }
        }
        else
        {
            Terminal.Log("Command does not exist");
        }
    }

    [RegisterCommand(Name = "Variables Add", Help = "Adds a variable with the given value (variables add VarName Value)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandVariablesAdd(CommandArg[] args)
    { 
        //This is just for the help page
    }

    [RegisterCommand(Name = "Variables Set" , Help = "Sets a variable to the given value (variables set VarName Value)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandVariablesSet(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Variables Get", Help = "Shows given variable", MinArgCount = 1 , MaxArgCount = 1)]
    static void CommandVariablesGet(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Variables List", Help = "Shows all variables")]
    static void CommandVariablesList(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Variables Dump", Help = "All variables dumped into variables.txt")]
    static void CommandVariablesDump(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Variables Reset", Help = "Clears all variables")]
    static void CommandVariablesReset(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Variables Import", Help = "Reads from the Dump file (resets all local variables created )")]
    static void CommandVariablesImport(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Locals Add", Help = "Adds a local with the given value (locals add VarName Value)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandLocalsAdd(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Locals Set", Help = "Sets a local with the given value (locals set VarName Value)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandLocalsSet(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Locals Get", Help = "Gets a local with the given value (locals get VarName)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandLocalsGet(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Locals Reset", Help = "Clears all locals")]
    static void CommandLocalsReset(CommandArg[] args)
    {
        //This is just for the help page
    }

    [RegisterCommand(Name = "Pause", Help = "Pauses the game")]
    static void CommandPause(CommandArg[] args)
    {
        if (Terminal.IssuedError)
        {
            return;
        }

        Terminal.Log("Game Paused");
        Debug.Break();
    }

    [RegisterCommand(Name = "fps", Help = "Toggles FPS counter")]
    static void CommandFpsOn(CommandArg[] args)
    {
        if (args.Length == 0)
        {
            Terminal.Log("Not enough arguments");
            return;
        }

        GUIFPSDisplayer gui = FindObjectOfType<GUIFPSDisplayer>();

        if (gui != null)
        {
            if (args.Length == 0)
            {

            }
            else
            {
                string command = args[0].String;
                gui.Enabled = command == "on";

                if (Terminal.IssuedError)
                {
                    return;
                }

                Terminal.Log("FPS meter " + (command == "on" ? "opened" : "closed"));
            }
        } else
        {
            Terminal.Log("No FPS displays found");
        }
    }

    [RegisterCommand(Name = "os", Help = "Outputs operating system and platform")]
    static void CommandOs(CommandArg[] args)
    {
        if (Terminal.IssuedError)
        {
            return;
        }

        Terminal.Log(SystemInfo.operatingSystem);
    }

    [RegisterCommand(Name = "building", Help = "Enables/disables city or guild buildings")]
    static void CommandBuilding(CommandArg[] args)
    {
        if (args.Length <= 1)
        {
            Terminal.Log("Not enough arguments");
            return;
        }

        string command = args[0].String;
        string parameter = args[1].String;
        switch (command)
        {
            case "tavern_city":
            case "city_tavern":
                VariableManager.Instance.SetOrAddVariable("city_tavern_open", parameter == "enable");
                break;
            case "tavern_guild":
            case "guild_tavern":
                VariableManager.Instance.SetOrAddVariable("guild_tavern_open", parameter == "enable");
                break;
            case "city_arena":
            case "arena":
                VariableManager.Instance.SetOrAddVariable("city_arena_open", parameter == "enable");
                break;
            case "city_market":
            case "market":
                VariableManager.Instance.SetOrAddVariable("city_market_open", parameter == "enable");
                break;
            case "city_palace":
            case "city_royal_palace":
            case "city_royalpalace":
            case "palace":
            case "royal_palace":
            case "royalpalace":
                VariableManager.Instance.SetOrAddVariable("city_royal_palace_open", parameter == "enable");
                break;
            case "city_slums":
            case "slums":
                VariableManager.Instance.SetOrAddVariable("city_slums_open", parameter == "enable");
                break;
            case "city_gate":
            case "gate":
                VariableManager.Instance.SetOrAddVariable("city_gate_open", parameter == "enable");
                break;
            default:
                Terminal.Log("Warning: No such building could be found");
                return;
        }

        if (Terminal.IssuedError)
        {
            return;
        }

        Terminal.Log("Building status has been altered");
    }
    #endregion

}
