using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CockroachMove : EnemyMovement
{
    private CockroachStatus status;
    public GameObject Enemy;
    private NavMeshAgent agent;
    private bool runSoundKey, walkSoundKey, breathSoundKey, down, idlePatChange;
    private CockroachAnimator cockroachAnimator;
    private EnemySearch enemySearch;
    private AudioSource audioSource;
    private int count, idlePat;
    private float timer, idleTimer, idleDelay, breathDelay;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cockroachAnimator = GetComponent<CockroachAnimator>();
        audioSource = GetComponent<AudioSource>();
        moveSlope = down = breathSoundKey =false;
        runSoundKey = walkSoundKey = idlePatChange = true;
        count = 1;
        timer = idleTimer = Time.time;
        idlePat = 0;
        idleDelay = breathDelay = 0;
        StartCoroutine(BreathSoundDelay());
    }
    private void FixedUpdate()
    {
        if (!status.trace && !status.isAttacking && !status.isDie)
        {
            if (idlePatChange)
            {
                idlePat = Random.Range(0, 4);
                idleTimer = Time.time;
                idleDelay = Random.Range(1.0f, 2.5f);
                idlePatChange = false;
                if (idlePat == 0)
                    cockroachAnimator.SetWalk();
                else 
                    cockroachAnimator.SetStand();
                breathDelay = Random.Range(1.5f, 2.0f);
            }
            else if (!idlePatChange)
            {
                if (idleDelay + idleTimer < Time.time)
                {
                    idlePatChange = true;
                }
                else
                {
                    if (idlePat == 0)
                    {
                        base.SetMovement();
                    }
                       
                    if (breathSoundKey)
                    {
                        audioSource.PlayOneShot(CockroachSound.Instance.GetIdleSound(), 0.05f);
                        breathSoundKey = false;
                        StartCoroutine(BreathSoundDelay());
                    }
                }
            }
        }
    }
    public override void Init(float speed)
    {
        base.Init(speed);
        status = GetComponent<CockroachStatus>();
        enemySearch = GetComponent<EnemySearch>();

    }
    public override bool Enter()
    {
        moveSlope = false;
        if (status.trace && !status.isAttacking)
        {
            if (!status.isGrowl && !status.isDie && !down)
            {
                if (!moveSlope)
                {
                    float dis = Vector3.Distance(transform.position, player.transform.position);
                    if (count > 0 && Time.time - timer > 1f && dis > 10f)
                        CheckRun();
                    if (dis < 6f)
                    {
                        //status.trace = false;
                        cockroachAnimator.SetStand();
                        //enemySearch.StartCoroutine(enemySearch.TraceDelay());
                        agent.velocity = Vector3.zero;
                        rigidbody.velocity = Vector3.zero;
                        //agent.ResetPath();
                        count = 1;
                    }
                    else
                    {
                        if (isRun)
                        {
                            agent.speed = runSpeed;
                            cockroachAnimator.SetRun();
                            if (runSoundKey)
                            {
                                StartCoroutine(RunSoundDelay());
                                audioSource.PlayOneShot(CockroachSound.Instance.GetFootStepSound(), 0.6f);
                            }
                        }
                        else
                        {
                            agent.speed = speed;
                            cockroachAnimator.SetWalk();
                            if (walkSoundKey)
                            {
                                StartCoroutine(WalkSoundDelay());
                                audioSource.PlayOneShot(CockroachSound.Instance.GetFootStepSound(), 0.6f);
                            }
                        }
                        agent.SetDestination(player.transform.position);
                    }
                }           
                if (timer % 0.05 == 0)
                    transform.LookAt(player.transform);
                idlePatChange = true;
            }
            else
            {
                Debug.Log("무브노논");
                agent.velocity = Vector3.zero;
                rigidbody.velocity = Vector3.zero;
                cockroachAnimator.SetStand();
            }
        }
        return true;
    }

    public void CheckRun()
    {
        int ran = Random.Range(0, 10);
        if (ran > 7)
        { 
            count = 0;
            isRun = true;
        }
        else
            isRun = false;
        timer = Time.time;
    }
    public IEnumerator RunSoundDelay()
    {
        runSoundKey = false;
        yield return new WaitForSeconds(0.4f);
        runSoundKey = true;
    }
    public IEnumerator WalkSoundDelay()
    {
        walkSoundKey = false;
        yield return new WaitForSeconds(0.6f);
        walkSoundKey = true;
    }
    public IEnumerator BreathSoundDelay()
    {
        breathSoundKey = false;
        yield return new WaitForSeconds(2f);
        breathSoundKey = true;
    }
    public IEnumerator Down()
    {
        agent.velocity = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        down = true;
        yield return new WaitForSeconds(0.5f);
        cockroachAnimator.SetStand();
        yield return new WaitForSeconds(1.0f);
        down = false;
        status.downGauge = -1000;       
    }
 
    public void LookPlayer()
    {
        transform.LookAt(player.transform.position);
    }
    public void AgentPathOff()
    {
        agent.velocity = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        agent.ResetPath();
    }
}
