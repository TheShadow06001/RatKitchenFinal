using System.Collections.Generic;
using UnityEngine;

public class WallPooler : MonoBehaviour
{
    public GameObject pooledWallObject;
    public int pooledAmount;
    List<GameObject> pooledWallObjects;

    private void Start()
    {
        pooledWallObjects = new();

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledWallObject);
            obj.SetActive(false);
            pooledWallObjects.Add(obj);
        }
    }

    public GameObject GetPooledWallObject()
    {
        for (int i = 0; i < pooledWallObjects.Count; i++)
        {
            if (!pooledWallObjects[i].activeInHierarchy)
            {
                return pooledWallObjects[i];
            }
        }

        GameObject obj = Instantiate(pooledWallObject);
        obj.SetActive(false);
        pooledWallObjects.Add(obj);
        return obj;
    }
}
