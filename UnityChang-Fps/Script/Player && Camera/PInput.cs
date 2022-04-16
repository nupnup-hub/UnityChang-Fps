using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class PInput : MonoBehaviour, EListener
{
    #region 변수
    public Dictionary<string, bool> status = new Dictionary<string, bool>();
    public string moveAxisName = "Horizontal";
    public string moveAzisName = "Vertical";

    public float moveX { get; private set; }
    public float moveZ { get; private set; }
    public int rotateL { get; private set; }
    public float currentX { get; private set; }
    public float currentY { get; private set; }
    public float preX { get; private set; }
    public float preY { get; private set; }
    public float inputX { get; private set; }
    public float inputY { get; private set; }
    public float Y_ANGLE_MIN { get; set; }
    public float Y_ANGLE_MAX { get; set; }
    public float anlgeMinOffset { get; private set; }
    public float anlgeMaxOffset { get; private set; }
    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.5f;
    public int jumpCount;
    public int jumpMax;
    public int slideCount;
    private bool wait = true;
    private bool zoomAble;
    public bool runStart, runEnd;
    #endregion
    #region 메소드

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, this);
        EventManager.Instance.AddListener(EVENT_TYPE.OPEN_INVENTORY, this);
        EventManager.Instance.AddListener(EVENT_TYPE.GAME_START, this);
        EventManager.Instance.AddListener(EVENT_TYPE.RUN_CAMERA, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOMABLE, this);
    }
    // Update is called once per frame
    private void Update()
    {
        if (!wait)
        {
            if (!status["Dead"] && !status["Inventory"])
            {
                moveX = Input.GetAxis(moveAxisName);
                moveZ = Input.GetAxis(moveAzisName);

                if (!status["Jump"] && !status["Slide"])
                {
                    if (Input.GetMouseButton(1))
                    {
                        if (status["AimingAble"] && !status["Aiming"] && zoomAble)
                        {
                            //Debug.Log("줌온");
                            status["CameraChange"] = true;
                            status["AimingAble"] = false;
                            EventManager.Instance.PostNotification(EVENT_TYPE.CAMERA_ZOOM, this, 0);
                        }
                    }
                    else if (status["Aiming"] && status["AimingAble"])
                    {
                        status["CameraChange"] = true;
                        status["AimingAble"] = false;
                        EventManager.Instance.PostNotification(EVENT_TYPE.CAMERA_ZOOM, this, 0);
                    }
                }

                if (moveX != 0 || moveZ != 0 && !status["CameraChange"])
                {
                    status["Stand"] = false;
                    status["Run"] = false;
                    status["Walk"] = true;
                    if (Input.GetKey(KeyCode.LeftShift) && !status["Aiming"] && !runStart)
                    {
                        //Debug.Log("Run");
                        status["Walk"] = false;
                        status["Run"] = true;
                        runEnd = false;
                        if (status["Zoomed"])
                            EventManager.Instance.PostNotification(EVENT_TYPE.RUN, this, 0);
                        if (Input.GetKey(KeyCode.F) && (!status["Jump"] && slideCount > 0))
                        {
                            status["Slide"] = true;
                            status["Run"] = false;
                        }
                    }
                    else if (!runEnd)
                    {
                        //Debug.Log("RunOff");
                        runStart = true;
                        runEnd = true;
                        EventManager.Instance.PostNotification(EVENT_TYPE.RUNOFF, this, 0);
                    }
                }
                else
                {
                    status["Walk"] = false;
                    status["Run"] = false;
                    status["Stand"] = true;
                }

                if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0 && !status["CameraChange"] && !status["Aiming"])
                    status["Jump"] = true;

                if (!status["CameraChange"])
                {
                    currentX += Input.GetAxis("Mouse X") * sensitivityX;
                    currentY += Input.GetAxis("Mouse Y") * sensitivityY;
                }
                if (preX != 0)
                {
                    currentX = preX;
                    currentY = preY;
                    preX = preY = 0;
                }
                currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
                if (Input.GetKey(KeyCode.Q)) rotateL = -1;
                else if (Input.GetKey(KeyCode.E)) rotateL = 1;
                else rotateL = 0;

                if (rotateL != 0) status["CharacterR"] = true;
                else status["CharacterR"] = false;

                if (inputX != currentX || inputY != currentY) status["CameraR"] = true;
                else status["CameraR"] = false;
                inputX = currentX;
                inputY = currentY;
            }
        }
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.DEAD)
        {
            status["Dead"] = true;
            status["Walk"] = false;
            status["Run"] = false;
            status["Stand"] = false;
        }
        else if (Event_Type == EVENT_TYPE.OPEN_INVENTORY)
        {
            if ((bool)Param)
            {
                status["Inventory"] = true;
                preX = currentX;
                preY = currentY;
            }
            else
            {
                status["Inventory"] = false;
            }
        }
        else if (Event_Type == EVENT_TYPE.ZOOMABLE)
        {
            if ((bool)Param)
                zoomAble = true;
            else
                zoomAble = false;
        }
        else if (Event_Type == EVENT_TYPE.GAME_START)
        {
            moveX = 0;
            moveZ = 0;
            rotateL = 0;
            currentX = 0;
            currentY = 0;
            preX = 0;
            preY = 0;
            inputX = 0;
            inputY = 0;
            Y_ANGLE_MAX = 25f;
            Y_ANGLE_MIN = -20f;
            anlgeMaxOffset = Y_ANGLE_MAX;
            anlgeMinOffset = Y_ANGLE_MIN;
            jumpMax = 1;
            jumpCount = jumpMax;
            slideCount = 1;
            status.Add("Stand", true);
            status.Add("Walk", false);
            status.Add("Run", false);
            status.Add("CameraR", false);
            status.Add("CharacterR", false);
            status.Add("Jump", false);
            status.Add("Slide", false);
            status.Add("Dead", false);
            status.Add("Inventory", false);
            status.Add("Aiming", false);
            status.Add("CameraChange", false);
            status.Add("Zoomed", false);
            status.Add("AimingAble", true);
            wait = false;
            zoomAble = false;
            runStart = false;
            runEnd = true;
            Debug.Log("start1끝");
            EventManager.Instance.PostNotification(EVENT_TYPE.GAME_START2, this, 0);
        }
    }
    #endregion
}
*/