using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();

        // Do more stuff only for player
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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
}
