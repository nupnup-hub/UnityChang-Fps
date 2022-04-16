using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Enemy Data", order = 3)]
public class EnemySetting : ScriptableObject
{
    [Header("Settings of objects")]
    public string name;
    public int id;

    [Header("Enemy Status")]
    public float healthPoint;
    public int moveSpeed;
    public int attackPattern;
    public float[] attackDamage;
    public float[] attackTimer;
    public int armor;
    public float downGauge;
    [Tooltip("탐지범위(m기준)")]
    public float searchRange;
   
    [Header("Enemy Sound")]
    public AudioClip idleSound;
    public AudioClip growlSound;
    public AudioClip[] attackSound;
    public AudioClip dieSound;
    public AudioClip footStepSound;
    public AudioClip downSound;
}
