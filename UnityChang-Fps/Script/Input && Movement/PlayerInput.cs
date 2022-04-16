 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, EListener
{
    public PlayerState state;
    public PLAYER nowMovementState;
    private PMovement pmovement;
    private PShooter pShooter;
    public GunState gunState;
    public GUNSTATE nowGunState;
    public string nowEquip;
    private PAnimationController aniController;
    public Dictionary<string, bool> condition = new Dictionary<string, bool>();

    //private Animator animator;
    // Start is called before the first frame update
    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.FALLING, this);
        EventManager.Instance.AddListener(EVENT_TYPE.OPEN_INVENTORY, this);
        state = new PlayerState();
        state.Init(this);
        gunState = new GunState();
        gunState.Init(this);
        nowMovementState = state.playerMovement;
        nowGunState = gunState.playerGun;
        pmovement = GetComponent<PMovement>();
        pShooter = GetComponent<PShooter>();
        aniController = GetComponent<PAnimationController>();
        condition.Add("Aiming", false);
        condition.Add("ViewChange", false);
        condition.Add("Falling", false);
        condition.Add("Inventory", false);      
    }   
    private void FixedUpdate()
    {       
        // 플레이어 액션
        if (!condition["Falling"])
        {
            state.InputHandler(this);
            //상태 변경될시
            if (nowMovementState != state.playerMovement)
            {
                state.Enter(this);
                aniController.SetMoveAnimation(state.playerMovement);
            }
            nowMovementState = state.playerMovement;
            state.UpdateMovement(this);
        }
        //건 액션
        if (nowMovementState != PLAYER.DODGE && !condition["Inventory"])
        {
            gunState.InputHandler(this);
            //상태 변경될시
            if (nowGunState != gunState.playerGun)
            {
                gunState.Enter(this);
                aniController.SetGunAnimation(gunState.playerGun);
            }
            nowGunState = gunState.playerGun;
            gunState.UpdateGunState(this);
        }
    }
    public void Move(float moveX, float moveZ, PLAYER type)
    {
        pmovement.Move(moveX, moveZ, type);
    }
    public string CheckEquip()
    {
        return pShooter.CheckEquip();
    }
    public void ViewChange()
    {
        Debug.Log("viewChange");
        state.Init(this);
        nowMovementState = state.playerMovement;
        aniController.SetMoveAnimation(state.playerMovement);
    }
    public void CancleAiming()
    {
        if (nowGunState != GUNSTATE.NONE)
        {
            gunState.Init(this);
            nowGunState = gunState.playerGun;
            condition["Aiming"] = false;          
        }
    }  
    public void Falling()
    {
        condition["Falling"] = true;
    }
    public void Landing()
    {
        condition["Falling"] = false;
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.FALLING)
        {
            if ((bool)Param)
                Falling();
            else
                Landing();
        }
        else if (Event_Type == EVENT_TYPE.OPEN_INVENTORY)
        {
            if ((bool)Param)
            {
                Cursor.visible = true;
                condition["Inventory"] = true;               
            }
            else
            {
                Cursor.visible = false;
                condition["Inventory"] = false;
            }
        }
    }
}