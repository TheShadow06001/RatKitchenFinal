using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 direction = Vector3.forward;

    public GameObject player;
    public GameObject anchorPoint;


    private void Start()
    {

    }

    public void UpdateCamera()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}