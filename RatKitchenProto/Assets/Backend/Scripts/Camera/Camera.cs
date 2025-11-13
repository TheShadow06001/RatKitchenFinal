using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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

    [Header("Player Out of Bounds Check")]
    [SerializeField] private Transform playerAnchor;
    [SerializeField] private bool enableOutOfViewCheck = true;
    [FormerlySerializedAs("camera")] [SerializeField] private Camera newCamera;

    private void Start()
    {
        ChaseThreshold = transform.position.z - player.transform.position.z;
    }

    public void UpdateCamera()
    {
        transform.position += direction * moveSpeed * Time.deltaTime; //Camera Consistant Movement
        //StartCoroutine(CameraChasePlayer());                   //Camera Chase Routine

        //if (enableOutOfViewCheck)
        //    CheckPlayerOutOfView();
    }

    //private void CheckPlayerOutOfView()
    //{
    //    if (!GameManager.Instance.CheckState<PlayingState>()) return;

    //    if (player == null || camera == null)
    //        return;

    //    Vector3 viewportPosition = camera.WorldToViewportPoint(player.transform.position);
    //    bool isOutOfView = viewportPosition.x < 0f || viewportPosition.x > 1f || viewportPosition.y < 0f || viewportPosition.y > 1f || viewportPosition.z < 0f;

    //    float zDiff = camera.transform.position.z - player.transform.position.z;
    //    bool tooFarBehind = zDiff > 3f;

    //    if (isOutOfView )
    //        HandlePlayerOutOfView();
    //}

    //private void HandlePlayerOutOfView()
    //{
    //    Debug.Log("Player is out of view, losing a life and respawning");
        
    //    HeartDisplay.instance.TakeDamage();

    //    //Rigidbody rb = player.GetComponent<Rigidbody>();
    //    //rb.linearVelocity = Vector3.zero;
    //    //rb.angularVelocity = Vector3.zero;

    //    player.transform.position = playerAnchor.position;
    //}

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
                    Vector3 TargetPosition = transform.position;
                    TargetPosition.z = player.transform.position.z + ChaseThreshold;
                    
                    //Smooth Chase Movement
                    transform.position = Vector3.Lerp(transform.position, TargetPosition, ChaseSpeed * Time.deltaTime);
                }
            } yield return null; 
        }
    }
}