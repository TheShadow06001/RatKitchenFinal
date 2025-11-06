using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    /* SHOULD PROBABLY SPLIT THIS INTO SEPARATE SCRIPTS FOR WALL, PLATFORM AND A HANDLER */

    [SerializeField] private List<PlatformType> platformTypes = new();
    [SerializeField] private List<WallType> wallTypes = new();

    Vector3 wallOffset;

    [SerializeField] private Transform generationPoint;
    [SerializeField] private float distanceBetween = 0f;
    [SerializeField] public int maxPlatformsPerRun = 20;
    [SerializeField] private int basePackagesPerRun = 3; // 1 wall fits 5 counters = 1 package

    [SerializeField] private int currentLevel = 1;

    [SerializeField] private GameObject endPlatformPrefab;
    [SerializeField] private GameObject endWallPrefab;
    private GameObject spawnedEndWall;

    private Dictionary<PlatformType, int> platformSpawnCounts = new();
    private Dictionary<WallType, int> wallSpawnCounts = new();
    private Dictionary<PlatformType, float> platformLengths = new();

    [SerializeField] private int spawnedPlatforms = 0;
    private bool isLevelComplete = false;

    private PlatformType lastPlatformType;
    private PlatformType secondLastPlatformType;

    private WallType lastWallType;
    private WallType secondLastWallType;

    public GameObject cameraMover;
    Vector3 cameraStartPosition;

    //tillfällig start position
    [SerializeField] Vector3 startPosition;
    private bool canGenerate = true;

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


        foreach (var type in platformTypes)
        {
            platformSpawnCounts[type] = 0;
        }

        foreach (var type in wallTypes)
        {
            wallSpawnCounts[type] = 0;
        }

        // hämta bredden på platforms här? spara i en lista/array?
        foreach (var type in platformTypes)
        {
            platformSpawnCounts[type] = 0;
            platformLengths[type] = GetPlatformLength(type.prefab);
        }

    }

    // When game manager is used, rename to UpdateKitchenGenerator and call on ResetKitchenGenerator?
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
            
            wallOffset = new Vector3(-0.87f, -0.09f, 0);     // magic numbers (x = -0,94f)
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

                Vector3 platformPos = wallSpawnPosition + new Vector3(chosenPlatform.xPositionSpawnOffset, 0, (i * platformSpacing));
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
        float totalSpawnWeight = 0f;
        Dictionary<PlatformType,float> weightedChances = new();

        foreach (var type in validType)
        {
            float curveMultiplier = 1f;
            //if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            //{
            //    curveMultiplier = Mathf.Clamp01(type.spawnChanceCurve.Evaluate(normalizedLevel));
            //}

            float weightedChance = type.spawnWeight * curveMultiplier;
            weightedChances[type] = weightedChance;
            totalSpawnWeight += weightedChance;

            //totalSpawnWeight += type.spawnWeight;
        }

        float randomPick = Random.value * totalSpawnWeight;
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
        float totalSpawnWeight = 0f;
        Dictionary<WallType, float> weightedChances = new();

        foreach (var type in validType)
        {
            float curveMultiplier = 1f;
            //if (type.spawnChanceCurve != null && type.spawnChanceCurve.length > 0)
            //{
            //    curveMultiplier = Mathf.Clamp01(type.spawnChanceCurve.Evaluate(normalizedLevel));
            //}

            float weightedChance = type.spawnWeight * curveMultiplier;
            weightedChances[type] = weightedChance;
            totalSpawnWeight += weightedChance;

            //totalSpawnWeight += type.spawnWeight;
        }

        float pickRandomWall = Random.value * totalSpawnWeight;
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

        if (next.mustHaveCounterBetween && (lastPlatformType?.tag == next.tag || secondLastPlatformType?.tag == next.tag))
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
        int baseCount = type.baseMaxCount;
        float scale = Mathf.Pow(type.maxCountMultiplierPerLevel, currentLevel);
        return Mathf.RoundToInt(baseCount * scale);
    }

    private int GetScaledMaxCount(WallType type)
    {
        int baseCount = type.baseMaxCount;
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
         if (endWallPrefab)
         {
            Quaternion ratWallRotation = Quaternion.Euler(0f, 270f, 0f); 
            Vector3 wallSpawnPos = transform.position + new Vector3(-0.8f, -0.09f, 0.2f);    // magic numbers
            spawnedEndWall = Instantiate(endWallPrefab, wallSpawnPos, ratWallRotation);
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

        foreach (var key in new List<PlatformType> (platformSpawnCounts.Keys)) // copy of list due to otherwise trying to loop over list while modifying
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
