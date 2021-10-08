using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    private PlayerManager playerManager;

    public Transform target;
    
    private float movementSpeed;
    public float currentMovementSpeed;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform GroundDetector;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask rockMask;

    private GameObject audioParent;

    private AudioSource breatheAudio;
    private AudioSource movementAudio;

    [SerializeField] private AudioClip walkAudio;
    [SerializeField] private AudioClip jumpAudio;

    Vector3 velocity;
    bool isGrounded;

    CameraView cameraScript;

    private void Awake()
    {
        cameraScript = GetComponentInChildren<CameraView>();
        audioParent = GameObject.Find("PlayerAudio");
        breatheAudio = audioParent.transform.GetChild(0).GetComponent<AudioSource>();
        movementAudio = audioParent.transform.GetChild(1).GetComponent<AudioSource>();
        movementSpeed = currentMovementSpeed;
    }

    private void Start()
    {
        breatheAudio.Play();
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

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;

        

        if (Mathf.Abs(movement.sqrMagnitude) > 0 && isGrounded && velocity.y <= 0)
        {
            PlayAudio(walkAudio, true);
        }
        else if(Mathf.Abs(movement.sqrMagnitude) <= 0 && movementAudio.clip == walkAudio && movementAudio.isPlaying)
        {
            movementAudio.Stop();
        }

        controller.Move(movement * movementSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            movementAudio.Stop();
            PlayAudio(jumpAudio, false);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayAudio(AudioClip clip, bool loop)
    {
        if (movementAudio.clip != clip || !movementAudio.isPlaying)
        {
            movementAudio.clip = clip;
            movementAudio.loop = loop;
            movementAudio.Play();
        }
    }

    public void LookAt(Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.LookAt(targetPosition);
    }

}
