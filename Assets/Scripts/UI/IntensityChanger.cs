using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityChanger : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float Intensity = 0;
    Material objectMaterial;
    Color materialColor;
    List<Color> childMaterialColors;
    float lastIntensity;
    public float IntensityBorder;
    bool borderPassed = false;
    List<GameObject> landObjects;

    void Start()
    {
        var tempLandObjects = FindObjectsOfType<LandObject>();
        landObjects = new List<GameObject>();
        foreach (var tempLandObject in tempLandObjects)
        {
            landObjects.Add(tempLandObject.gameObject);
        }
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
        if (Intensity < IntensityBorder && borderPassed)
        {
            Intensity = IntensityBorder;
        }
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
        float closestDistance = float.MaxValue;

        foreach (var landObject in landObjects)
        {
            float viewRange = landObject.GetComponent<LandObject>().ViewRange;
            float dist = Vector3.Distance(gameObject.transform.position, landObject.transform.position);
            Debug.Log(dist);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                if (dist < viewRange)
                {
                    Intensity = (1 + viewRange) / ((1 + viewRange) + (1 + dist));
                    borderPassed = true;
                }
                else
                {
                    Intensity = 0;
                }
            }

        }

        if (lastIntensity != Intensity)
        {
            SetIntensity();
        }
    }
}

