using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    public float searchTurm;
    public float searchRange;
    public float traceTime;
    protected GameObject player;
    public bool delay;
    public float degree;
    public Vector3 d;
    // Start is called before the first frame update
    public virtual void Init(float Range)
    {
        searchTurm = 0.5f;
        searchRange = Range;
        player = GameObject.FindGameObjectWithTag("Player");
        delay = false;
        degree = 0;
    }
    public virtual bool Enter()
    {
        if (EnemySight())
            return true;
        else
            return false;

    }
    public bool EnemySight()
    {
        Vector3 rayStart = transform.position;
        rayStart.y += 7f;
        Vector3 playerPos = player.transform.position;
        playerPos.y += 7f;

        Vector3 enemyDir = (rayStart + transform.forward) - rayStart;
        Vector3 playerDir = playerPos - rayStart;

       // Debug.DrawRay(rayStart, enemyDir * 10f, Color.green);
       // Debug.DrawRay(rayStart, playerDir * 10f, Color.blue);    
        enemyDir = enemyDir.normalized;
        playerDir = playerDir.normalized;   
        float angle = Vector3.Dot(enemyDir, playerDir);
        if (angle > 0.25f)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < searchRange)
            {
                if (Physics.Linecast(rayStart, playerPos))
                {
                    return false;
                }
                else
                {
                    //d =   (transform.forward) - (player.transform.position - transform.position);
                    //degree = Vector3.SignedAngle(enemyDir, playerDir, Vector3.up);                 
                    traceTime = Time.time;                  
                    return true;
                }
            }
        }
        return false;
    }
    public bool EnemyDamaged()
    {
        return true;
    }
    public IEnumerator TraceDelay()
    {
        delay = true;
        yield return new WaitForSeconds(1.5f);
        delay = false;
    }
}
