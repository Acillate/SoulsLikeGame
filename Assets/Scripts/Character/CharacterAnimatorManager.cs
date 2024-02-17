using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    float vertical;
    float horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue) 
    {
        //OPTION 1 
        character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);

        /*
        //OPTION 2 - SNAP if not snapped in player input but we already did that 
        float snappedHorizontal = 0;
        float snappedVertical = 0;

        #region Horizontal
        //This if chain will round the horizontal movement to -1, -0.5, 0, 0.5, 1
        if(horizontalValue > 0 && horizontalValue <= 0.5f) 
        {
            snappedHorizontal = 0.5f;
        }
        else if(horizontalValue > 0.5f && horizontalValue <= 1)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalValue < 0 && horizontalValue >= -0.5f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalValue < -0.5f && horizontalValue >= -1)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        #endregion

        #region Vertical
        //This if chain will round the vertical movement to -1, -0.5, 0, 0.5, 1

        if (verticalValue > 0 && verticalValue <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalValue > 0.5f && verticalValue <= 1)
        {
            snappedVertical = 1;
        }
        else if (verticalValue < 0 && verticalValue >= -0.5f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalValue < -0.5f && verticalValue >= -1)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        character.animator.SetFloat("Horizontal", snappedHorizontal);
        character.animator.SetFloat("Vertical", snappedVertical);

        #endregion
        */

    }
}
