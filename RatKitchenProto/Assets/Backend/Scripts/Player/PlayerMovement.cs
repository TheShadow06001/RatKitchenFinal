using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] public GameObject anchorPoint;

    [SerializeField] private float forwardAcceleration = 1f;
    [SerializeField] private float maxSpeedMultiplier = 2f;
    [SerializeField] private float verticalJumpForce = 5f;

    [SerializeField] private PlayerChangeLane laneChanger;

    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask groundLayer;
    private Animator animator;
    private float cameraSpeed;

    private RaycastHit hit;
    public bool isGrounded;

    private float moveSpeed;
    private Rigidbody rigidBody;

    Vector3 rayCastPosition;

    private void Start()
    {
        cameraSpeed = mainCamera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;
        rayCastPosition = transform.position + new Vector3(0, 0.1f, 0);

        laneChanger = laneChanger.GetComponent<PlayerChangeLane>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

    }


    public void PlayerUpdate()
    {
        HandleForwardSpeed();
        Jump();
        Vector3 currentPos = transform.position;
        //Debug.DrawRay(rayCastPosition, Vector3.down * rayLength, Color.red);

        float totalSpeed = moveSpeed;

        currentPos.z += totalSpeed * Time.deltaTime;
        transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);

        var count = animator.GetInteger("HurtCount");
        animator.SetInteger("HurtCount", count);
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Hurt");
            animator.SetInteger("HurtCount", count + 1);
        }
    }


    private void HandleForwardSpeed()
    {
        if (transform.position.z < anchorPoint.transform.position.z - 0.1f)
        {
            var targetSpeed1 = cameraSpeed * maxSpeedMultiplier;
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed1, Time.deltaTime * forwardAcceleration);
            animator.SetFloat("RunSpeed", moveSpeed / cameraSpeed);
        }
        else
        {
            moveSpeed = cameraSpeed;
            animator.SetFloat("RunSpeed", moveSpeed / cameraSpeed);
        }
    }

    private void Jump()
    {
        if (rigidBody != null && Input.GetKeyDown(KeyCode.Space))  //&& !laneChanger.isChangingLanes)
        {
            rayCastPosition = transform.position + new Vector3(0, 0.1f, 0);
            isGrounded = Physics.Raycast(rayCastPosition, Vector3.down, out RaycastHit hit1, rayLength, groundLayer);
            if (Physics.Raycast(rayCastPosition, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
            {
                Debug.Log($"HIT - IsGrounded: true | Collider: {hit.collider.name} | Tag: {hit.collider.tag} | Point: {hit.point:F3}");
                Debug.DrawLine(rayCastPosition, hit.point, Color.green, 0.5f);
            }
            else
            {
                Debug.Log("MISS - IsGrounded: false");
                Debug.DrawRay(rayCastPosition, Vector3.down * rayLength, Color.red, 0.5f);
            }
            if (isGrounded == true)
            {
                rigidBody.AddForce(Vector3.up * verticalJumpForce, ForceMode.Impulse);
                //rigidBody.AddForce(Vector3.forward * latteralJumpForce, ForceMode.Impulse);
                animator.SetTrigger("Jump");
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(rayCastPosition, Vector3.down * rayLength, Color.red);
    }
}