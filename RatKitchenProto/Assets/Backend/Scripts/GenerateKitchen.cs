using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateKitchen : MonoBehaviour
{
    [SerializeField] private GameObject sinkPrefab;
    [SerializeField] private GameObject ovenPrefab;
    [SerializeField] private GameObject[] counterPrefabs;
    public int maxSinks = 1;
    public int maxOvens = 1;
    public int amountOfElements = 10;

    private void Awake()
    {
        GenerateRandomKitchen();
    }

    private void GenerateRandomKitchen()
    {
        var firstElement = Instantiate(counterPrefabs[Random.Range(0, counterPrefabs.Length)]);
        firstElement.transform.SetParent(transform);
        for (var i = 1; i <= amountOfElements; i++)
        {
            var spawnPosition = new Vector3(i * -0.801f, 0, 0);

            var randomType = Random.Range(0, 3);
            GameObject prefabToSpawn = null;

            switch (randomType)
            {
                case 0:
                    if (maxSinks > 0)
                    {
                        prefabToSpawn = sinkPrefab;
                        maxSinks--;
                    }
                    else
                    {
                        prefabToSpawn = counterPrefabs[Random.Range(0, counterPrefabs.Length)];
                    }

                    break;
                case 1:
                    if (maxOvens > 0)
                    {
                        prefabToSpawn = ovenPrefab;
                        maxOvens--;
                    }
                    else
                    {
                        prefabToSpawn = counterPrefabs[Random.Range(0, counterPrefabs.Length)];
                    }

                    break;
                case 2:
                    prefabToSpawn = counterPrefabs[Random.Range(0, counterPrefabs.Length)];
                    break;
            }

            if (prefabToSpawn != null)
            {
                var clone = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                clone.transform.SetParent(transform);
            }
        }
    }
}