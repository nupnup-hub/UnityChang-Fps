using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BNODETYPE
{
    SEQUENCE,
    SELECTOR,
    ACT
}
public enum ACTTYPE
{
    NONE,
    SEARCH,
    IDLE,
    WALK,
    RUN,
    ATTACK
}
[System.Serializable]
public class BNode
{
    public List<BNode> children = new List<BNode>();
    public BNODETYPE nodeType;
    public ACTTYPE actType;
    public BNode(BNODETYPE n)
    {
        nodeType = n;
    }
    public BNode(BNODETYPE n, ACTTYPE a)
    {
        nodeType = n;
        actType = a;
    }
    public bool Search(BehaviorTree enemy)
    {
        for (int i = 0; i < children.Count; i++)
        {
            {
                bool result = children[i].Search(enemy);
                if (result && nodeType == BNODETYPE.SELECTOR)
                {
                    //Debug.Log(nodeType.ToString() + " i: " +(i + 1) + "성공");
                    return true;
                }
                else if (!result && nodeType == BNODETYPE.SEQUENCE)
                {
                    //Debug.Log(nodeType.ToString() + " i: " + (i + 1) + "실패");
                    return false;
                }
            }
        }
        if (nodeType == BNODETYPE.SELECTOR)
            return false;
        else if (nodeType == BNODETYPE.SEQUENCE)
        {
            return true;
        }
        else
        {
            if (actType == ACTTYPE.SEARCH)
            {
                
                if (enemy.enemySearch.Enter())
                    return true;
            }
            else if (actType == ACTTYPE.ATTACK)
            {
                if (enemy.enemyAttack.Enter())
                    return true;
            }
            else if (actType == ACTTYPE.IDLE || actType == ACTTYPE.WALK || actType == ACTTYPE.RUN)
            { 
                if (enemy.enemyMove.Enter())
                    return true; 
            }
            return false;
        }
    }
    public void AddNode(BNODETYPE type)
    {
        children.Add(new BNode(type));
    }
    public void AddNode(BNODETYPE type, ACTTYPE actType)
    {
        children.Add(new BNode(type, actType));
    }
    public void AddNode(List<int> coordinate, BNODETYPE type)
    {
        int n = coordinate[0];
        coordinate.RemoveAt(0);
        if (n == -1)
            AddNode(type);
        else
            children[n].AddNode(coordinate, type);
    }
    public void AddNode(List<int> coordinate, BNODETYPE type, ACTTYPE actType)
    {
        int n = coordinate[0];
        coordinate.RemoveAt(0);
        if (n == -1)
            AddNode(type, actType);
        else
            children[n].AddNode(coordinate, type, actType);
    }
}



