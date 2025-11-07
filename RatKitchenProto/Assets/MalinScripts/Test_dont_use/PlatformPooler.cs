using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour
{
    public GameObject pooledObject;
    public int pooledAmount; // amount of objects to put in pool
    private List<GameObject> pooledObjects;

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (var i = 0; i < pooledAmount; i++)
        {
            var obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (var i = 0; i < pooledObjects.Count; i++)
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i]; // return from list of pooled objects

        //create new if nothing left in pool
        var obj = Instantiate(pooledObject);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
}