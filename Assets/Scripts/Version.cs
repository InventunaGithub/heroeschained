using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Version : MonoBehaviour
{
    [SerializeField] int VersionMajor;
    [SerializeField] int VersionMinor;
    [SerializeField] int Build;

    public string GetVersion()
    {
        return VersionMajor.ToString() + "." + VersionMinor.ToString()
            + " build " + Build.ToString();
    }
}
