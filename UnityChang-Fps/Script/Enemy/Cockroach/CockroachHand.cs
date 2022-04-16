using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachHand : MonoBehaviour
{
    private float damage;
    public void Init(float damage) 
    {
        this.damage = damage;
    }
    public void OnTriggerEnter(Collider hit)
    {
        if(hit.tag == "Player")
        {
            DamageAble damageAble = hit.gameObject.GetComponent<DamageAble>();
            damageAble.OnDamage(damage);
        }
    }
}
