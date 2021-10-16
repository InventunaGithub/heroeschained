using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CommandTerminal;
using System.IO;

public class ConsoleManager : MonoBehaviour
{
    
    [RegisterCommand(Help = "Shows Version")]
    static void CommandVer(CommandArg[] args)
    {
        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("Version 0");
    }

    [RegisterCommand(Name = "Variables", Help = "Variables command", MinArgCount = 1, MaxArgCount = 3)]
    static void CommandVariables(CommandArg[] args)
    {
        string command = args[0].String;
        if(command.ToLower() == "add")
        {
            if(args.Length == 3)
            {
                string variableName = args[1].String;
                object value = args[2];
                VariableManager.Instance.Add(variableName, value);
                if (Terminal.IssuedError) return; // Error will be handled by Terminal
                Terminal.Log(variableName + " added with value :  " + value.ToString());
            }
            else
            {
                throw new Exception("not enough arguments.");
            }
        }
        else if (command.ToLower() == "set")
        {
            if (args.Length == 3)
            {
                string variableName = args[1].String;
                object value = args[2];
                VariableManager.Instance.Set(variableName, value);
                if (Terminal.IssuedError) return; // Error will be handled by Terminal
                Terminal.Log(variableName + " added with value :  " + value.ToString());
            }
            else
            {
                throw new Exception("not enough arguments.");
            }
        }
        else if (command.ToLower() == "get")
        {
            if (args.Length == 2)
            {
                string variableName = args[1].String;
                if (Terminal.IssuedError) return; // Error will be handled by Terminal

                Terminal.Log("Value : " + VariableManager.Instance.Get(variableName).ToString());
            }
            else
            {
                if(args.Length == 3)
                {
                    throw new Exception("Too Many arguments.");
                }
                if (args.Length == 1)
                {
                    throw new Exception("not enough arguments.");
                }
            }
        }
        else if (command.ToLower() == "list")
        {
            if (args.Length == 1)
            {
                if (Terminal.IssuedError) return; // Error will be handled by Terminal
                foreach (string item in VariableManager.Instance.ListAll())
                {
                    Terminal.Log(item);
                }
            }
            else
            {
                throw new Exception("Too Many arguments.");
            }
        }
        else if (command.ToLower() == "dump")
        {
            if (args.Length == 1)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/dump.txt";
                string allVariables = "";
                foreach (string item in VariableManager.Instance.ListAll())
                {
                    allVariables += item + " \n";
                }
                File.WriteAllText(path, allVariables);

                if (Terminal.IssuedError) return; // Error will be handled by Terminal

                Terminal.Log("Dump.txt created at " + path);
            }
            else
            {
                throw new Exception("Too Many arguments.");
            }
        }
        else
        {
            throw new Exception("Command does not exist");
        }


    }

    [RegisterCommand(Name = "Variables.Add", Help = "Sets a variable to the given value (variables.set VarName Value)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandVariablesAdd(CommandArg[] args)
    {
        string variableName = args[0].String;
        object value = args[1];
        VariableManager.Instance.Add(variableName, value);

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log(variableName + " added with value  " + value.ToString());
    }

    [RegisterCommand(Name = "Variables.Set" , Help = "Sets a variable to the given value (variables.set VarName Value)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandVariablesSet(CommandArg[] args)
    {
        string variableName = args[0].String;
        object value = args[1];
        VariableManager.Instance.Set(variableName, value);

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log(variableName + " set to  " + value.ToString());
    }

    [RegisterCommand(Name = "Variables.Get", Help = "Shows given variable", MinArgCount = 1 , MaxArgCount = 1)]
    static void CommandVariablesGet(CommandArg[] args)
    {
        string variableName = args[0].String;
        

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("Value : " + VariableManager.Instance.Get(variableName).ToString());
    }

    [RegisterCommand(Name = "Variables.List", Help = "Shows all variables")]
    static void CommandVariablesList(CommandArg[] args)
    {
        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        foreach (string item in VariableManager.Instance.ListAll())
        {
            Terminal.Log(item);
        }
    }

    [RegisterCommand(Name = "Variables.dump", Help = "All variables dumped into variables.txt")]
    static void CommandVariablesDump(CommandArg[] args)
    {
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/dump.txt";
        string allVariables = "";
        foreach (string item in VariableManager.Instance.ListAll())
        {
            allVariables += item + " \n";
        }
        File.WriteAllText(path, allVariables);

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("Dump.txt created at " + path);
    }

    [RegisterCommand(Name = "Pause", Help = "Pauses the game")]
    static void CommandPause(CommandArg[] args)
    {
        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("Game Paused");
        Debug.Break();
    }

    [RegisterCommand(Name = "fps.on", Help = "shows FPS counter")]
    static void CommandFpsOn(CommandArg[] args)
    {
        VariableManager.Instance.ShowFPS = true;
        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("Fps meter opened");
    }

    [RegisterCommand(Name = "fps.off", Help = "Hides FPS counter")]
    static void CommandFpsOff(CommandArg[] args)
    {
        VariableManager.Instance.ShowFPS = false;
        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log("Fps meter closed");
    }

    [RegisterCommand(Name = "os", Help = "Outputs operating system and platform.")]
    static void CommandOs(CommandArg[] args)
    {
        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log(SystemInfo.operatingSystem);
    }

}
