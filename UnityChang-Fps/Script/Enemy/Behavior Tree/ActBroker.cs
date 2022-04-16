using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActBroker : MonoBehaviour
{    
    public static ActBroker Instance
    {
        get { return Instance; }
        set { }
    }

    private static ActBroker instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(this);
    }
    
    public void CallAct(Component sender,MONSTERTYPE monster, ACTTYPE act)
    {
       
    }
}
