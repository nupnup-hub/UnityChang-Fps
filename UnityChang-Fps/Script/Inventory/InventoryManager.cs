using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, EListener
{
    public GameObject inventory;
    public Texture2D defaultCursor;
    public Texture2D dropCursor;
    private CursorMode cursorMode;
    private Vector2 hotSpot;
    public AudioClip openInventorySound;
    public AudioClip closeInventorySound;
    private AudioSource audioSource;
    private bool open, zoom;
    
    // Start is called before the first frame update
 
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOM, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOMOFF, this);
        SetDefaultCursor();
        audioSource = GameObject.FindWithTag("ETCSound").GetComponent<AudioSource>();
        inventory.SetActive(false);
        open = zoom = false;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !zoom)
        {
            open = !open;
            if (open)
            {
                // 인벤오픈 & 오픈을 알림
                inventory.SetActive(true);
                Cursor.visible = true;
                audioSource.PlayOneShot(openInventorySound);
                EventManager.Instance.PostNotification(EVENT_TYPE.OPEN_INVENTORY, this, true);
                
            }
            else
            {
                // 인벤클로즈 & 클로즈를 알림
                inventory.SetActive(false);
                Cursor.visible = false;
                audioSource.PlayOneShot(closeInventorySound);
                EventManager.Instance.PostNotification(EVENT_TYPE.OPEN_INVENTORY, this, false);
            }
        }
    }
    public void SetDefaultCursor()
    {
        //Debug.Log("디폴특");
        cursorMode = CursorMode.Auto;
        hotSpot = Vector2.zero;
        Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
    }
    public void SetDropCursor()
    {
        cursorMode = CursorMode.Auto;
        hotSpot = Vector2.zero;
        Cursor.SetCursor(dropCursor, hotSpot, cursorMode);
    }
     public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.ZOOM)
            zoom = true;
        else if (Event_Type == EVENT_TYPE.ZOOMOFF)
            zoom = false;
    }
}
