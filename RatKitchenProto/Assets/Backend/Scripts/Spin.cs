using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}