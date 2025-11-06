using System;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Current Level")]
    [SerializeField] private int currentLevel = 1;
    //[SerializeField] private LevelSettings currentSettings; // manual override of level settings
    [SerializeField] private int basePlatformCount = 20;
    [SerializeField] private int platformsPerLevelIncrease = 5;
    [SerializeField] KitchenGenerator kitchenGenerator;


    [Header("Scaling - currently not being used")]
    public bool useDynamicScaling = true;
    public float maxLevel = 10f; // time-based instead? or just set it very high?

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        CurrentMaxPlatforms = basePlatformCount;
    }

    public event Action OnLevelReset;

    public int CurrentLevel => currentLevel;
    public int CurrentMaxPlatforms { get; private set; }

    //public LevelSettings CurrentSettings => currentSettings;

    public void LevelComplete()
    {
        currentLevel++;
        CurrentMaxPlatforms += platformsPerLevelIncrease;

        Debug.Log("Level" + currentLevel + "started, max platforms are now" + CurrentMaxPlatforms);

        OnLevelReset?.Invoke();

        if (kitchenGenerator != null)
        {
            foreach (var platformType in kitchenGenerator.GetPlatformTypes())
            {
                if (platformType.typeOfPlatform == "Stove" || platformType.typeOfPlatform == "Sink")
                {
                    platformType.baseMaxCount += 1;
                    platformType.MaxCountPerRun += 1;
                }
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
