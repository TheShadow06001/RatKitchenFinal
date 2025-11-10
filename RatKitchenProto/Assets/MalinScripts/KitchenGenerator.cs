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
    [SerializeField] private int basePackagesPerRun = 3; // 1 wall fits 5 counters = 1 package

    [SerializeField] private int currentLevel = 1;

    [SerializeField] private GameObject endPlatformPrefab;
    [SerializeField] private GameObject endWallPrefab;

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
        //var settings = DifficultyManager.Instance.CurrentSettings;
        //if (settings != null)
        //{
        //    currentLevel = settings.levelNumber;
        //    maxPlatformsPerRun = settings.maxPlatforms;
        //}   


        foreach (var type in platformTypes) platformSpawnCounts[type] = 0;

        foreach (var type in wallTypes) wallSpawnCounts[type] = 0;

        // h�mta bredden p� platforms h�r? spara i en lista/array?
        foreach (var type in platformTypes)
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
            var chosenWall = WallTypeToSpawn();
            if (chosenWall == null)
                return;

            var wallSpawnPosition = transform.position;

            wallOffset = new Vector3(-0.87f, -0.09f, 0); // magic numbers (x = -0,94f)
            var wallRotation = Quaternion.Euler(0f, 180f, 0f);
            var newWall = KitchenPool.Instance.GetPooledWall(chosenWall, wallSpawnPosition + wallOffset, wallRotation);

            newWall.SetActive(true);
            wallSpawnCounts[chosenWall]++;

            /* PLATFORM SPAWN */
            var platformSpacing = platformLengths[platformTypes[0]] + distanceBetween;
            var packageLength = chosenWall.platformsPerWall * platformSpacing;

            for (var i = 0; i < chosenWall.platformsPerWall; i++)
            {
                var chosenPlatform = PlatformTypeToSpawn();
                if (chosenPlatform == null)
                    return;

                var platformPos = wallSpawnPosition +
                                  new Vector3(chosenPlatform.xPositionSpawnOffset, 0, i * platformSpacing);
                var platformRotation = Quaternion.Euler(0f, 90f, 0f);

                var newPlatform = KitchenPool.Instance.GetPooledObject(chosenPlatform, platformPos, platformRotation);
                newPlatform.SetActive(true);

                platformSpawnCounts[chosenPlatform]++;
                secondLastPlatformType = lastPlatformType;
                lastPlatformType = chosenPlatform;

                spawnedPlatforms++;
            }

            secondLastWallType = lastWallType;
            lastWallType = chosenWall;

            //transform.position = wallSpawnPosition + new Vector3(/*chosenWall.platformsPerWall * platformSpacing*/0, 0, chosenWall.platformsPerWall * platformSpacing);
            transform.position = wallSpawnPosition + new Vector3(0, 0, packageLength);
        }
    }

    private PlatformType PlatformTypeToSpawn()
    {
        List<PlatformType> validType = new();

        foreach (var type in platformTypes)
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

        //weighted random algorithm
        //float normalizedLevel = DifficultyManager.Instance.GetNormalizedLevel();
        var totalSpawnWeight = 0f;
        Dictionary<PlatformType, float> weightedChances = new();

        foreach (var type in validType)
        {
            var curveMultiplier = 1f;
            //if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            //{
            //    curveMultiplier = Mathf.Clamp01(type.spawnChanceCurve.Evaluate(normalizedLevel));
            //}

            var weightedChance = type.spawnWeight * curveMultiplier;
            weightedChances[type] = weightedChance;
            totalSpawnWeight += weightedChance;

            //totalSpawnWeight += type.spawnWeight;
        }

        var randomPick = Random.value * totalSpawnWeight;
        float cumulative = 0;

        /*foreach (var type in validType)
        {
            cumulative += type.spawnWeight;
            if (randomPick <= cumulative)
                return type;
        }*/

        foreach (var pair in weightedChances)
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
        //float normalizedLevel = DifficultyManager.Instance.GetNormalizedLevel();
        var totalSpawnWeight = 0f;
        Dictionary<WallType, float> weightedChances = new();

        foreach (var type in validType)
        {
            var curveMultiplier = 1f;
            //if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            //{
            //    curveMultiplier = Mathf.Clamp01(type.spawnChanceCurve.Evaluate(normalizedLevel));
            //}

            var weightedChance = type.spawnWeight * curveMultiplier;
            weightedChances[type] = weightedChance;
            totalSpawnWeight += weightedChance;

            //totalSpawnWeight += type.spawnWeight;
        }

        var pickRandomWall = Random.value * totalSpawnWeight;
        float cumulative = 0;

        /* foreach (var type in validType)
         {
             cumulative += type.spawnWeight;
             if (pickRandomWall <= cumulative)
                 return type;
         }*/
        foreach (var pair in weightedChances)
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
        var baseCount = type.baseMaxCount;
        var scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);

    }

    private int GetScaledMaxCount(WallType type)
    {
        var baseCount = type.baseMaxCount;
        var scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);
    }

    private float GetPlatformLength(GameObject prefab)
    {
        var r = prefab.GetComponentInChildren<Renderer>();
        if (r != null)
            return r.bounds.size.z;
        return prefab.transform.localScale.z;
    }

    private void SpawnEndPlatform()
    {
        //if (endWallPrefab)
        //{
        //    var ratWallRotation = Quaternion.Euler(0f, 270f, 0f);
        //    var wallSpawnPos = transform.position + new Vector3(-0.8f, -0.09f, 0.2f); // magic numbers
        //    spawnedEndWall = Instantiate(endWallPrefab, wallSpawnPos, ratWallRotation);
        //}        
        
        if (endPlatformPrefab)
        {
            Quaternion platformRotation = Quaternion.Euler(0f, 90f, 0f);
            Vector3 platformSpawnPos = transform.position + new Vector3(-0.577f, 0.917f, -0.24f); // magic numbers
            spawnedEndPlatform = Instantiate(endPlatformPrefab, platformSpawnPos, platformRotation);
        }

    }

    public void SetDifficulty(LevelSettings settings)
    {
        maxPlatformsPerRun = settings.maxPlatforms;
    }

    public void ResetKitchenGenerator(int newMaxPlatforms, int newLevel)
    {
        transform.position = startPosition;
        cameraMover.transform.position = cameraStartPosition;
        Destroy(spawnedEndWall);

        spawnedPlatforms = 0;
        isLevelComplete = false;
        maxPlatformsPerRun = newMaxPlatforms;
        currentLevel = newLevel;

        foreach (var key in
                 new List<PlatformType>(platformSpawnCounts
                     .Keys)) // copy of list due to otherwise trying to loop over list while modifying
            platformSpawnCounts[key] = 0;

        foreach (var key in new List<WallType>(wallSpawnCounts.Keys))
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