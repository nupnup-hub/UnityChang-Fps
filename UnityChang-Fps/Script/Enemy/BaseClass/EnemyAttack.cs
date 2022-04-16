using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private int attackPattern;
    private float[] attackDamage;
    private float[] attackTimer;
    private float[] timer = new float[2];
    protected GameObject player;
    public int pattern;
    // Start is called before the first frame update
  
    public virtual void Init(int attackPattern, float[] attackDamage, float[] attackTimer)
    {
        this.attackPattern = attackPattern;
        this.attackDamage = attackDamage;
        this.attackTimer = attackTimer;
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < attackDamage.Length; i++)
            timer[i] = 0;
        pattern = -1;
    }
    public virtual bool Enter()
    {
        return true;
    }
    public bool CheckRangeInPlayer(float range)
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= range)
            return true;
        else
            return false;
    }
    public bool PatternCoolTimeCheck()
    {
        for (int i = 0; i < attackPattern; i++)
        {
            if (attackTimer[i] + timer[i] < Time.time)
                return true;
        }
        return false;
    }
    public bool PatternChoice()
    {
        float exit = Time.time;
        while (pattern == -1)
        {
            int ran = Random.Range(0, 2);
            if (ran == 0)
            {
                for (int i = 0; i < attackPattern; i++)
                {
                    if (attackTimer[i] + timer[i] < Time.time)
                    {
                        pattern = i;
                        return true;
                    }
                }
            }
            else if (ran == 1)
            {
                for (int i = 1; i >= 0; i--)
                {
                    if (attackTimer[i] + timer[i] < Time.time)
                    {
                        pattern = i;
                        return true;
                    }
                }
            }
            if (exit + 10f < Time.time)
                return false;
        }
        return false;
    }
    public void SetTimer()
    {
        timer[pattern] = Time.time;
        pattern = -1;
    }
    public float GetDamage()
    {
        return attackDamage[pattern];
    }
 
}
