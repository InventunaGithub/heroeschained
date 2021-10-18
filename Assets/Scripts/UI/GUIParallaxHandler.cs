using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Mert Karavural
// Date: 7 Oct 2020

public class GUIParallaxHandler : MonoBehaviour
{
    public GUIParallaxLayer[] Layers;
    public float LimitLeft = -1000;
    public float LimitBottom = -1000;
    public float LimitRight = 1000;
    public float LimitTop = 1000;
    Vector3 lastMousePos;
    bool mousePressed;
    bool limitReachedX;
    bool limitReachedY;
    public bool MoveVertical = false;

    public Vector3 mouseDelta
    {
        get
        {
            return Input.mousePosition - lastMousePos;
        }
    }

    void Update()
    { 
        if(Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            mousePressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
        }
    }

    void FixedUpdate()
    {
        if (mousePressed)
        {
            bool tempX = false;
            foreach (var layer in Layers)
            {
                if (layer.OnLimitX(mouseDelta.x, LimitLeft, LimitRight))
                {
                    tempX = true;
                }
            }

            if (tempX)
            {
                limitReachedX = true;
            }
            else
            {
                limitReachedX = false;
            }

            if (!limitReachedX)
            {
                foreach (var layer in Layers)
                {
                    layer.MoveX(mouseDelta.x, LimitLeft, LimitRight);
                }
            }
            if(MoveVertical)
            {
                bool tempY = false;
                foreach (var layer in Layers)
                {
                    if (layer.OnLimitY(mouseDelta.y, LimitBottom, LimitTop))
                    {
                        tempY = true;
                    }
                }
                if (tempY)
                {
                    limitReachedY = true;
                }
                else
                {
                    limitReachedY = false;
                }

                if (!limitReachedY)
                {
                    foreach (var layer in Layers)
                    {
                        layer.MoveY(mouseDelta.y, LimitBottom, LimitTop);
                    }
                }
            }
            lastMousePos = Input.mousePosition;
            
        }
    }
}
