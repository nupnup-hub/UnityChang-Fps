using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : PlayerState
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
                else
                    playerInput.state = walking;
            }
            else
                playerInput.state = standing;
        }
    }
    public override void Enter(PlayerInput playerInput)
    { 
        if (playerInput.condition["Aiming"])
            playerInput.CancleAiming();//줌 취소코드
        EventManager.Instance.PostNotification(EVENT_TYPE.RUN, playerInput, null);
        //call contorller
    }
    public override void UpdateMovement(PlayerInput playerInput)
    {
        playerInput.Move(moveX, moveZ , playerMovement);
        //if(playerInput.condition["ViewChange"])
            //playerInput.state = standing;
    }
    public RunningState()
    {
        Debug.Log("러닝생성");
        playerMovement = PLAYER.RUN;
    }
}
