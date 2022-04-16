using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour, DamageAble
{
    public GameObject enemy;
    private EnemyEntity enemyEntity;

    public void Start()
    {
        enemyEntity = enemy.GetComponent<EnemyEntity>();
    }
    public void OnDamage(float damage)
    {
        enemyEntity.OnDamage(damage);
    }
    
}
