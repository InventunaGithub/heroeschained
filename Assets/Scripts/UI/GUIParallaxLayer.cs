using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public bool OnLimitY(float MouseSpeed, float limitLeft, float limitRight)
    {
        if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) > limitRight)
        {
            return true;
        }
        else if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) < limitLeft)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MoveY(float MouseSpeed, float limitLeft, float limitRight)
    {
        if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) > limitRight)
        {
            RectTransformLayer.DOAnchorPosY(limitRight, 1);
        }
        else if (RectTransformLayer.localPosition.y + (MouseSpeed * Speed) < limitLeft)
        {
            RectTransformLayer.DOAnchorPosY(limitLeft, 1);
        }
        else
        {
            RectTransformLayer.DOAnchorPosY(RectTransformLayer.localPosition.y + (MouseSpeed * Speed), 1);
        }
    }


}
