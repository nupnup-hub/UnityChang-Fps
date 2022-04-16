using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : PlayerState
{

    public override void InputHandler(PlayerInput playerInput)
    {
        if (!playerInput.condition["ViewChange"])
        {
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.Space))
                playerInput.state = dodging;
            else if (moveX != 0 || moveZ != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) && !playerInput.condition["Aiming"])
                    playerInput.state = running;
            }
            else
                playerInput.state = standing;
        }
    }
    public override void Enter(PlayerInput playerInput)
    {
        //Debug.Log("walk");
        if(playerInput.nowMovementState == PLAYER.RUN)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.RUNOFF, playerInput, null);
        }
        //call contorller
    }
    public override void UpdateMovement(PlayerInput playerInput)
    {
        playerInput.Move(moveX, moveZ, playerMovement);
       // if (playerInput.condition["ViewChange"])
          //  playerInput.state = standing;
    }
    public WalkingState()
    {
        Debug.Log("월킹생성");
        playerMovement = PLAYER.WALK;        
    }
}
