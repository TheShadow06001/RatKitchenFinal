using UnityEngine;

public class TempMovement : MonoBehaviour
{
    private float speed = 2f;

    // Update is called once per frame
    private void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * Time.deltaTime;
    }
}