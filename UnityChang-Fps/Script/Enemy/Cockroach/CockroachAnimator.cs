using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }
    public void SetStand()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsStanding", true);
    }
    public void SetWalk()
    {
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", true);
    }
    public void SetRun()
    {
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);
    }
    public void SetDie()
    {
        animator.SetBool("IsDie", true);
    }
    public IEnumerator SetAttack(int i , float time)
    {
        i += 1;
        animator.SetBool("Attack" +i.ToString(), true);
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        yield return new WaitForSeconds(time);
        animator.SetBool("Attack" + i.ToString(), false);
    }
    public IEnumerator SetGrowl()
    {
        animator.SetBool("Growl", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Growl", false);
    }   
    public IEnumerator SetDown()
    {
        animator.SetBool("Down", true);
        animator.SetBool("IsStanding", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Down", false);
    }
}
