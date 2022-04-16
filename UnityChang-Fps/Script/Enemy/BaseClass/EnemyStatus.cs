using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTERTYPE
{
    COCKROACH,
    ARACHNID,
    JUGGERNAUT,
    REPTILE,
    INSECT,
    MUTANT,
    SLUG
}
[System.Serializable]
public class EnemyStatus
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
    public EnemyStatus(EnemySetting enemy)
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
      
    }
    public EnemyStatus (EnemyStatus enemy)
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
       
    }
}
