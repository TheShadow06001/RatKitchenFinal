using System.Collections.Generic;
using UnityEngine;

public class ObstaclesPlacement : MonoBehaviour
{
    [Header("Spawn points and Prefabs")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<GameObject> possibleObstacles = new();

    [Header("Spawn settings and placement rules")]
    [SerializeField] private int minObstacles = 1;
    [SerializeField] private int maxObstacles = 1;
    [SerializeField] private bool needsObstacleAtStart = true;
    [SerializeField] private bool needsObstacleAtEnd = true;
    [SerializeField] private bool needsOnlyOneObstacle = false;

    [SerializeField] List<GameObject> spawnedObstacles = new();

    private void OnEnable()
    {
        ClearObstacles();
        Generate();
    }

    private void OnDisable()
    {
        ClearObstacles();
    }

    private void Generate()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        if (possibleObstacles == null || possibleObstacles.Count == 0) return;

        List<Transform> availablePoints = new(spawnPoints);



        if (needsOnlyOneObstacle)
        {
            Transform placementPoint = availablePoints[Random.Range(0, availablePoints.Count)];
            SpawnAtPoint(placementPoint);
            return;
        }

        int obstaclesToSpawn = Mathf.Clamp(Random.Range(minObstacles, maxObstacles + 1), 0, availablePoints.Count);

        if (needsObstacleAtStart && availablePoints.Count > 0)
        {
            int frontIndex = Random.Range(0, Mathf.Min(2, availablePoints.Count));

            Transform frontPoint = availablePoints[frontIndex];

            SpawnAtPoint(frontPoint);

            availablePoints.Remove(frontPoint);

            obstaclesToSpawn--;
        }

        if (needsObstacleAtEnd && availablePoints.Count > 0)
        {
            int backStart = Mathf.Max(0, availablePoints.Count - 2);
            int backIndex = Random.Range(backStart, availablePoints.Count);

            Transform backPoint = availablePoints[backIndex];

            SpawnAtPoint(backPoint);

            availablePoints.Remove(backPoint);

            obstaclesToSpawn--;
        }

        for (var i = 0; i < obstaclesToSpawn && availablePoints.Count > 0; i++)
        {
            int index = Random.Range(0, availablePoints.Count);

            Transform PlacementPoint = availablePoints[index];

            availablePoints.RemoveAt(index);

            SpawnAtPoint(PlacementPoint);
        }
    }

    private void SpawnAtPoint(Transform point)
    {
        GameObject prefabToSpawn = possibleObstacles[Random.Range(0, possibleObstacles.Count)];

        if (prefabToSpawn == null)
            return;


        GameObject obstacle = Instantiate(prefabToSpawn, point.position, prefabToSpawn.transform.rotation, transform);

        if (prefabToSpawn.CompareTag("Pots&Pans"))
        {
            float parentScale = transform.lossyScale.x;

            obstacle.transform.localScale = Vector3.one / parentScale;
        }

        spawnedObstacles.Add(obstacle);
    }

    private void ClearObstacles()
    {
        if (spawnedObstacles.Count > 0)
        {
            foreach (GameObject obj in spawnedObstacles)
            {
                if (obj != null)
                    Destroy(obj);
            }

            spawnedObstacles.Clear();
            return;
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform obj = transform.GetChild(i);
            if (obj.CompareTag("Obstacle"))
            {
                Destroy(obj.gameObject);
            }
        }
    }
}