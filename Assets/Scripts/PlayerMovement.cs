using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public Transform target;
    
    private float movementSpeed;
    public float currentMovementSpeed;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform GroundDetector;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    CameraView cameraScript;

    private void Awake()
    {
        cameraScript = GetComponentInChildren<CameraView>();
    }

    private void Start()
    {
        movementSpeed = currentMovementSpeed;
        
    }

    public void DisablePlayerMovement()
    {
        movementSpeed = 0f;
    }
    
    public void EnablePlayerMovement()
    {
        movementSpeed = currentMovementSpeed;
    }

    public void Update()
    {
          isGrounded = Physics.CheckSphere(GroundDetector.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;

        controller.Move(movement * movementSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        Debug.Log(isGrounded);

               
    }

    public void LookAt(Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        transform.LookAt(targetPosition);
    }

}
