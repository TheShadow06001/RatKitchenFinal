using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject platformDestructionPoint;
    private PooledPlatform pooledPlatform;
    private PooledWall pooledWall;

    private void Start()
    {
        platformDestructionPoint = GameObject.Find("DestructionPoint");

        pooledPlatform = GetComponent<PooledPlatform>();
        pooledWall = GetComponent<PooledWall>();

        DifficultyManager.Instance.OnLevelReset += ReturnToPoolOnReset;
    }

    private void Update()
    {
        if (transform.position.z < platformDestructionPoint.transform.position.z)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (pooledPlatform !=null)
        {
            KitchenPool.Instance.ReturnToPool(pooledPlatform.platformType, gameObject);
        }

        if (pooledWall != null)
        {
            KitchenPool.Instance.ReturnWallToPool(pooledWall.wallType, gameObject);
        }
    }

    private void ReturnToPoolOnReset()
    {
        if (gameObject.activeInHierarchy)
            ReturnToPool();
    }

    private void OnDestroy()
    {
        if(DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.OnLevelReset -= ReturnToPoolOnReset;
        }
    }
}
