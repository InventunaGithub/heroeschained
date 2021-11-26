using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandInfo
{
    int height;
    int width;
    string landEast;
    string landWest;
    string landNorth;
    string landSouth;
    float locX;
    float locY;
    float locZ;
    string theme;

    public int Height
    {
        get
        {
            return height;
        }
    }

    public int Width
    {
        get
        {
            return width;
        }
    }

    public string LandEast
    {
        get
        {
            return landEast;
        }
    }

    public string LandWest
    {
        get
        {
            return landWest;
        }
    }

    public string LandNorth
    {
        get
        {
            return landNorth;
        }
    }

    public string LandSouth
    {
        get
        {
            return landSouth;
        }
    }

    public float LocX
    {
        get
        {
            return locX;
        }
    }

    public float LocY
    {
        get
        {
            return locY;
        }
    }

    public float LocZ
    {
        get
        {
            return locZ;
        }
    }

    public string Theme
    {
        get
        {
            return theme;
        }
    }

    public LandInfo(int width, int height, float locX, float locY, float locZ, string theme, string landEast, string landWest, string landNorth, string landSouth)
    {
        this.width = width;
        this.height = height;
        this.locX = locX;
        this.locY = locY;
        this.locZ = locZ;
        this.theme = theme;

        this.landEast = landEast;
        this.landWest = landWest;
        this.landNorth = landNorth;
        this.landSouth = landSouth;
    }
}
