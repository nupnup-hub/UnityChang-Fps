using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour, EListener
{
    public float currentX { get; private set; }
    public float currentY { get; private set; }
    //public float preX { get; private set; }
    //public float preY { get; private set; }  
    public float Y_ANGLE_MIN { get; set; }
    public float Y_ANGLE_MAX { get; set; }
    public float anlgeMinOffset { get; private set; }
    public float anlgeMaxOffset { get; private set; }
    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.5f;
    public Dictionary<string, bool> status = new Dictionary<string, bool>();
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.OPEN_INVENTORY, this);
        EventManager.Instance.AddListener(EVENT_TYPE.STOP_GAME, this);
        currentX = 0;
        currentY = 0;
        Y_ANGLE_MAX = 25f;
        Y_ANGLE_MIN = -20f;
        anlgeMaxOffset = Y_ANGLE_MAX;
        anlgeMinOffset = Y_ANGLE_MIN;
        status.Add("Aiming", false);
        status.Add("Aimed", false);
        status.Add("CameraChange", false);
        status.Add("Run", false);
        status.Add("Inventory", false);
        status.Add("Pause", false);
    }

    // Update is called once per frame
    void Update()
    {        
        if (!status["Inventory"] && !status["Pause"])
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY += Input.GetAxis("Mouse Y") * sensitivityY;
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }       
        //preX = currentX;
        //preY = currentY;
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {

        if (Event_Type == EVENT_TYPE.OPEN_INVENTORY)
        {
            if ((bool)Param)
                status["Inventory"] = true;
            else           
                status["Inventory"] = false;                        
        }
        if (Event_Type == EVENT_TYPE.STOP_GAME)
        {
            if ((bool)Param)
                status["Pause"] = true;
            else
                status["Pause"] = false;
        }
    }
}
