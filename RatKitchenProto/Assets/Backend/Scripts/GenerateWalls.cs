using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateWalls : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject windowedWallPrefab;
    public int maxWindows = 1;
    public int amountOfElements = 5;

    private void Awake()
    {
        GenerateRandomWalls();
    }

    private void GenerateRandomWalls()
    {
        for (var i = 0; i <= amountOfElements; i++)
        {
            var spawnPosition = new Vector3(i * -4f, 0, -0.93f);

            var randomType = Random.Range(0, 2);
            GameObject prefabToSpawn = null;

            switch (randomType)
            {
                case 0:
                    if (maxWindows > 0)
                    {
                        prefabToSpawn = windowedWallPrefab;
                        maxWindows--;
                    }
                    else
                    {
                        prefabToSpawn = wallPrefab;
                    }

                    break;
                case 1:
                    prefabToSpawn = wallPrefab;
                    break;
            }

            if (prefabToSpawn != null)
            {
                var clone = Instantiate(prefabToSpawn, spawnPosition, Quaternion.Euler(0, 90, 0));
                clone.transform.SetParent(transform);
            }
        }
    }
}