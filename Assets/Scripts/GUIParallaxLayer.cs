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

    public bool OnLimit(float MouseSpeed, float limitLeft, float limitRight)
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
    
    public void Move(float MouseSpeed , float limitLeft , float limitRight)
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

    
}
