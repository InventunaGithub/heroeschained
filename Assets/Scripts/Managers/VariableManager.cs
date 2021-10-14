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
            return null;
            Debug.LogException(e, this);
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
            Debug.LogException(e, this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
