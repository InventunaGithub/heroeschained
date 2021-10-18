using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// Author: Mert Karavural
// Date: 7 Oct 2020

public class GUIParallaxLayer : MonoBehaviour
{
    public float Speed = 1f;
    public RectTransform RectTransformLayer;

    void Start()
    {
        RectTransformLayer = GetComponent<RectTransform>();
    }

    public bool OnLimitX(float MouseSpeed, float limitLeft, float limitRight)
    {
        if (RectTransformLayer.localPosition.x + (MouseSpeed * Speed) > limitRight)
        {
            return true;
        }
        else if (RectTransformLayer.localPosition.x + (MouseSpeed * Speed) < limitLeft)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void MoveX(float MouseSpeed , float limitLeft , float limitRight)
    {
        if(RectTransformLayer.localPosition.x + (MouseSpeed * Speed) > limitRight)
        {
            RectTransformLayer.DOAnchorPosX(limitRight, 1);
        }
        else if (RectTransformLayer.localPosition.x + (MouseSpeed * Speed) < limitLeft)
        {
            RectTransformLayer.DOAnchorPosX(limitLeft, 1);
        }
        else
        {
            RectTransformLayer.DOAnchorPosX(RectTransformLayer.localPosition.x + (MouseSpeed * Speed), 1);
        }
    }
    public bool OnLimitY(float MouseSpeed, float limitBottom, float limitTop)
    {
        if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) > limitTop)
        {
            return true;
        }
        else if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) < limitBottom)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MoveY(float MouseSpeed, float limitBottom, float limitTop)
    {
        if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) > limitTop)
        {
            RectTransformLayer.DOAnchorPosY(limitTop, 1);
        }
        else if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) < limitBottom)
        {
            RectTransformLayer.DOAnchorPosY(limitBottom, 1);
        }
        else
        {
            RectTransformLayer.DOAnchorPosY(RectTransformLayer.localPosition.y + (MouseSpeed * Speed), 1);
        }
    }


}
