using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VariableManager : MonoBehaviour
{
    public static VariableManager Instance { get;private set; }
    private Dictionary<string, object> Variables = new Dictionary<string, object>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Add(string variableName , object value)
    {
        if (Variables.ContainsKey(variableName))
        {
            throw new Exception("There's already a variable with same name");
        }

        Variables.Add(variableName, value);
    }

    public void Set(string variableName, object value)
    {
        if(!Variables.ContainsKey(variableName))
        {
            throw new Exception("Variable not found");
        }

        Variables[variableName] = value;
    }

    public object Get(string variableName)
    {
        if (!Variables.ContainsKey(variableName))
        {
            throw new Exception("Variable not found");
        }

        return Variables[variableName];
    }

    public bool Exists(string variableName)
    {
        if(Variables.ContainsKey(variableName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Delete(string variableName)
    {
        if (!Variables.ContainsKey(variableName))
        {
            throw new Exception("Variable not found");
        }

        Variables.Remove(variableName);
    }

}
