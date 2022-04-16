using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachStatus : MonoBehaviour
{
    public string name;
    public int id;
    public float healthPoint;
    public int moveSpeed;
    public int attackPattern;
    public float[] attackDamage;
    public float[] attackTimer;
    public int armor;
    public float downGauge;
    public float searchRange;
    public bool attacked;
    public bool trace;
    public bool isDie;
    public bool isGrowl;
    public bool isAttacking;
    public void Init(EnemyStatus enemy)
    {
        name = enemy.name;
        id = enemy.id;
        healthPoint = enemy.healthPoint;
        moveSpeed = enemy.moveSpeed;
        attackPattern = enemy.attackPattern;
        attackDamage = enemy.attackDamage;
        attackTimer = enemy.attackTimer;
        armor = enemy.armor;
        downGauge = enemy.downGauge;
        searchRange = enemy.searchRange;
        attacked = isDie  =false;
        isGrowl = true;
        isAttacking = false;
    }
}
