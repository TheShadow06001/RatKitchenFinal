using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [SerializeField] private List<WallType> wallTypes = new();
    [SerializeField] private Transform generationPoint;
    [SerializeField] private float distanceBetweenWalls = 0f;
    [SerializeField] private int currentLevel = 1;

    private WallType lastWallType;
    private WallType secondLastWallType;

    private Dictionary<WallType, int> wallSpawnCounts = new();

    private void Start()
    {
        foreach (var type in wallTypes)
        {
            wallSpawnCounts[type] = 0;
        }
    }

    public WallType SpawnNextWall(Vector3 spawnBasePosition)
    {
        WallType chosenWall = WallTypeToSpawn();
        if (chosenWall == null)
            return null;

        Vector3 wallPosition = spawnBasePosition;
        GameObject newWall = KitchenPool.Instance.GetPooledWall(chosenWall, wallPosition, Quaternion.identity);
        newWall.SetActive(true);

        wallSpawnCounts[chosenWall]++;
        secondLastWallType = lastWallType;
        lastWallType = chosenWall;

        return chosenWall;
    }

    private WallType WallTypeToSpawn()
    {
        List<WallType> validType = new();

        foreach (var type in wallTypes)
        {
            if (!type.CanSpawnAtLevel(currentLevel))
                continue;

            if (wallSpawnCounts[type] >= GetScaledMaxCount(type))
                continue;

            if (IsInvalidWallNeighbour(type))
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return wallTypes.Find(w => w.isBaseCase) ?? wallTypes[0]; // standard-pick

        //weighted random algorithm
        float totalSpawnWeight = 0f;
        foreach (var type in validType)
        {
            totalSpawnWeight += type.spawnWeight;
        }

        float pickRandomWall = Random.value * totalSpawnWeight;
        float cumulative = 0;

        foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (pickRandomWall <= cumulative)
                return type;
        }

        return validType[0];
    }

    private bool IsInvalidWallNeighbour(WallType next)
    {
        if (lastWallType != null && next.cannotHaveNeighbour.Contains(lastWallType.tag))
            return true;

        return false;
    }

    private int GetScaledMaxCount(WallType type)
    {
        int baseCount = type.baseMaxCount;
        float scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);
    }

    public Vector3 GetNextSpawnPosition(Vector3 currentPosition)
    {
        float offsetX = lastWallType != null ? lastWallType.prefab.transform.localScale.x + distanceBetweenWalls : 0f;
        return currentPosition + new Vector3(offsetX, 0, 0);
    }


}
