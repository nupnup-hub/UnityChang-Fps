using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GUNSTATE
{  
    NONE,
    IDLE,
    FIRE,
    RELOAD
}
public class GunState 
{ 
    public static GunFIreState fire = new GunFIreState();
    public static GunIdleState idle = new GunIdleState();
    public static GunReloadState reload = new GunReloadState();
    public static GunEmptyState emptyGun = new GunEmptyState();
    public GUNSTATE playerGun;

    public virtual void InputHandler(PlayerInput playerInput) { }
    public virtual void Enter(PlayerInput playerInput) { }
    public virtual void UpdateGunState(PlayerInput playerInput) { }
    public void Init(PlayerInput playerInput)
    {
        playerInput.gunState = emptyGun;               
    }
}
