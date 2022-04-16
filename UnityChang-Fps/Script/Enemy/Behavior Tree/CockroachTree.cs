using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachTree : BehaviorTree
{   
    public readonly int height = 3;
    private CockroachStatus status;
    void Start()
    {
        enemySearch = GetComponent<CockroachSearch>();
        enemyMove = GetComponent<CockroachMove>();
        enemyAttack = GetComponent<CockroachAttack>();       
        enemyHealth = GetComponent<CockroachHealth>();
        status = GetComponent<CockroachStatus>();
        status.Init(EnemyDatabase.Instance[(int)MONSTERTYPE.COCKROACH]);
        enemySearch.Init(status.searchRange);
        enemyMove.Init(status.moveSpeed);
        enemyAttack.Init(status.attackPattern, status.attackDamage, status.attackTimer);       
        enemyHealth.Init(status.healthPoint, status.armor, status.downGauge);
       
        root = new BNode(BNODETYPE.SEQUENCE);
        root.AddNode(BNODETYPE.SEQUENCE);
        root.AddNode(BNODETYPE.SELECTOR);
        root.AddNode(BNODETYPE.SEQUENCE);
        Debug.Log(root.children.Count);
        root.AddNode(new List<int>(new int[] {0,-1 }) , BNODETYPE.ACT, ACTTYPE.SEARCH);
        root.AddNode(new List<int>(new int[] { 1, -1 }), BNODETYPE.ACT, ACTTYPE.IDLE);
        root.AddNode(new List<int>(new int[] { 1, -1 }), BNODETYPE.ACT, ACTTYPE.WALK);
        root.AddNode(new List<int>(new int[] { 1, -1 }), BNODETYPE.ACT, ACTTYPE.RUN);
        root.AddNode(new List<int>(new int[] { 2, -1 }), BNODETYPE.ACT, ACTTYPE.ATTACK);      
    }
    private void Update()
    {
        root.Search(this);
    }
}
