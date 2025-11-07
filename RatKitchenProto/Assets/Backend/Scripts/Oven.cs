using UnityEngine;
using Random = UnityEngine.Random;

public class Oven : KitchenElements
{
    public GameObject fryingPan;
    public GameObject bigPan;
    private readonly float ovenHeigth = 1.1f;
    private Vector3 parentVectorPosition;


    private void OnEnable()
    {
        Generate();
    }

    private Vector3 CountPosition(float x, float z)
    {
        parentVectorPosition = transform.position;

        var CELL_WIDTH = x / 2;
        var CELL_HEIGHT = z / 2;

        var CENTER_OF_CELL_X = CELL_WIDTH / 2;
        var CENTER_OF_CELL_Z = CELL_WIDTH / 2;

        float randX = Random.Range(0, 2);
        float randY = Random.Range(0, 2);

        var finalX = randX * CELL_WIDTH + CENTER_OF_CELL_X;
        var finalZ = randY * CELL_HEIGHT + CENTER_OF_CELL_Z;
        var spawnPosition = new Vector3(finalX, ovenHeigth, finalZ) + parentVectorPosition;

        return spawnPosition;
    }

    private void Generate()
    {
        GameObject prefabToSpawn = null;
        var randomType = Random.Range(0, 2);

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
            var obstacle = Instantiate(prefabToSpawn, CountPosition(-0.78f, -0.57f), Quaternion.Euler(-90, 0, 0));
            obstacle.transform.SetParent(transform);
        }
    }
}