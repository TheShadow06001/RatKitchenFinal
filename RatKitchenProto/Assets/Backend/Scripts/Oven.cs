using UnityEngine;
using Random = UnityEngine.Random;

public class Oven : KitchenElements
{
    public GameObject fryingPan;
    public GameObject bigPan;
    float ovenHeigth = 1.1f;
    Vector3 parentVectorPosition;


    private void OnEnable()
    {
        Generate();
    }

    private Vector3 CountPosition(float x, float z)
    {
        parentVectorPosition = transform.position;

        float CELL_WIDTH = x / 2;
        float CELL_HEIGHT = z / 2;

        float CENTER_OF_CELL_X = CELL_WIDTH / 2;
        float CENTER_OF_CELL_Z = CELL_WIDTH / 2;

        float randX = Random.Range(0, 2);
        float randY = Random.Range(0, 2);

        float finalX = randX * CELL_WIDTH + CENTER_OF_CELL_X;
        float finalZ = randY * CELL_HEIGHT + CENTER_OF_CELL_Z;
        Vector3 spawnPosition = new Vector3(finalX, ovenHeigth, finalZ) + parentVectorPosition;

        return spawnPosition;
    }
    private void Generate()
    {
        GameObject prefabToSpawn = null;
        int randomType = Random.Range(0, 2);

        switch (randomType)
        {
            case 0:
                prefabToSpawn = fryingPan;
                break;
            case 1:
                prefabToSpawn = bigPan;
                break;
        }


        if (prefabToSpawn != null)
        {
            GameObject obstacle = Instantiate(prefabToSpawn, CountPosition(-0.78f, -0.57f), Quaternion.Euler(-90, 0, 0));
            obstacle.transform.SetParent(transform);
        }
    }
}
