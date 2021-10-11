using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityChanger : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float Intensity = 1;
    Material objectMaterial;
    Color materialColor;
    List<Color> childMaterialColors;
    float lastIntensity;

    void Start()
    {
        objectMaterial = gameObject.GetComponent<Renderer>().material;
        materialColor = objectMaterial.color;
        childMaterialColors = new List<Color>();
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            childMaterialColors.Add(g.gameObject.GetComponent<Renderer>().material.color);
        }
        SetIntensity();
    }

    private void SetIntensity()
    {
        // MERT GÖREV BAŞLANGICI
        objectMaterial.color = materialColor * Intensity;
        int index = 0;
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            g.gameObject.GetComponent<Renderer>().material.color = childMaterialColors[index] * Intensity;
            index = index + 1;
        }
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
