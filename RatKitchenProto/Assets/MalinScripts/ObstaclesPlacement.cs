using System.Collections.Generic;
using UnityEngine;

public class ObstaclesPlacement : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<GameObject> possibleObstacles = new();
    [SerializeField] private int minObstacles = 1;
    [SerializeField] private int maxObstacles = 1;
    [SerializeField] private bool needsObstacleAtStart = true;
    [SerializeField] private bool needsObstacleAtEnd = true;


    private void OnEnable()
    {
        Generate();
    }

    private void Generate()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        if (possibleObstacles == null || possibleObstacles.Count == 0) return;

        List<Transform> availablePoints = new(spawnPoints);

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

        for (int i = 0; i < obstaclesToSpawn && availablePoints.Count > 0; i++)
        {
            int index = Random.Range(0, availablePoints.Count);

            Transform point = availablePoints[index];

            availablePoints.RemoveAt(index);

            SpawnAtPoint(point);
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
        

    }
}
