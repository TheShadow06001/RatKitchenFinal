using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DifficultyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private KitchenGenerator kitchenGenerator;
    [FormerlySerializedAs("camera")] [SerializeField] private CameraScript newCamera;
    [SerializeField] private PlayerMovement player; 

    [SerializeField] private PlatformType sinkPlatform; // not needed?
    [SerializeField] private PlatformType ovenPlatform;// not needed?

    [Header("Current Level")] 
    [SerializeField] private int currentLevel = 1;
    //[SerializeField] private LevelSettings currentSettings; // manual override of level settings
    [SerializeField] private int basePlatformCount = 20;
    [SerializeField] private int platformsPerLevelIncrease = 5;
    [SerializeField] private float cameraSpeedMultiplier = 1.1f;
    [SerializeField] private float currentCameraSpeed;
    [SerializeField] private float maxSpeed = 2.5f;

    private Dictionary<PlatformType, int> runtimeMaxCounts = new();

    private int sinkBaseMaxCount; // not needed?
    private int ovenBaseMaxCount;// not needed?


    [Header("Scaling - currently not being used")]
    public bool useDynamicScaling = true;

    public float maxLevel = 10f; // time-based instead? or just set it very high?
    public static DifficultyManager Instance { get; private set; }

    public int CurrentLevel => currentLevel;
    public int CurrentMaxPlatforms { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        CurrentMaxPlatforms = basePlatformCount;

        sinkBaseMaxCount = sinkPlatform.baseMaxCount;   // not needed?
        ovenBaseMaxCount = ovenPlatform.baseMaxCount;   // not needed?
    }

    public event Action OnLevelReset;

    //public LevelSettings CurrentSettings => currentSettings;

    private void Start()
    {
        if (kitchenGenerator != null)
        {
            foreach (PlatformType t in kitchenGenerator.GetPlatformTypes())
            {
                runtimeMaxCounts[t] = t.baseMaxCount;
            }
        }
    }

    public int GetRuntimeMaxCount(PlatformType type)
    {
        if (runtimeMaxCounts != null && runtimeMaxCounts.TryGetValue(type, out int v))
            return v;

        return type.baseMaxCount;

    }

    public void LevelComplete()
    {
        currentLevel++;
        
        CurrentMaxPlatforms += platformsPerLevelIncrease;

        if (newCamera.moveSpeed < maxSpeed)
        {
            newCamera.moveSpeed *= cameraSpeedMultiplier;
            
            if (newCamera.moveSpeed > maxSpeed)
            {
                newCamera.moveSpeed = maxSpeed;
            }
            player.ChangeSpeed();
        }
        


        Debug.Log("Level" + currentLevel + "started, max platforms are now" + CurrentMaxPlatforms);

        OnLevelReset?.Invoke();

        if (kitchenGenerator != null)
        {
            foreach (PlatformType platformType in kitchenGenerator.GetPlatformTypes())
                if (platformType.typeOfPlatform == "Stove" || platformType.typeOfPlatform == "Sink")
                {
                    runtimeMaxCounts[platformType] = runtimeMaxCounts.GetValueOrDefault(platformType, platformType.baseMaxCount) + 1;
                    //sinkBaseMaxCount++;
                    //ovenBaseMaxCount++;
                }

            kitchenGenerator.ResetKitchenGenerator(CurrentMaxPlatforms, currentLevel);
        }
    }


    //public void SetLevel(int newLevel)
    //{
    //    currentLevel = newLevel;
    //}

    //public float GetNormalizedLevel()
    //{
    //    return Mathf.Clamp01(currentLevel / maxLevel);
    //}

    //public void NextLevel()
    //{
    //    SetLevel(currentLevel + 1);
    //}
}