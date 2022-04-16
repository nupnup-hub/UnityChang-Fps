using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingState : PlayerState
{
    public override void InputHandler(PlayerInput playerInput)
    {
        //카메라 시점 변환중에는 상태변경 불가
        if (!playerInput.condition["ViewChange"])
        {
            // 방향키에 따른 값 가져옴
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
            //스페이스바 누를시 회피 (회피 미구현)
            if (Input.GetKey(KeyCode.Space))
                playerInput.state = dodging;
            // 방향키를 눌럿을시
            else if (moveX != 0 || moveZ != 0)
            {
                // 쉬프트를 눌렀으며 조준중이 아닐시
                if (Input.GetKey(KeyCode.LeftShift) && !playerInput.condition["Aiming"])
                    playerInput.state = running;
                else
                    playerInput.state = walking;
            }
        }
    }
    public override void Enter(PlayerInput playerInput)
    {
        //Debug.Log("stand");
        //이전 상태가 RUN일시 카메라 위치 변경을 위해 이벤트매니저에 통보
        if (playerInput.nowMovementState == PLAYER.RUN)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.RUNOFF, playerInput, null);
        }     
    }
    
    public StandingState()
    {
        Debug.Log("스탠드생성");
        playerMovement = PLAYER.STAND; 
    }
}
