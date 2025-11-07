using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject anchorPoint;

    [SerializeField] private float forwardAcceleration = 1f;
    [SerializeField] private float maxSpeedMultiplier = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private PlayerChangeLane laneChanger;

    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask groundLayer;
    private Animator animator;
    private float cameraSpeed;

    private RaycastHit hit;
    private bool isGrounded;

    private float moveSpeed;
    private Rigidbody rigidBody;

    private void Start()
    {
        cameraSpeed = mainCamera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;

        laneChanger = laneChanger.GetComponent<PlayerChangeLane>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);
    }

    public void PlayerUpdate()
    {
        HandleForwardSpeed();
        Jump();
        var currentPos = transform.position;

        var totalSpeed = moveSpeed;

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
        if (rigidBody != null && Input.GetKeyDown(KeyCode.Space) && !laneChanger.isChangingLanes)
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundLayer);
            if (isGrounded)
            {
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                animator.SetTrigger("Jump");
            }
        }
    }
}