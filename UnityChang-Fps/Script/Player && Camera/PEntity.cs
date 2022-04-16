using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEntity : MonoBehaviour, DamageAble
{
    private float maxHp;
    public float healthPoint;
   
    public bool isDie { get; private set; }
    
     void Awake()
     {
        maxHp = 100f;
        healthPoint = maxHp;
        isDie = false;
     }
    
    public virtual void OnDamage(float damage)
    {
        healthPoint -= damage;
        Debug.Log(healthPoint);
        if (healthPoint <= 0)
        {
            healthPoint = 0;
            isDie = true;               
        }
    }

    public virtual void RestoreHp(float figure)
    {
        healthPoint += figure;
        if (healthPoint > maxHp)
            healthPoint = maxHp;
    }
   
}
