using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachSound : MonoBehaviour
{
    public AudioClip idleSound;
    public AudioClip growlSound;
    public AudioClip[] attackSound;
    public AudioClip dieSound;
    public AudioClip footStepSound;
    public AudioClip downSound;
    private static CockroachSound cockroachSound = null;
    public static CockroachSound Instance
    {
        get { return cockroachSound; }
        private set { }
    }
    void Awake()
    {
        if (cockroachSound == null)
            cockroachSound = this;     
    }
    public void Start()
    {
        EnemySetting data = EnemyDatabase.InstanceDatabase.data[(int)MONSTERTYPE.COCKROACH];
        idleSound  = data.idleSound;
        growlSound = data.growlSound;
        attackSound = data.attackSound;
        dieSound = data.dieSound;
        footStepSound = data.footStepSound;
        downSound = data.downSound;
    }
    public AudioClip GetIdleSound()
    {
        return idleSound;
    }
    public AudioClip GetGrowlSound()
    {
        return growlSound;
    }
    public AudioClip GetAttackSound(int i )
    {
        return attackSound[i];
    }
    public AudioClip GetDieSound( )
    {
        return dieSound;
    }
    public AudioClip GetFootStepSound( )
    {
        return footStepSound;
    }
    public AudioClip GetDownSound( )
    {
        return downSound;
    }

}
