using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallType", menuName = "Scriptable Objects/Wall Type")]
public class WallType : ScriptableObject
{
    [Header("General")] public string typeOfWall;

    public string tag;
    public GameObject prefab;

    [Header("Spawn Settings")] public int MaxCountPerRun = 2;

    public int baseMaxCount = 2;
    public float spawnWeight = 1f;
    public List<string> cannotHaveNeighbour = new();
    public bool isBaseCase;
    public int platformsPerWall = 5;

    [Header("Variants, if applicable")] public GameObject[] variantPrefabs;

    [Header("Difficulty Scaling")] public int minLevelToAppear = 1;

    public int maxLevelToAppear = 999;
    public AnimationCurve spawnChanceCurve;
    public float maxCountMultiplierPerLevel = 1.0f;

    public bool CanSpawnAtLevel(int level)
    {
        return level >= minLevelToAppear && level <= maxLevelToAppear;
    }

    public GameObject GetRandomPrefab()
    {
        if (variantPrefabs != null && variantPrefabs.Length > 0 && Random.value < 0.5f)
            return variantPrefabs[Random.Range(0, variantPrefabs.Length)];
        return prefab;
    }
}