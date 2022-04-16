using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEmptyState : GunState
{
    float viewChangeTime, nowTime;
    bool wait;
    public override void InputHandler(PlayerInput playerInput)
    {
        if (!wait && Input.GetMouseButton(1))
        {
            playerInput.nowEquip = playerInput.CheckEquip();
            if (playerInput.nowEquip != "EMPTY")
                playerInput.gunState = idle;            
        }
    }
    public override void Enter(PlayerInput playerInput)
    {
        Debug.Log("줌 해재");
        playerInput.condition["Aiming"] = false;
        playerInput.condition["ViewChange"] = true;       
        wait = true;
        EventManager.Instance.PostNotification(EVENT_TYPE.ZOOMOFF, playerInput, null);     
        //무기거둘시 애니메이션
    }
    public override void UpdateGunState(PlayerInput playerInput)
    {
        if (wait && Time.time - nowTime > viewChangeTime)
        {
            wait = false;
            playerInput.condition["ViewChange"] = false;         
        }
    }
    public GunEmptyState()
    {
        playerGun = GUNSTATE.NONE;
        viewChangeTime = 0.2f;
        wait = false;
    }
}
