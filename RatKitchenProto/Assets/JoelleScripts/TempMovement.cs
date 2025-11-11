using UnityEngine;

public class TempMovement : MonoBehaviour
{
    private void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * Time.deltaTime;
    }
}