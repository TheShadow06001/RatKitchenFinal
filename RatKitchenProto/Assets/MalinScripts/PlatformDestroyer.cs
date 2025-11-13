using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject platformDestructionPoint;
    public GameObject wallDestructionPoint;
    private PooledPlatform pooledPlatform;
    private PooledWall pooledWall;

    private bool justSpawned = true;

    private void Start()
    {
        platformDestructionPoint = GameObject.Find("DestructionPoint");
        wallDestructionPoint = GameObject.Find("WallDestructionPoint");

        pooledPlatform = GetComponent<PooledPlatform>();
        pooledWall = GetComponent<PooledWall>();

        DifficultyManager.Instance.OnLevelReset += ReturnToPoolOnReset;
        Invoke(nameof(ResetSpawnFlag), 0.3f);
    }

    private void ResetSpawnFlag()
    {
        justSpawned = false;
    }

    private void Update()
    {
        if (justSpawned)
            return;

        if (pooledPlatform != null && platformDestructionPoint != null)
        {
            if (transform.position.z < platformDestructionPoint.transform.position.z)
                ReturnToPool();
        }
        else if (pooledWall != null && wallDestructionPoint != null)
        {
            if (transform.position.z < wallDestructionPoint.transform.position.z)
                ReturnToPool();
        }
    }

    private void OnDestroy()
    {
        if (DifficultyManager.Instance != null) 
            DifficultyManager.Instance.OnLevelReset -= ReturnToPoolOnReset;
    }

    private void ReturnToPool()
    {
        if (pooledPlatform != null) 
            KitchenPool.Instance.ReturnToPool(pooledPlatform.platformType, gameObject);

        if (pooledWall != null) 
            KitchenPool.Instance.ReturnWallToPool(pooledWall.wallType, gameObject);
    }

    private void ReturnToPoolOnReset()
    {
        if (gameObject.activeInHierarchy)
            ReturnToPool();
    }
}