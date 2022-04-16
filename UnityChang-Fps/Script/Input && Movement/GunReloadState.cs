using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReloadState : GunState
{
    public override void InputHandler(PlayerInput playerInput)
    {

    }
    public virtual void Enter(PlayerInput playerInput) { }
    public virtual void UpdateGunState(PlayerInput playerInput) { }
    public GunReloadState()
    {
        playerGun = GUNSTATE.RELOAD;
    }
}
