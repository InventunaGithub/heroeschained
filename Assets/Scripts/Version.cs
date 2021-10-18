using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Version : MonoBehaviour
{
    [SerializeField] int versionMajor;
    [SerializeField] int versionMinor;
    [SerializeField] int build;

    public string GetVersion()
    {
        return versionMajor.ToString() + "." + versionMinor.ToString()
            + " build " + build.ToString();
    }
}
