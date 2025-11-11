using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KitchenGenerator : MonoBehaviour
{
    /* SHOULD PROBABLY SPLIT THIS INTO SEPARATE SCRIPTS FOR WALL, PLATFORM AND A HANDLER */

    [SerializeField] private List<PlatformType> platformTypes = new();
    [SerializeField] private List<WallType> wallTypes = new();

    [SerializeField] private Transform generationPoint;
    [SerializeField] private float distanceBetween;
    [SerializeField] public int maxPlatformsPerRun = 20;
    //TODO: Implement or Remove ↓
    //[SerializeField] private int basePackagesPerRun = 3; // 1 wall fits 5 counters = 1 package

    [SerializeField] private int currentLevel = 1;

    [SerializeField] private GameObject endPlatformPrefab;
    //[SerializeField] private GameObject endWallPrefab;

    [SerializeField] private int spawnedPlatforms;

    public GameObject cameraMover;

    private Vector3 startPosition;
    private Vector3 cameraStartPosition;
    private readonly bool canGenerate = true;
    private bool isLevelComplete;

    private PlatformType lastPlatformType;
    private WallType lastWallType;
    
    private readonly Dictionary<PlatformType, float> platformLengths = new();

    private readonly Dictionary<PlatformType, int> platformSpawnCounts = new();
    private PlatformType secondLastPlatformType;
    private WallType secondLastWallType;
    private GameObject spawnedEndWall;
    private GameObject spawnedEndPlatform;

    private Vector3 wallOffset;
    private readonly Dictionary<WallType, int> wallSpawnCounts = new();

    private void Awake()
    {
        startPosition = transform.position;
        cameraStartPosition = cameraMover.transform.position;
    }

    private void Start()
    {
        foreach (PlatformType type in platformTypes) platformSpawnCounts[type] = 0;

        foreach (WallType type in wallTypes) wallSpawnCounts[type] = 0;

        foreach (PlatformType type in platformTypes)
        {
            platformSpawnCounts[type] = 0;
            platformLengths[type] = GetPlatformLength(type.prefab);
        }
    }

    public void UpdateKitchenGenerator()
    {
        if (isLevelComplete)
            return;

        if (!canGenerate)
            return;

        if (spawnedPlatforms >= maxPlatformsPerRun)
        {
            SpawnEndPlatform();
            isLevelComplete = true;
            return;
        }

        /* WALL SPAWN */
        if (transform.position.z < generationPoint.position.z)
        {
            WallType chosenWall = WallTypeToSpawn();
            if (chosenWall == null)
                return;

            Vector3 wallSpawnPosition = transform.position;

            wallOffset = new Vector3(-0.87f, -0.09f, 0); // magic numbers (x = -0,94f)
            Quaternion wallRotation = Quaternion.Euler(0f, 180f, 0f);
            GameObject newWall = KitchenPool.Instance.GetPooledWall(chosenWall, wallSpawnPosition + wallOffset, wallRotation);

            newWall.SetActive(true);
            wallSpawnCounts[chosenWall]++;

            /* PLATFORM SPAWN */
            float platformSpacing = platformLengths[platformTypes[0]] + distanceBetween;
            float packageLength = chosenWall.platformsPerWall * platformSpacing;

            for (int i = 0; i < chosenWall.platformsPerWall; i++)
            {
                PlatformType chosenPlatform = PlatformTypeToSpawn();
                if (chosenPlatform == null)
                    return;

                Vector3 platformPos = wallSpawnPosition +
                                  new Vector3(chosenPlatform.xPositionSpawnOffset, 0, i * platformSpacing);
                Quaternion platformRotation = Quaternion.Euler(0f, 90f, 0f);

                GameObject newPlatform = KitchenPool.Instance.GetPooledObject(chosenPlatform, platformPos, platformRotation);
                newPlatform.SetActive(true);

                platformSpawnCounts[chosenPlatform]++;
                secondLastPlatformType = lastPlatformType;
                lastPlatformType = chosenPlatform;

                spawnedPlatforms++;
            }

            secondLastWallType = lastWallType;
            lastWallType = chosenWall;

            transform.position = wallSpawnPosition + new Vector3(0, 0, packageLength);
        }
    }

    private PlatformType PlatformTypeToSpawn()
    {
        List<PlatformType> validType = new();

        foreach (PlatformType type in platformTypes)
        {
            //if (!type.CanSpawnAtLevel(currentLevel)) 
            //    continue;

            if (platformSpawnCounts[type] >= GetScaledMaxCount(type))
                continue;

            if (IsInvalidPlatformNeighbour(type))
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return platformTypes.Find(p => p.isBaseCase) ?? platformTypes[0];

        float totalSpawnWeight = 0f;
        Dictionary<PlatformType, float> weightedChances = new();

        float normalizedTimeProgress = Mathf.Clamp01(Time.timeSinceLevelLoad / (15f * 60f)); // 15 min gameplay

        foreach (PlatformType type in validType)
        {
            float curveMultiplier = 1f;
            if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            {
                curveMultiplier = Mathf.Max(0.01f, type.spawnChanceCurve.Evaluate(normalizedTimeProgress));
            }

            float weightedChance = type.spawnWeight * curveMultiplier * Random.Range(0.95f, 1.05f);
            weightedChances[type] = weightedChance; 
            totalSpawnWeight += weightedChance;
        }

        if (totalSpawnWeight <= 0f)
            return platformTypes.Find(p => p.isBaseCase) ?? platformTypes[0];

        float randomPick = Random.value * totalSpawnWeight;
        float cumulative = 0;

        foreach (KeyValuePair<PlatformType, float> pair in weightedChances)
        {
            cumulative += pair.Value;
            if (randomPick <= cumulative)
                return pair.Key;
        }

        return validType[0];
    }

    private WallType WallTypeToSpawn()
    {
        List<WallType> validType = new();

        foreach (WallType type in wallTypes)
        {
            //if (!type.CanSpawnAtLevel(currentLevel))
            //    continue;

            if (wallSpawnCounts[type] >= GetScaledMaxCount(type))
                continue;

            if (IsInvalidWallNeighbour(type))
                continue;

            validType.Add(type);
        }

        if (validType.Count == 0)
            return wallTypes.Find(w => w.isBaseCase) ?? wallTypes[0]; // standard-pick


        float totalSpawnWeight = 0f;
        Dictionary<WallType, float> weightedChances = new();

        float normalizedTimeProgress = Mathf.Clamp01(Time.timeSinceLevelLoad / (15f * 60f));

        foreach (WallType type in validType)
        {
            float curveMultiplier = 1f;
            if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            {
                curveMultiplier = Mathf.Max(0.01f, type.spawnChanceCurve.Evaluate(normalizedTimeProgress));
            }

            float weightedChance = type.spawnWeight * curveMultiplier * Random.Range(0.8f, 1.2f);
            weightedChances[type] = weightedChance;
            totalSpawnWeight += weightedChance;
        }

        float pickRandomWall = Random.value * totalSpawnWeight;
        float cumulative = 0;

        foreach (KeyValuePair<WallType, float> pair in weightedChances)
        {
            cumulative += pair.Value;
            if (pickRandomWall <= cumulative)
                return pair.Key;
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

    private bool IsInvalidWallNeighbour(WallType next)
    {
        if (lastWallType != null && next.cannotHaveNeighbour.Contains(lastWallType.tag))
            return true;

        return false;
    }

    private int GetScaledMaxCount(PlatformType type)
    {
        float baseCount = type.baseMaxCount;
        float scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        //return Mathf.RoundToInt(baseCount * scale);
        int scaled = Mathf.RoundToInt(baseCount * scale);
        return Mathf.Min(scaled, maxPlatformsPerRun);
    }

    private int GetScaledMaxCount(WallType type)
    {
        float baseCount = type.baseMaxCount;
        float scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);
    }

    private float GetPlatformLength(GameObject prefab)
    {
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        if (r != null)
            return r.bounds.size.z;
        return prefab.transform.localScale.z;
    }

    private void SpawnEndPlatform()
    {
        Quaternion platformRotation = Quaternion.Euler(0f, 90f, 0f);
        Vector3 platformSpawnPos = transform.position + new Vector3(-0.577f, 0.917f, -0.24f); // magic numbers

        if (spawnedEndPlatform == null)
        {
            spawnedEndPlatform = Instantiate(endPlatformPrefab, platformSpawnPos, platformRotation);
        }
        else
        {
            spawnedEndPlatform.SetActive(true);
            spawnedEndPlatform.transform.position = platformSpawnPos;
            spawnedEndPlatform.transform.rotation = platformRotation;
        }
    }

    public void ResetKitchenGenerator(int newMaxPlatforms, int newLevel)
    {
        transform.position = startPosition;
        cameraMover.transform.position = cameraStartPosition;
        //Destroy(spawnedEndWall);

        if (spawnedEndPlatform != null)
            spawnedEndPlatform.SetActive(false);

        spawnedPlatforms = 0;
        isLevelComplete = false;
        maxPlatformsPerRun = newMaxPlatforms;
        currentLevel = newLevel;

        foreach (PlatformType key in
                 new List<PlatformType>(platformSpawnCounts
                     .Keys)) // copy of list due to otherwise trying to loop over list while modifying
            platformSpawnCounts[key] = 0;

        foreach (WallType key in new List<WallType>(wallSpawnCounts.Keys))
            wallSpawnCounts[key] = 0;

        lastPlatformType = null;
        secondLastPlatformType = null;
    }

    // access for difficultymanager to SO
    public List<PlatformType> GetPlatformTypes()
    {
        return platformTypes;
    }
}