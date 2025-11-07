using System.Collections.Generic;
using UnityEngine;

public class WallPooler : MonoBehaviour
{
    public GameObject pooledWallObject;
    public int pooledAmount;
    private List<GameObject> pooledWallObjects;

    private void Start()
    {
        pooledWallObjects = new List<GameObject>();

        for (var i = 0; i < pooledAmount; i++)
        {
            var obj = Instantiate(pooledWallObject);
            obj.SetActive(false);
            pooledWallObjects.Add(obj);
        }
    }

    public GameObject GetPooledWallObject()
    {
        for (var i = 0; i < pooledWallObjects.Count; i++)
            if (!pooledWallObjects[i].activeInHierarchy)
                return pooledWallObjects[i];

        var obj = Instantiate(pooledWallObject);
        obj.SetActive(false);
        pooledWallObjects.Add(obj);
        return obj;
    }
}