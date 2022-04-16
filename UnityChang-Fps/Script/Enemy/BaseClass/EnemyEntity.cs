using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour , DamageAble
{
    private float maxHp;
    public float healthPoint;
    public bool isDie { get; private set; }
    private int armor;
    private float downGauge;

    public virtual void Init(float maxHp, int armor, float downGauge)
    {
        this.maxHp = maxHp;
        healthPoint = maxHp;
        this.armor = armor;
        this.downGauge = downGauge;
        isDie = false;
    }
    public void Start()
    {
            
    }
    public virtual void OnDamage(float damage)
    {
        healthPoint -= damage;
        if(healthPoint < 0)
        {
            healthPoint = 100;
            isDie = true;         
        }
    }
}
