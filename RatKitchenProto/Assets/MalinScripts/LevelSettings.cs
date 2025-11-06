using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Scriptable Objects/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    public int levelNumber;
    public float cameraSpeed;
    public float obstacleSpawner;
    public int maxPlatforms;
    public int maxSinks;
    public int maxStoves;
}
