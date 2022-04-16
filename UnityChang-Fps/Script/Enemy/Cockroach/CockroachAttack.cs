using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachAttack : EnemyAttack
{
    private float attackRange = 6f;
    private CockroachStatus status;
    private CockroachMove cockroachMove;
    public GameObject[] hands;
    private CockroachHand[] cockroachHands;
    private CockroachAnimator cockroachAnimator;
    private AudioSource audioSource;
    private Vector3 basePos;
    private Rigidbody rigidbody;
    public override void Init(int attackPattern, float[] attackDamage, float[] attackTimer)
    {
        base.Init(attackPattern, attackDamage, attackTimer);
        status = GetComponent<CockroachStatus>();
        cockroachMove = GetComponent<CockroachMove>();
        cockroachAnimator = GetComponent<CockroachAnimator>();
        audioSource = GetComponent<AudioSource>();
        cockroachHands = new CockroachHand[hands.Length];
        for (int i = 0; i < hands.Length; i++)
            cockroachHands[i] = hands[i].GetComponent<CockroachHand>();
        DeactivateWeapon();
        basePos = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
    }
    public override bool Enter()
    {
        if (CheckRangeInPlayer(attackRange) && !status.isDie && status.downGauge != -1)
        {
            if (status.trace && !status.isAttacking)
            {
                Debug.Log("간닷1");
                if (PatternCoolTimeCheck())
                {
                    Debug.Log("간닷2");
                    if (PatternChoice())
                    {
                        status.isAttacking = true;
                        cockroachMove.AgentPathOff();
                        basePos = transform.position;
                        audioSource.PlayOneShot(CockroachSound.Instance.GetAttackSound(pattern));
                        ActivateWeapon();
                        cockroachAnimator.StartCoroutine(cockroachAnimator.SetAttack(pattern, 1f));
                        SetTimer();                      
                        StartCoroutine(AttackingTimer());
                    }
                }            
            }
        }
        return false;
    }
    public void ActivateWeapon()
    {
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].SetActive(true);
            cockroachHands[i].Init(GetDamage());
        }     
    }
    public void DeactivateWeapon()
    {
        for (int i = 0; i < hands.Length; i++)
            hands[i].SetActive(false);
    }
    IEnumerator AttackingTimer()
    {  
        yield return new WaitForSeconds(1f);
        cockroachAnimator.SetWalk();
        status.isAttacking = false;
        DeactivateWeapon();
    }
}
