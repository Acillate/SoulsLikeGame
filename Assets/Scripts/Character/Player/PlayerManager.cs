using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();

        // Do more stuff only for player
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    protected override void Update()
    {
        base.Update();

        // if we do not own this gameobject we dont not control or edit it
        if (!IsOwner)
        {
            return;
        }
        //Handle Player Movement
        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner) { return; }
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //If this is the player object owned by this client
        if(IsOwner) 
        { 
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
        }
    }
}
