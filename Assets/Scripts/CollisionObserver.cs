using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
