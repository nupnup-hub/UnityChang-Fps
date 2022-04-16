using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    public BNode root;
    public EnemyMovement enemyMove;
    public EnemyAttack enemyAttack;
    public EnemySearch enemySearch;
    public EnemyEntity enemyHealth;
}
