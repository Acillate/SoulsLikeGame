using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    //These values are going to be taken from the input manager
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 moveDir;
    private Vector3 targetRotationDir;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float rotationSpeed = 15;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }



    public void HandleAllMovement(){
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;

        // Clamp the movements
    }

    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();
        // our move direction is based on our cameras facing perspective and movement input
        moveDir = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDir = moveDir + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDir.Normalize();
        moveDir.y = 0;

        if(PlayerInputManager.instance.moveAmount > 0.5f)
        {
            // Move at a running speed
            player.characterController.Move(moveDir * runningSpeed * Time.deltaTime);
        }
        else if(PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            // Move at walking speed
            player.characterController.Move(moveDir * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation() 
    {
        //Rotate based on camera facing direction
        targetRotationDir = Vector3.zero;
        targetRotationDir = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDir = targetRotationDir + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDir.Normalize();
        targetRotationDir.y = 0;

        // if your not applying any rotation the rotation is where we are already facing
        if(targetRotationDir == Vector3.zero)
        {
            targetRotationDir = transform.forward;
        }

        // What ever direction we want to get we rotate to that direction
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDir);

        //A combo of where we are already facing and where we want to face
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
