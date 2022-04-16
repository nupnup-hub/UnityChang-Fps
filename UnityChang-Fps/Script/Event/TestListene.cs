using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListene : MonoBehaviour, EListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {       
        if (Event_Type == EVENT_TYPE.DEAD)
        {
            Debug.Log("전달 확인");           
        }
    }
}
