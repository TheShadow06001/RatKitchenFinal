using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Camera Dependencies")]
    public GameObject player;
    public PlayerMovement playerScript;
    [Header("Camera Movement Settings")]
    public float moveSpeed = 5f;
    public Vector3 direction = Vector3.forward;
    [Header("Camera Chase Settings")] 
    [SerializeField] private float ChaseSpeed;
    private float ChaseThreshold;
    public float ChaseCooldownTime = 2f;
    private bool CanChase;
    private bool IsChasing;

    private void Start()
    {
        ChaseThreshold = transform.position.z - player.transform.position.z;
    }

    public void UpdateCamera()
    {
        transform.position += direction * moveSpeed * Time.deltaTime; //Camera Consistant Movement
        StartCoroutine(CameraChasePlayer());                   //Camera Chase Routine
    }

    public IEnumerator CameraChasePlayer()
    {
        while (true)
        {
            if (!playerScript.isGrounded) //Check if player has been grounded for X amount of time.
            {
                CanChase = false;
                yield return new WaitForSeconds(ChaseCooldownTime);
                CanChase = true;
            }

            if (CanChase)
            {
                if (player.transform.position.z + ChaseThreshold > transform.position.z) //Is Player ahead of Camera?
                {
                    IsChasing = true;
                    Vector3 TargetPosition = transform.position;
                    TargetPosition.z = player.transform.position.z + ChaseThreshold;
                    
                    //Smooth Chase Movement
                    transform.position = Vector3.Lerp(transform.position, TargetPosition, ChaseSpeed * Time.deltaTime);
                }
            } yield return null; 
        }
    }
}