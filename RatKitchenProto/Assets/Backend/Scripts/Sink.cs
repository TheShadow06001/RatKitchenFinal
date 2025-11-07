using UnityEngine;

public class Sink : KitchenElements
{
    public GameObject plate;
    private float sinkHeigth = 17.5f;
    private Vector3 sinkPosition;


    private void OnEnable()
    {
        GameObject plateOnScene = Instantiate(plate, CountPosition(-0.7f, sinkHeigth * 0.05f, -0.2f), Quaternion.Euler(-90, 0, 0));
        plateOnScene.transform.SetParent(transform);
    }

    private Vector3 CountPosition(float x, float y, float z)
    {
        sinkPosition = transform.position;

        var CELL_WIDTH = x / 2;
        var CELL_HEIGHT = z / 2;

        var CENTER_OF_CELL_X = CELL_WIDTH / 2;
        var CENTER_OF_CELL_Z = CELL_WIDTH / 2;

        float randX = Random.Range(0, 2);
        float randY = Random.Range(0, 2);

        var finalX = randX * CELL_WIDTH + CENTER_OF_CELL_X;
        var finalZ = randY * CELL_HEIGHT + CENTER_OF_CELL_Z;
        var spawnPosition = new Vector3(finalX, y, finalZ) + sinkPosition;

        return spawnPosition;
    }
}