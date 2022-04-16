using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAnimationController : MonoBehaviour
{
    private Animator animator;
    private int nowGun;
    private Dictionary<int, string> moveClip = new Dictionary<int, string>();   
    private Dictionary<int, string> gunClip = new Dictionary<int, string>();
    private Dictionary<int, string> gunAction = new Dictionary<int, string>();
    //private Dictionary<PLAYER, string> aimClip;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        moveClip.Add(0, "IsStanding");
        moveClip.Add(1, "IsWalking");
        moveClip.Add(2, "IsRunning");
        moveClip.Add(3, "IsDodging");
        gunClip.Add(0, "EMPTY");
        gunClip.Add(1, "GLOCK17");
        gunClip.Add(2, "AR15");
        gunClip.Add(3, "SHOTGUN");
        gunAction.Add(0, "NONE");
        gunAction.Add(1, "IDLE");
        gunAction.Add(2, "FIRE");
        gunAction.Add(3, "RELOAD");     
        animator.SetBool((string)gunClip[0], true);  
    }
    
    public void SetMoveAnimation(PLAYER type)
    {
        for (int i = 0; i < moveClip.Count; i++)
        { 
            if ((int)type == i)
                animator.SetBool((string)moveClip[i], true);
            else
                animator.SetBool((string)moveClip[i], false);
        }
    }
    public void SetGunAnimation(GUNSTATE type)
    {
        for (int i = 0; i < 2; i++)
        {
            if ((int)type == i)
                animator.SetBool((string)gunAction[i], true);
            else
                animator.SetBool((string)gunAction[i], false);
        }
    }
    public void ChangWeaponClip(string name)
    {
        Debug.Log(name + " 전달" );
        if (name == "EMPTY")
            animator.SetBool("SWAP", true);
        else       
            animator.SetBool(name + "_SWAP", true);
        animator.SetBool((string)name, true);
        for (int i = 0; i < gunClip.Count; i++)
        {
            if (gunClip[i] != name)
                animator.SetBool((string)gunClip[i], false);            
        }
        animator.SetBool("EMPTY", true);
        StartCoroutine(SwapTime(name));
    }
    public void Reload()
    {
        animator.SetInteger("PRE_ZOOM", 0);
        animator.SetBool("RELOAD", true);
    }
    public void CancelReload(bool zoom)
    {
        animator.SetBool("RELOAD", false);
        if (zoom)
            animator.SetInteger("PRE_ZOOM", 1);
        else
            animator.SetInteger("PRE_ZOOM", 2);        
    }
    public void Fire()
    {
        animator.SetBool("FIRE", true);
    }
    public void CancelFire()
    {
        animator.SetBool("FIRE", false);
    }
    public IEnumerator SwapTime(string name)
    {  
        yield return new WaitForSeconds(0.6f);              
        if (name == "EMPTY")
             animator.SetBool("SWAP", false);
        else
        {
            animator.SetBool(name + "_SWAP", false);
            ;// animator.SetBool("EMPTY", false);
        }
    }
}
