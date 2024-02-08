using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    PlayerControls playerControls;
    [SerializeField] Vector2 movementInput;

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
        }
        playerControls.Enable();
    }

    private void OnDestroy() {
        //if we destroy this object unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
