using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachSearch : EnemySearch
{
    private CockroachStatus  status;
    private int growCount;
    private AudioSource audioSource;
    private CockroachAnimator cockroachAnimator;
    public override void Init(float range)
    {
        base.Init(range);
        status = GetComponent<CockroachStatus>();
        growCount = 1;
        audioSource = GetComponent<AudioSource>();
        cockroachAnimator = GetComponent<CockroachAnimator>();
    }
    public override bool Enter()
    {
        bool looked = false;
        if (!delay && !status.isDie)
        {
            if (base.Enter())
            {
                looked = true;
                if (growCount > 0)
                {
                   Growl();  
                }
            }
            else if (traceTime + 10.0f < Time.time)
            {             
                status.trace = false;
                status.attacked = false;
            }
 
            if (status.trace || looked || status.attacked)
            {
                if (looked || status.attacked)
                { 
                    TraceStart();
                }
                return true;
            }
            else
            { 
                return false;
            }
        }
        return false;
    }
    public void TraceStart()
    {
        status.trace = true;
        traceTime = Time.time;
    }
    public void Growl()
    {
        growCount = 0;
        cockroachAnimator.StartCoroutine(cockroachAnimator.SetGrowl());
        StartCoroutine(GrowlTimer());
        audioSource.PlayOneShot(CockroachSound.Instance.GetGrowlSound());
    }
    public IEnumerator GrowlTimer()
    {
        status.isGrowl = true;      
        //degree = degree * 0.05f;
        transform.LookAt(player.transform.position);
        for (int i = 0; i < 20; i++)
        {
            //transform.rotation = Quaternion.AngleAxis(degree, Vector3.up);
            //transform.Rotate(new Vector3(0, degree, 0 ));
            yield return new WaitForSeconds(0.05f);
        }     
        status.isGrowl = false;
    }
    
}
