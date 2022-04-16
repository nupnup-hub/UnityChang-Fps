using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachHealth : EnemyEntity
{
    private CockroachStatus status;
    private CockroachMove cockroachMove;
    private CockroachAnimator cockroachAnimator;
    private CockroachSearch cockroachSearch;
    private AudioSource audioSource;
    private bool isDown;
    public override void Init(float maxHp, int armor, float downGauge)
    {
        base.Init(maxHp, armor, downGauge);
        status = GetComponent<CockroachStatus>();
        cockroachAnimator = GetComponent<CockroachAnimator>();
        audioSource = GetComponent<AudioSource>();
        cockroachMove = GetComponent<CockroachMove>();
        cockroachSearch = GetComponent<CockroachSearch>();
        isDown = false;
    }
    
    public override void OnDamage(float damage)
    {
        Debug.Log("hit" + damage);        
        base.OnDamage(damage);
        if (!isDown)
        { 
            status.downGauge -= damage;
            if (status.downGauge < 0)
            {
                isDown = true;
                status.downGauge = -1;
                cockroachAnimator.StartCoroutine(cockroachAnimator.SetDown());
                audioSource.PlayOneShot(CockroachSound.Instance.GetDownSound());
                cockroachMove.StartCoroutine(cockroachMove.Down());              
            }
        }
        if(isDie)
        {
            status.isDie = true;
            cockroachMove.AgentPathOff();
            audioSource.PlayOneShot(CockroachSound.Instance.GetDieSound());
            cockroachAnimator.SetDie();            
            Destroy(gameObject, 2f);
        }
        if (!status.attacked)
        {
            cockroachMove.LookPlayer();
            status.attacked = true;
            if (status.isGrowl)
            {
                cockroachSearch.Growl();               
            }
            cockroachSearch.TraceStart();
        }
    }
   
}
