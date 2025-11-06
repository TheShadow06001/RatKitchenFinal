using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject anchorPoint;

    [SerializeField] float forwardAcceleration = 1f;
    [SerializeField] float maxSpeedMultiplier = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] PlayerChangeLane laneChanger;

    [SerializeField] float rayLength = 1f;
    [SerializeField] LayerMask groundLayer;

    float moveSpeed;
    float cameraSpeed;

    RaycastHit hit;
    bool isGrounded;
    Rigidbody rigidBody;
    Animator animator;
    void Start()
    {
        cameraSpeed = mainCamera.GetComponent<CameraScript>().moveSpeed;
        moveSpeed = cameraSpeed;

        laneChanger = laneChanger.GetComponent<PlayerChangeLane>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayerUpdate()
    {
        HandleForwardSpeed();
        Jump();
        Vector3 currentPos = transform.position;

        float totalSpeed = moveSpeed;

        currentPos.z += totalSpeed * Time.deltaTime;
        transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z);

        int count = animator.GetInteger("HurtCount");
        animator.SetInteger("HurtCount", count);
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Hurt");
            animator.SetInteger("HurtCount", count + 1);
        }
    }



    void HandleForwardSpeed()
    {
        if (transform.position.z < anchorPoint.transform.position.z - 0.1f)
        {
            float targetSpeed1 = cameraSpeed * maxSpeedMultiplier;
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed1, Time.deltaTime * forwardAcceleration);
            animator.SetFloat("RunSpeed", moveSpeed / cameraSpeed);
        }
        else
        {
            moveSpeed = cameraSpeed;
            animator.SetFloat("RunSpeed", moveSpeed / cameraSpeed);
        }
    }
    void Jump()
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
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);
    }
}
