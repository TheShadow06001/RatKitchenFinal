using System.Collections.Generic;
using UnityEngine;

public class ObstaclePooler : MonoBehaviour
{
    public GameObject pooledObstacle;
    public int pooledAmount;
    private List<GameObject> pooledObstacles;

    private void Start()
    {
        pooledObstacles = new List<GameObject>();

        for (var i = 0; i < pooledAmount; i++)
        {
            var obj = Instantiate(pooledObstacle);
            obj.SetActive(false);
            pooledObstacles.Add(obj);
        }
    }

    public GameObject GetPooledObstacle()
    {
        for (var i = 0; i < pooledObstacles.Count; i++)
            if (!pooledObstacles[i].activeInHierarchy)
                return pooledObstacles[i];

        var obj = Instantiate(pooledObstacle);
        obj.SetActive(false);
        pooledObstacles.Add(obj);
        return obj;
    }
}