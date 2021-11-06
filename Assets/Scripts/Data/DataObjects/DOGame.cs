using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOGame : DataObject
{
    private string id;
    private string name;

    public string ID
    {
        get
        {
            return id;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public DOGame(string id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
