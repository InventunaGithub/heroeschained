using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIParallaxHandler : MonoBehaviour
{
    public GUIParallaxLayer[] Layers;
    public float LimitLeft = -1000;
    public float LimitRight = 1000;
    Vector3 lastMousePos;
    bool mousePressed;
    bool limitReached;

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
            bool temp = false;
            foreach (var layer in Layers)
            {
                if (layer.OnLimit(mouseDelta.x, LimitLeft, LimitRight))
                {
                    temp = true;
                }
            }

            if(temp)
            {
                limitReached = true;
            }
            else
            {
                limitReached = false;
            }

            if(!limitReached)
            {
                foreach (var layer in Layers)
                {
                    layer.Move(mouseDelta.x, LimitLeft, LimitRight);
                }
            }
            lastMousePos = Input.mousePosition;
            
        }
    }
}
