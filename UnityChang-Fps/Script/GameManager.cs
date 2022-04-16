using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// playerInput, gun, inventory 에 입력값이 있다.
public class GameManager : MonoBehaviour, EListener
{    
    private CursorMode cursorMode;
    private Vector2 hotSpot;
    
    // Start is called before the first frame update
    void Awake()
    {
        cursorMode = CursorMode.Auto;
        hotSpot = Vector2.zero;
        // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);    
    }
    void Start()
    {
        StartCoroutine(GameStart());
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, this);
    }

 
    private IEnumerator GameStart( )
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("start");
        //EventManager.Instance.PostNotification(EVENT_TYPE.GAME_START, this, 0);
    }
    private IEnumerator GameReset()
    {
        Debug.Log("Load");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Loading");
        //EventManager.Instance.PostNotification(EVENT_TYPE.GAME_START, this, 0);
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {   
        if (Event_Type == EVENT_TYPE.DEAD)
        {
            StartCoroutine(GameReset());
        }
    }
 

}
