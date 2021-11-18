using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 11.2021
public class CollisionObserver : MonoBehaviour
{
    public List<GameObject> CollidedObjects;

    private void OnTriggerEnter(Collider other)
    {
        CollidedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        CollidedObjects.Remove(other.gameObject);
    }
}
