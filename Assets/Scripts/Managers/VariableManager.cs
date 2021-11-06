using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using CommandTerminal;
using CodeStage.AntiCheat.Storage;

//Author: Mert Karavural
//Date: 14 Oct 2020

public class VariableManager : MonoBehaviour
{
    public static VariableManager Instance { get; private set; }
    private Dictionary<string, object> Variables = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void AddLocal(string variableName, string value)
    {
        if (ObscuredPrefs.HasKey(variableName))
        {
            throw new Exception("There's already a local with the same name");
        }

        ObscuredPrefs.Set(variableName, value);
        ObscuredPrefs.Save();
    }

    public void AddVariable(string variableName, object value)
    {
        if (Variables.ContainsKey(variableName))
        {
            throw new Exception("There's already a variable with the same name");
        }

        Variables.Add(variableName, value);
    }

    public void SetOrAddLocal(string variableName, string value)
    {
        AddOrSetLocal(variableName, value);
    }

    public void SetOrAddVariable(string variableName, object value)
    {
        AddOrSetVariable(variableName, value);
    }

    public void AddOrSetVariable(string variableName, object value)
    {
        if (!Variables.ContainsKey(variableName))
        {
            AddVariable(variableName, value);
        }
        else
        {
            SetVariable(variableName, value);
        }
    }

    public void AddOrSetLocal(string variableName, string value)
    {
        if (!ObscuredPrefs.HasKey(variableName))
        {
            AddLocal(variableName, value);
        }
        else
        {
            SetLocal(variableName, value);
        }
    }

    public void SetLocal(string variableName, string value)
    {
        if (!ObscuredPrefs.HasKey(variableName))
        {
            throw new Exception("Local not found");
        }

        ObscuredPrefs.Set(variableName, value);
        ObscuredPrefs.Save();
    }

    public void SetVariable(string variableName, object value)
    {
        if (!Variables.ContainsKey(variableName))
        {
            throw new Exception("Variable not found");
        }

        Variables[variableName] = value;
    }

    public object GetVariable(string variableName)
    {
        if (!Variables.ContainsKey(variableName))
        {
            throw new Exception("Variable not found");
        }

        return Variables[variableName];
    }

    public string GetLocal(string variableName)
    {
        if (!ObscuredPrefs.HasKey(variableName))
        {
            throw new Exception("Local not found");
        }

        return ObscuredPrefs.Get<string>(variableName);
    }

    public bool VariableExists(string variableName)
    {
        return Variables.ContainsKey(variableName);
    }

    public bool LocalExists(string name)
    {
        return ObscuredPrefs.HasKey(name);
    }


    public void DeleteLocal(string variableName)
    {
        if (!ObscuredPrefs.HasKey(variableName))
        {
            throw new Exception("Local not found");
        }

        ObscuredPrefs.DeleteKey(variableName);
        ObscuredPrefs.Save();
    }

    public void DeleteVariable(string variableName)
    {
        if (!Variables.ContainsKey(variableName))
        {
            throw new Exception("Variable not found");
        }

        Variables.Remove(variableName);
    }

    public void ResetVariables()
    {

        Variables.Clear();
    }

    public void ResetLocals()
    {

        ObscuredPrefs.DeleteAll();
        ObscuredPrefs.Set("List", "");
        ObscuredPrefs.Save();
    }

    /*public string[] ListLocals()
    {
        return ObscuredPrefs.Get<string>("List").Split('~');
    }*/

    public List<string> ListVariables()
    {
        List<string> temp = new List<string>();
        foreach (KeyValuePair<string, object> p in Variables)
        {
            temp.Add(p.Key.ToString() + ": " + p.Value.ToString());
        }

        return temp;
    }
    void Start()
    {
    }

    public void GotoScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
