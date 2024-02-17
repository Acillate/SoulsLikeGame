using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    // CHANGE THESE NUMBERS TO TWEAK CAMERA PERFORMENCE
    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1.0f; //  THE BIGGER THIS NUMBER IS THE LONGER FOR THE CAMERA TO REACH THE TARGET AFTER EACH MOVEMENT
    [SerializeField] float leftAndRightRotationSpeed = 220f; // SPEED OF THE X AXIS ON RIGHT JOYSTICK
    [SerializeField] float upAndDownRotationSpeed = 220f; // SPEED OF THE Y AXIS ON THE RIGHT JOYSTICK
    [SerializeField] float minimumPivot = -30; // LOWEST POINT YOU ARE ABLE TO LOOK DOWN 
    [SerializeField] float maximumPivot = 60;  // HIGHEST POINT YOU ARE ABLE TO LOOK DOWN
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    // JUST DISPLAYS CAMERAS VALUES
    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; //USED FOR CAMERA COLLISIONS
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;
    private float cameraZPosition; //VALUE USED FOR CAMERA COLLISIONS
    private float targetCameraZPosition; //VALUE USED FOR CAMERA COLLISIONS

    private void Awake()
    {
        //There can only be one instance of this script at one time if another exists destroy it
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if(player != null)
        {
            // FOLLOW THE PLAYER
            HandleFollowTarget();
            // ROTATE AROUND THE PLAYER
            HandleRotations();
            // COLLIDE WITH OBJECTS
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        // IF LOCKED ON TO ENEMEY FORCE ROTATION TOWARDS ENEMEY
        // ELSE ROTATE NORMALLY

        //NORMAL ROTATIONS
        //ROTATE LEFT AND RIGHT BASED ON HORIZONTAL MOVEMENT ON THE RIGHT JOYSTICK
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        //ROTATE UP AND DOWN BASED ON VERTICAL MOVEMENT ON THE RIGHT JOYSTICK
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        //CLAMP THE UP AND DOWN LOOK ANGLE BETWEEN A MIN AND MAX VALUE
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        //ROTATE THIS GAMEOBJECT ON THE X AXIS
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        //ROTATE THE PIVOT GAMEOBJECT ON THE Y AXIS
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        /*
         * Checking to see if there is an object in front of us and if we are touching something in the camera position radius 
         * we push the camera back so we dont go through the object in front of us
         */
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //Check if there is a object in front of our camera
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            // if there is we get our distance from it
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            // we equate our target z position to the following 
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        //if our target position is less than our collision radius we subtract our collision radius 
        if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        // we then apply our final position using a lerp over a time of 0.2f
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
