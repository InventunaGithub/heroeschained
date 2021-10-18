using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CommandTerminal;

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

    public void Reset()
    {
      
        Variables.Clear();
    }

    public List<string> ListAll()
    {
        List<string> temp = new List<string>();
        foreach (KeyValuePair<string, object> p in Variables)
        {
            temp.Add(p.Key.ToString() +" - "+ p.Value.ToString());
        }
        return temp;
    }

    // fps display

    public Text Display;
    public bool Detailed = false;
    public bool ShowFPS = false;
    public float Frequency = 0.25f;

    // Start is called before the first frame update
    float totalFps = 0;
    int totalRead = 0;
    void Start()
    {
        StartCoroutine(DisplayFPS());
        Display.gameObject.SetActive(ShowFPS);
    }

    IEnumerator DisplayFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(Frequency);

            float fps = (1.0f / Time.deltaTime);
            totalFps += fps;
            totalRead += 1;
            if (ShowFPS)
            {
                Display.gameObject.SetActive(ShowFPS);
                if (Detailed)
                {
                    Display.text = "FPS: " + fps.ToString("N0") + " (Avg: " + (totalFps / totalRead).ToString("N0") + ")";
                }
                else
                {
                    Display.text = "FPS: " + (totalFps / totalRead).ToString("N0");
                }
            }
            else
            {
                Display.gameObject.SetActive(ShowFPS);
            }

        }
    }

}
