using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseCanvas;
    private bool pause;

    void Start()
    {
        pause = false;
        pauseCanvas.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                StartScreen();
            }
            else
            {
                StopScreen();
            }   
        }
     
        
    }
    public void StopScreen()
    {
        Debug.Log("stop");
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
        pause = !pause;
        Cursor.visible = true;
        EventManager.Instance.PostNotification(EVENT_TYPE.STOP_GAME, this, true);
    }
    public void StartScreen()
    {
        Debug.Log("start");
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        pause = !pause;
        Cursor.visible = false;
        EventManager.Instance.PostNotification(EVENT_TYPE.STOP_GAME, this, false);
    }
  
}
