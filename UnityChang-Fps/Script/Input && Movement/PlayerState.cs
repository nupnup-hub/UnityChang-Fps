using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER
{
    STAND,
    WALK,
    RUN,
    DODGE,
}

public class PlayerState
{
   
    public static StandingState standing = new StandingState();
    public static WalkingState walking = new WalkingState();
    public static RunningState running = new RunningState();
    public static DodgingState dodging = new DodgingState();  
    public float moveX, moveZ;
    public PLAYER playerMovement;

    public void Awake()
    {
        //Debug.Log("초기화 끝");
    }
    // 상태에 따른 입력값 받아옴
    public virtual void InputHandler(PlayerInput playerInput) { }
    // 상태변경시 단한번 수행
    public virtual void Enter(PlayerInput playerInput) { }
    // 해당상태에 연결된 Action 수행
    public virtual void UpdateMovement(PlayerInput playerInput) { }
    // 초기화
    public void Init(PlayerInput playerInput)
    {
        playerInput.state = standing;       
        //playerInput.state.Enter(playerInput);
    }
}
