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
        try
        {
            Variables.Add(variableName, value);
        }
        catch (Exception e)
        {
            Debug.Log("There already a variable named " + variableName);
            Debug.LogException(e, this);
        }
    }

    public void Set(string variableName, object value)
    {
        try
        {
            Variables[variableName] = value;
        }
        catch (Exception e)
        {
            Debug.Log("There is no variable named " + variableName);
            Debug.LogException(e, this);
        }
    }

    public object Get(string variableName)
    {
        try
        {
            return Variables[variableName];
        }
        catch (Exception e)
        {
            Debug.Log("There is no variable named " + variableName);
            Debug.LogException(e, this);
            return "DoesNotExist";
        }
    }

    public bool Exists(string variableName)
    {
        if(Variables.ContainsKey(variableName))
        {
            return true;
        }
        else
        {
            Debug.Log("There is no variable named " + variableName);
            return false;
        }
    }

    public void Delete(string variableName)
    {
        try
        {
            Variables.Remove(variableName);
        }
        catch (Exception e)
        {
            Debug.Log("There is no variable named " + variableName);
            Debug.LogException(e, this);
        }
    }

    void Start()
    {
        // FOR TEST PURPOSES ONLY, TO BE DELETED BY THE LEAD
        Add("Var1", "String");
        Add("Var2", 1);
        Add("Var1", "String");
        Debug.Log("Does Var1 Exists ? " + Exists("Var1").ToString());
        Debug.Log("Does Var5 Exists ? " + Exists("Var5").ToString());
        Debug.Log("Var 1 = " + Get("Var1").ToString());
        Set("Var1", "New String Set");
        Debug.Log("Changed Var 1 = " + Get("Var1").ToString());
        Delete("Var1");
        Debug.Log("Changed Var 1 = " + Get("Var1").ToString());
        Debug.Log("Trying to get Var5 " + Get("Var5").ToString());
        Delete("Var5");
        Set("Var5" , 123);
        
    }
}
