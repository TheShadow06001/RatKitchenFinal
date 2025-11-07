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

        var obstaclesToSpawn = Mathf.Clamp(Random.Range(minObstacles, maxObstacles + 1), 0, availablePoints.Count);

        if (needsObstacleAtStart && availablePoints.Count > 0)
        {
            var frontIndex = Random.Range(0, Mathf.Min(2, availablePoints.Count));

            var frontPoint = availablePoints[frontIndex];

            SpawnAtPoint(frontPoint);

            availablePoints.Remove(frontPoint);

            obstaclesToSpawn--;
        }

        if (needsObstacleAtEnd && availablePoints.Count > 0)
        {
            var backStart = Mathf.Max(0, availablePoints.Count - 2);

            var backIndex = Random.Range(backStart, availablePoints.Count);

            var backPoint = availablePoints[backIndex];

            SpawnAtPoint(backPoint);

            availablePoints.Remove(backPoint);

            obstaclesToSpawn--;
        }

        for (var i = 0; i < obstaclesToSpawn && availablePoints.Count > 0; i++)
        {
            var index = Random.Range(0, availablePoints.Count);

            var point = availablePoints[index];

            availablePoints.RemoveAt(index);

            SpawnAtPoint(point);
        }
    }

    private void SpawnAtPoint(Transform point)
    {
        var prefabToSpawn = possibleObstacles[Random.Range(0, possibleObstacles.Count)];

        if (prefabToSpawn == null)
            return;


        var obstacle = Instantiate(prefabToSpawn, point.position, prefabToSpawn.transform.rotation, transform);

        if (prefabToSpawn.CompareTag("Pots&Pans"))
        {
            var parentScale = transform.lossyScale.x;

            obstacle.transform.localScale = Vector3.one / parentScale;
        }
    }
}