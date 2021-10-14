using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [RegisterCommand(Name = "Variables.Add", Help = "Adds a new variable to the variables (variables.addnew VarName Value)", MinArgCount = 2, MaxArgCount = 2)]
    static void CommandVariablesAdd(CommandArg[] args)
    {
        string variableName = args[0].String;
        object value = args[1];
        VariableManager.Instance.Add(variableName, value);

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log(variableName + " Added with the value " + value.ToString());
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
