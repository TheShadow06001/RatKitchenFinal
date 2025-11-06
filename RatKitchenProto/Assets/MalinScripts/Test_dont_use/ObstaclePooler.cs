using System.Collections.Generic;
using UnityEngine;

public class ObstaclePooler : MonoBehaviour
{
    public GameObject pooledObstacle;
    public int pooledAmount;
    List<GameObject> pooledObstacles;

    private void Start()
    {
        pooledObstacles = new();

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObstacle);
            obj.SetActive(false);
            pooledObstacles.Add(obj);
        }
    }

    public GameObject GetPooledObstacle()
    {
        for (int i = 0; i < pooledObstacles.Count; i++)
        {
            if (!pooledObstacles[i].activeInHierarchy)
            {
                return pooledObstacles[i];
            }
        }

        GameObject obj = Instantiate(pooledObstacle);
        obj.SetActive(false);
        pooledObstacles.Add(obj);
        return obj;
    }
}
