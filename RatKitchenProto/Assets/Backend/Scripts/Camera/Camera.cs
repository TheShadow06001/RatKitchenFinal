using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 direction = Vector3.forward;

    private void Start()
    {
    }

    // Update is called once per frame
    public void UpdateCamera()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}