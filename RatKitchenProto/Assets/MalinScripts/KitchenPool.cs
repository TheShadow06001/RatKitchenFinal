using System.Collections.Generic;
using UnityEngine;

public class KitchenPool : MonoBehaviour
{
    /* KITCHENPOOL WILL POOL BOTH PLATFORM TYPES AND WALL TYPES */

    public static KitchenPool Instance;

    [SerializeField] private int poolSize;
    public PlatformType[] kitchenPlatforms;
    public WallType[] wallTypes;
    private readonly List<PlatformType> loadedKitchenPlatforms = new();
    private readonly List<WallType> loadedWallTypes = new();
    private readonly Dictionary<PlatformType, Queue<GameObject>> platformDictionary = new();

    //test
    private readonly Dictionary<WallType, Queue<GameObject>> wallDictionary = new();


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        kitchenPlatforms = Resources.LoadAll<PlatformType>("KitchenPlatforms");

        wallTypes = Resources.LoadAll<WallType>("KitchenWalls");

        BuildPools(kitchenPlatforms, wallTypes);
    }

    private void BuildPools(PlatformType[] kitchenPlatformList, WallType[] wallTypeList)
    {
        platformDictionary.Clear();
        loadedKitchenPlatforms.Clear();
        loadedWallTypes.Clear();


        foreach (var type in kitchenPlatformList)
        {
            if (type == null || type.prefab == null)
            {
                Debug.LogWarning("Poolmanager: missing PlatformType or prefab for pool");
                continue;
            }

            if (platformDictionary.ContainsKey(type))
            {
                Debug.LogWarning($"Duplicate pool detected for {type.name}");
                continue;
            }

            loadedKitchenPlatforms.Add(type);

            Queue<GameObject> objectPool = new();

            for (var i = 0; i < poolSize; i++)
            {
                var obj = Instantiate(type.GetRandomPrefab());
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            platformDictionary.Add(type, objectPool);
        }

        foreach (var type in wallTypeList)
        {
            if (type == null || type.prefab == null)
            {
                Debug.LogWarning("Poolmanager: missing WallType or prefab for pool");
                continue;
            }

            if (wallDictionary.ContainsKey(type))
            {
                Debug.LogWarning($"Duplicate pool detected for {type.name}");
                continue;
            }

            loadedWallTypes.Add(type);

            Queue<GameObject> wallPool = new();

            for (var i = 0; i < poolSize; i++)
            {
                var obj = Instantiate(type.GetRandomPrefab());
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                wallPool.Enqueue(obj);
            }

            wallDictionary.Add(type, wallPool);
        }
    }

    public List<PlatformType> GetAllPlatformTypes()
    {
        return loadedKitchenPlatforms;
    }

    public List<WallType> GetAllWallTypes()
    {
        return loadedWallTypes;
    }

    /* PLATFORM POOL*/
    public GameObject GetPooledObject(PlatformType type, Vector3 position, Quaternion rotation)
    {
        if (type == null || !platformDictionary.ContainsKey(type))
            return null;

        var objectPool = platformDictionary[type];
        GameObject obj = null;

        foreach (var pooledObj in objectPool)
        {
            if (!pooledObj.activeSelf)
            {
                obj = pooledObj;
                break;
            }
        }

        if (obj == null)
        {
            Debug.Log("No platform in pool, instantiating...");
            obj = Instantiate(type.GetRandomPrefab());
            obj.transform.SetParent(transform);
            objectPool.Enqueue(obj);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnToPool(PlatformType type, GameObject obj)
    {
        if (type == null || !platformDictionary.ContainsKey(type))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        if (!platformDictionary[type].Contains(obj))
        platformDictionary[type].Enqueue(obj);
    }

    /* WALL POOL*/
    public GameObject GetPooledWall(WallType type, Vector3 position, Quaternion rotation)
    {
        if (type == null || !wallDictionary.ContainsKey(type))
            return null;

        var wallPool = wallDictionary[type];
        GameObject obj = null;

        foreach (var pooledObj in wallPool)
        {
            if (!pooledObj.activeSelf)
            {
                obj = pooledObj;
                break;
            }
        }

        if (obj == null)
        {
            Debug.Log("No platform in pool, instantiating...");
            obj = Instantiate(type.GetRandomPrefab());
            obj.transform.SetParent(transform);
            wallPool.Enqueue(obj);
        }

        //if (wallPool.Count > 0)
        //{
        //    obj = wallPool.Dequeue();
        //}
        //else
        //{
        //    obj = Instantiate(type.GetRandomPrefab());
        //    obj.transform.SetParent(transform);
        //}

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnWallToPool(WallType type, GameObject obj)
    {
        if (type == null || !wallDictionary.ContainsKey(type))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        wallDictionary[type].Enqueue(obj);
    }
}