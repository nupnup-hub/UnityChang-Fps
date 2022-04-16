using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingState : PlayerState
{
    private bool isDodge;
    private float timer;
    private float dodgeTime;
    public override void InputHandler(PlayerInput playerInput)
    {
        if (!isDodge)
        {
            if (!playerInput.condition["ViewChange"])
            {
                moveX = Input.GetAxis("Horizontal");
                moveZ = Input.GetAxis("Vertical");
                if (moveX != 0 || moveZ != 0)
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
    }

    public override void Enter(PlayerInput playerInput)
    {
        Debug.Log("dodge");
        if (playerInput.nowMovementState == PLAYER.RUN)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.RUNOFF, playerInput, null);
        }
        if (!isDodge)
        {
            timer = Time.time;
            isDodge = true;
            playerInput.CancleAiming();
            //줌 취소코드
        }

    }
    public override void UpdateMovement(PlayerInput playerInput)
    {
        if (Time.time - timer > dodgeTime)
        {
            isDodge = false;
        }
    }
    public DodgingState()
    {
        Debug.Log("닷지생성");
        playerMovement = PLAYER.DODGE;
        isDodge = false;
        dodgeTime = 0.8f;
    }
}
