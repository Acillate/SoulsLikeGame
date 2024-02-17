using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;
    PlayerControls playerControls;

    //LEFT JOYSTICK VARIABLES
    [Header("MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    //RIGHT JOYSTICK VARIABLES
    [Header("CAMERA INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    private void Awake() {
        //There can only be one instance of this script at one time if another exists destroy it
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start() {
        instance.enabled = false;
        // If the scene changes run this logic
        SceneManager.activeSceneChanged += OnSceneChange;
        DontDestroyOnLoad(gameObject);
    }
    private void OnSceneChange(Scene oldScene, Scene newScene){
        // this is so our player cant move around if enter things like the character creation menu
        // if we are loading into our world scene enable our player controls
        if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex()){
            instance.enabled = true;
        }
        // otherwise we must be at the main menu disable our player controls
        else{
            instance.enabled = false;
        }
    }
    private void OnEnable() {

        if(playerControls == null){
            playerControls = new PlayerControls();
            //When ever the joystick is moved take that input value and give it to Vector2 movementInput
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
        }
        playerControls.Enable();
    }

    private void OnDestroy() {
        //if we destroy this object unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // if we minimize or lower the window stop adjusting inputs
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // clamps the number between 0-1
        // Returns the absolute number (meaning number that is alwasy positive)
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));


        //We clamp the values so they are 0, 0.5, or 1
        if(moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5 && moveAmount <= 1) 
        {
            moveAmount = 1;
        }

        //0 because horiztonal is for strafe movement we dont want strafe movement unless we are locked on to a enemey

        if (player == null) return;
        //if we are not locked on only use the move amount
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);

        //if we are locked on pass the horiztonal movement as well
    }

    private void HandleCameraMovementInput() 
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }
}
