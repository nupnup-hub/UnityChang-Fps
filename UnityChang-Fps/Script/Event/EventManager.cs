using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region 프로퍼티
    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }
    #endregion

    #region 변수
    private static EventManager instance = null;
    private Dictionary<EVENT_TYPE, List<EListener>> Listeners =
        new Dictionary<EVENT_TYPE, List<EListener>>();
    #endregion

    #region 메소드
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

    public void AddListener(EVENT_TYPE Event_Type, EListener Listener)
    {
        List<EListener> ListenList = null;
        Debug.Log(Listeners.ToString() + "추가완료!");
        // 값이 있으면 Listeners에 추가
        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }
        // 없으면 Event_Type과 Listener을 연결해서 Listeners에 추가
        ListenList = new List<EListener>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);
    }

    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        List<EListener> ListenList = null;

        if (!Listeners.TryGetValue(Event_Type, out ListenList))
            return;

        for(int i = 0; i < ListenList.Count; i++)
        {
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type, Sender, Param);
        }
    }

    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        Listeners.Remove(Event_Type);
    }

    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<EListener>> TmpListeners =
            new Dictionary<EVENT_TYPE, List<EListener>>();

        foreach(KeyValuePair<EVENT_TYPE, List<EListener>> Item in Listeners)
        {
            for(int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }

        Listeners = TmpListeners;
    }

    /*void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }*/
    #endregion
    

}
