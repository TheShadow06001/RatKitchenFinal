using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereMovement : MonoBehaviour
{
    [Header("Movement Settings")] public float moveSpeed = 10f;

    public float jumpForce = 5f;

    [Header("Ground Check")] public Transform groundCheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        var moveX = Input.GetAxis("Horizontal");
        var moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0f, moveZ);


        if (Input.GetButtonDown("Jump") && isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        var force = moveDirection * moveSpeed;
        rb.AddForce(force);
    }
}