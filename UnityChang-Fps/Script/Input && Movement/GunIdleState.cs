using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunIdleState : GunState
{
    float viewChangeTime, nowTime;
    bool wait;
    public override void InputHandler(PlayerInput playerInput)
    {
        if(!wait)
        {
            Debug.Log("초이스");
            /*if (Input.GetMouseButton(1) && Input.GetMouseButton(0))
                playerInput.gunState = fire;
            else if (Input.GetKey(KeyCode.R))
                playerInput.gunState = reload;
            else if (Input.GetMouseButton(1))
                ;
            else
                playerInput.gunState = emptyGun;*/
            if (Input.GetMouseButton(1))
                ;
            else
                playerInput.gunState = emptyGun;
        }
    }
    public override void Enter(PlayerInput playerInput)
    {
        Debug.Log("줌 온");
        nowTime = Time.time;
        //애니메이션재생       
        playerInput.condition["ViewChange"] = true;      
        wait = true;
        EventManager.Instance.PostNotification(EVENT_TYPE.ZOOM, playerInput, null);
    }
    public override void UpdateGunState(PlayerInput playerInput)
    {
        if(wait && Time.time - nowTime > viewChangeTime)
        {
            Debug.Log("시간끝");
            wait = false;
            playerInput.condition["ViewChange"] = false;
            playerInput.condition["Aiming"] = true;         
        }
    }
    public GunIdleState()
    {
        playerGun = GUNSTATE.IDLE;
        viewChangeTime = 0.2f;
        wait = false;
    }
}
