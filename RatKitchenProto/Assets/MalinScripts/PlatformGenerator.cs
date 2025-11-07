using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private List<PlatformType> platformTypes = new();
    [SerializeField] public int platformsPerWall = 5;
    [SerializeField] private float distanceBetweenPlatforms;
    [SerializeField] private int currentLevel = 1;

    private PlatformType lastPlatformType;

    private readonly Dictionary<PlatformType, int> platformSpawnCounts = new();
    private PlatformType secondLastPlatformType;

    private void Start()
    {
        foreach (var type in platformTypes) platformSpawnCounts[type] = 0;
    }

    public void SpawnPlatformsAlongWall(Vector3 wallPosition, WallType wall)
    {
        var spawnPos = wallPosition;

        for (var i = 0; i < platformsPerWall; i++)
        {
            var chosenPlatform = PlatformTypeToSpawn();
            if (chosenPlatform == null)
                continue; // �ndrat fr�n return

            var prefabToSpawn = chosenPlatform.GetRandomPrefab();
            var newPlatform = KitchenPool.Instance.GetPooledObject(chosenPlatform, spawnPos, Quaternion.identity);
            newPlatform.SetActive(true);

            platformSpawnCounts[chosenPlatform]++;
            secondLastPlatformType = lastPlatformType;
            lastPlatformType = chosenPlatform;

            spawnPos += new Vector3(chosenPlatform.prefab.transform.localScale.x + distanceBetweenPlatforms, 0, 0);
        }
    }

    private PlatformType PlatformTypeToSpawn()
    {
        List<PlatformType> validType = new();

        foreach (var type in platformTypes)
        {
            if (!type.CanSpawnAtLevel(currentLevel))
                continue;

            if (platformSpawnCounts[type] >= GetScaledMaxCount(type))
                continue;

            if (IsInvalidPlatformNeighbour(type))
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return platformTypes.Find(p => p.isBaseCase) ?? platformTypes[0];

        //weighted random algorithm
        var totalSpawnWeight = 0f;
        foreach (var type in validType) totalSpawnWeight += type.spawnWeight;

        var randomPick = Random.value * totalSpawnWeight;
        float cumulative = 0;

        foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (randomPick <= cumulative)
                return type;
        }

        return validType[0];
    }

    private bool IsInvalidPlatformNeighbour(PlatformType next)
    {
        if (lastPlatformType != null && next.cannotHaveNeighbour.Contains(lastPlatformType.tag))
            return true;

        if (next.mustHaveCounterBetween &&
            (lastPlatformType?.tag == next.tag || secondLastPlatformType?.tag == next.tag))
            return true;

        return false;
    }

    private int GetScaledMaxCount(PlatformType type)
    {
        var baseCount = type.baseMaxCount;
        var scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);
    }
}