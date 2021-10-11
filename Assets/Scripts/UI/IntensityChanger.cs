using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityChanger : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float Intensity = 1;

    float lastIntensity;

    void Start()
    {
        SetIntensity();
    }

    private void SetIntensity()
    {
        // MERT GÖREV BAŞLANGICI

        // MERT GÖREV BİTİŞİ

        lastIntensity = Intensity;
    }

    void Update()
    {
        if(lastIntensity != Intensity)
        {
            SetIntensity();
        }
    }
}
