using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Cinemachine;
using FishNet.Example.ColliderRollbacks;
using FishNet.Example.Scened;


//This is made by Bobsi Unity - Youtube
public class PlayerControl : NetworkBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float turnSpeed = 180f;
    public GameObject playerbody;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] AudioListener listener;

    private Animator anim;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;
            Debug.Log("hi Owner , camera priority : " + vc.Priority);
        }
        else
        {
            gameObject.GetComponent<PlayerControl>().enabled = false;
            listener.enabled = false;
            vc.Priority = 0;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();    
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool isRunning = false;

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

        //velocity
        var velocity = Vector3.forward * Input.GetAxis("Vertical")*walkingSpeed;

        anim.SetFloat("Speed",velocity.magnitude);

        float movementDirectionY = moveDirection.y;
        //movement direction 
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        //rotate the body
        playerbody.transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Escape the game
        if (Input.GetKey(KeyCode.P)){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        
        //vc
        if (canMove && vc != null)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            vc.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            
        }
    }
}