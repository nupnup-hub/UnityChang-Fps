using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour, EListener
{
    // Start is called before the first frame update
    public GameObject Player;
    public GameObject PlayerBody;
    public GameObject PlayerAccessory;
    public Transform rigTransform;
    public Transform zoomRigTransform;
    public Transform zoomAtTransform;
    public Transform spineTransform;
    private CameraInput cameraInput;
    private AudioListener audioListener;
    private Transform camTransform;
    private float distanceY;
    private float distanceZ;
    private float zoomAtDistanceX;
    private float offsetZX;
    private float zoomDistanceZ;
    private float offsetZZ;
    private float aimDistanceZ;
    private float offsetY;
    private float offsetZ;
    private float modelY;
    public float rotateSpeed = 1.0f;
    public float rotateSpeedOffset;
    private bool stopCamera;
    public GameObject itemNotice;
    public GameObject failGetItemNotice;
    private GameObject getItem;
    private bool wait = true;
    private bool rightHit, leftHit;
    private float rateOfZoomAngle;
    private Animator animator;
    private Transform playerChest;
    public Transform gunTransform;

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SUCCESS_GET_ITEM, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOM, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOMOFF, this);
        EventManager.Instance.AddListener(EVENT_TYPE.RUN, this);
        EventManager.Instance.AddListener(EVENT_TYPE.RUNOFF, this);
        cameraInput = GetComponent<CameraInput>();
        audioListener = GetComponent<AudioListener>();
        camTransform = transform;
        distanceZ = 7f;
        zoomAtDistanceX = 1.6f;
        offsetZX = zoomAtDistanceX;
        zoomDistanceZ = 6f;
        offsetZZ = zoomDistanceZ;
        aimDistanceZ = 3f;
        offsetZ = distanceZ;
        modelY = Player.transform.localScale.y;
        rotateSpeedOffset = rotateSpeed;
        distanceY = modelY * 1.4f;
        offsetY = distanceY;
        stopCamera = false;
        itemNotice.SetActive(false);
        wait = false;
        rightHit = leftHit = true;
        rateOfZoomAngle = distanceZ / zoomDistanceZ;
        //Debug.Log("start2끝");
        //test       
        animator = Player.GetComponent<Animator>();
        playerChest = animator.GetBoneTransform(HumanBodyBones.Chest);
        // Cursor visible

    }
    private void LateUpdate()
    {
        if (!wait)
        {
            if (!stopCamera)
                Move();
            ItemCheck();
        }
    }

    public void Move()
    {
        //L , M, R 레이 계산

        //카메라 리그 위치 계산                         
        Vector3 dir = new Vector3(0, distanceY, -distanceZ);
        Quaternion rotation = Quaternion.Euler(cameraInput.currentY * rotateSpeed, cameraInput.currentX * rotateSpeed, 0);
        Vector3 target = Player.transform.position + rotation * dir;

        //zoomAtTransform위치
        Vector3 zoomLookAtDir = new Vector3(zoomAtDistanceX, 0, 0);
        Vector3 zoomLookAtTarget = Player.transform.position + rotation * zoomLookAtDir;
        zoomAtTransform.position = Vector3.Slerp(zoomAtTransform.position, zoomLookAtTarget, 1f);
        //zoomAtTransform.LookAt(lookAt);
        //zoomRig 위치 계산          
        Vector3 zoomDir = new Vector3(0, distanceY, -zoomDistanceZ);
        Vector3 zoomTarget = zoomAtTransform.position + rotation * zoomDir;
        zoomRigTransform.position = zoomTarget;
        Vector3 zoomRigLookAt = zoomAtTransform.position;
        zoomRigLookAt.y += modelY * 1.4f;
        zoomRigTransform.LookAt(zoomRigLookAt);

        Vector3 rigDir = new Vector3(0, distanceY, -offsetZZ);
        Vector3 rigTarget = zoomAtTransform.position + rotation * rigDir;
        rigTransform.position = rigTarget;
        //카메라가 바라볼 위치 계산
        Vector3 lookAt = Player.transform.position;
        lookAt.y += modelY * 1.4f;
        rigTransform.LookAt(zoomRigLookAt);

        Vector3 rayStart, rayEndL, rayEndR;
        RaycastHit hit;
        rayStart = Player.transform.position;
        rayStart.y += modelY * 1.4f;
        rayEndL = rigTransform.position - rayStart;
        rayEndR = rigTransform.position - rayStart;
        rayEndL.x = rayEndL.x + 1.5f;
        rayEndR.x = rayEndR.x - 1.5f;
        //Debug.DrawRay(rayStart, rayEndL * (offsetZ), Color.red);
        //Debug.DrawRay(rayStart, rayEndL * (offsetZ + 2f), Color.red);
        //Debug.DrawRay(rayStart, rayEndR * (offsetZ + 2f), Color.green);
        //Debug.DrawRay(rayStart, rayEndR * (offsetZ), Color.green);
        /*
       
           
        Debug.DrawRay(rayStart+rayEndL, Vector3.up * 10f, Color.red);
        Debug.DrawRay(rayStart+rayEndR, Vector3.up * 10f, Color.green);
        
        */
        //Debug.DrawRay(rayStart, -camTransform.forward * 12f, Color.yellow);
        Vector3 cameraForward = camTransform.forward;
        cameraForward.y = 0;
        // 카메라 충돌 레이캐스트    

        if (Physics.Raycast(rayStart, rayEndL, out hit, offsetZ) && (hit.collider.tag != "Enemy" && hit.collider.tag != "Player") && !rightHit
             && !cameraInput.status["Aiming"])
        {
            Debug.Log("Left Hit");
            float dis = Vector3.Distance(hit.point, Player.transform.position);
            dis = dis * dis - distanceY * distanceY;
            if (dis < 0)
            {
                //dis *= -1;
                dis = 2;
                zoomDistanceZ = Mathf.Sqrt(dis);
            }
            else
            {
                zoomDistanceZ = Mathf.Sqrt(dis);
                if (zoomDistanceZ > 2)
                    zoomDistanceZ -= (0.175f * zoomDistanceZ);
            }          
            if (hit.distance < 2.0f)
            {
                //Debug.Log(hit.distance);
                PlayerBody.SetActive(false);
                PlayerAccessory.SetActive(false);
            }
            else
            {
                PlayerBody.SetActive(true);
                PlayerAccessory.SetActive(true);
            }
            leftHit = true;
        }
        else
            leftHit = false;

        if (Physics.Raycast(rayStart, rayEndR, out hit, offsetZ) && (hit.collider.tag != "Enemy" && hit.collider.tag != "Player") && !leftHit
              && !cameraInput.status["Aiming"])
        {
            Debug.Log("Right Hit");
            float dis = Vector3.Distance(hit.point, Player.transform.position);
            dis = dis * dis - distanceY * distanceY;
            if (dis < 0)
            {
                //dis *= -1;
                dis = 2;
                zoomDistanceZ = Mathf.Sqrt(dis);
            }
            else
            {
                zoomDistanceZ = Mathf.Sqrt(dis);
                if (zoomDistanceZ > 2)
                    zoomDistanceZ -= (0.175f * zoomDistanceZ);
            }
            if (hit.distance < 2.0f)
            {
                //Debug.Log(hit.distance);
                PlayerBody.SetActive(false);
                PlayerAccessory.SetActive(false);
            }
            else
            {
                PlayerBody.SetActive(true);
                PlayerAccessory.SetActive(true);
            }
            rightHit = true;
        }
        else
            rightHit = false;

        if (!leftHit && !rightHit)
        {
            if (PlayerBody.activeSelf == false)
            {
                PlayerBody.SetActive(true);
                PlayerAccessory.SetActive(true);
            }
            if (!cameraInput.status["Run"] && !cameraInput.status["CameraChange"])
            {
                if (!cameraInput.status["Aiming"])
                    zoomDistanceZ = offsetZZ;
                else
                    zoomDistanceZ = offsetZZ - 3;
                //Debug.Log("충돌x");
            }
        }
        camTransform.position = Vector3.Slerp(camTransform.position, zoomRigTransform.position, 1f);
        if (cameraInput.status["Aiming"])
        {
            Quaternion rotationC = rotation;
            rotationC.x = 0;
            rotationC.z = 0;
            Player.transform.rotation = rotationC;
            Debug.DrawRay(camTransform.position, camTransform.forward * 100f, Color.red);

            Vector3 chestDir = camTransform.position + (camTransform.forward * 50f);
            Vector3 chestOffset = new Vector3(0, -95, -71);

            //playerChest.LookAt(chestDir);
            //playerChest.rotation = playerChest.rotation * Quaternion.Euler(chestOffset);
            Vector3 gunDir = camTransform.position;
            gunDir += (camTransform.forward * 50f);
            //gunTransform.LookAt(gunDir);
        }
        Debug.DrawRay(zoomRigLookAt, Vector3.up * 15f, Color.red);
        Debug.DrawRay(zoomRigTransform.position, Vector3.up * 15f, Color.blue);
        Debug.DrawRay(camTransform.position, Vector3.up * 15f, Color.green);
        if (cameraInput.status["Aiming"])
            camTransform.LookAt(zoomRigLookAt);
        else if (!leftHit && !rightHit)
            camTransform.LookAt(zoomRigLookAt);
        else
            camTransform.LookAt(lookAt);

    }
    public void ItemCheck()
    {
        Vector3 rayStart = transform.position;
        rayStart.y += 2f;
        //rayStart.y = rayStart.y + 1f;        
        
        RaycastHit hit;

        List<Vector3> rayEnd = new List<Vector3>();
        Vector3 camPosition = camTransform.forward;
        for (int i = 0; i < 20; i++)
        {
            camPosition.y = camPosition.y - (0.015f * i);
            rayEnd.Add(camPosition);
            //Debug.DrawRay(rayStart, rayEnd[i] * 15f, Color.red);
            if (Physics.Raycast(rayStart, rayEnd[i], out hit, 17f) && (hit.collider.tag == "Item" || hit.collider.tag == "Weapon"))
            {
                //Debug.Log(hit.collider.name);
                GetItem(hit);
                break;
            }
            else
                itemNotice.SetActive(false);
        }
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.ZOOM)
        {
            cameraInput.status["CameraChange"] = true;
            StartCoroutine(ZoomOn());
        }
        else if (Event_Type == EVENT_TYPE.ZOOMOFF)
        {
            cameraInput.status["CameraChange"] = true;
            StartCoroutine(ZoomOff());
        }
        else if (Event_Type == EVENT_TYPE.RUN)
        {
            StartCoroutine(RunOn());
        }
        else if (Event_Type == EVENT_TYPE.RUNOFF)
        {
            StartCoroutine(RunOff());
        }
        else if (Event_Type == EVENT_TYPE.SUCCESS_GET_ITEM)
        {
            if ((bool)Param)
            {
                getItem.SetActive(false);
                Destroy(getItem);
            }
            else
            {
                StartCoroutine(Notice());
            }
        }
    }
    public IEnumerator Notice()
    {
        failGetItemNotice.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        failGetItemNotice.SetActive(false);
    }
    public IEnumerator ZoomOn()
    {
        cameraInput.status["Aimed"] = false;
        float plusX = (offsetZX - zoomAtDistanceX) / 20f;
        float plusZ = (zoomDistanceZ - (offsetZZ - 3)) / 20f;
        for (int i = 0; i < 20; i++)
        {
            if (zoomDistanceZ >= offsetZZ - 3)
                zoomDistanceZ -= plusZ;
            if (zoomAtDistanceX <= offsetZX)
                zoomAtDistanceX += plusX;
            yield return new WaitForSeconds(0.001f);
        }
        cameraInput.Y_ANGLE_MAX = cameraInput.anlgeMaxOffset / 2f;
        cameraInput.Y_ANGLE_MIN = cameraInput.anlgeMinOffset / 1f;
        cameraInput.status["CameraChange"] = false;
        cameraInput.status["Aiming"] = true;
    }
    public IEnumerator ZoomOff()
    {
        cameraInput.status["Aiming"] = false;
        float plus = (offsetZZ - zoomDistanceZ) / 20f;
        for (int i = 0; i < 20; i++)
        {
            if (zoomDistanceZ <= offsetZZ)
                zoomDistanceZ += plus;
            yield return new WaitForSeconds(0.001f);
        }
        cameraInput.status["CameraChange"] = false;
        cameraInput.Y_ANGLE_MAX = cameraInput.anlgeMaxOffset;
        cameraInput.Y_ANGLE_MIN = cameraInput.anlgeMinOffset;
    }
    public IEnumerator RunOn()
    {
        cameraInput.status["Run"] = true;
        cameraInput.status["Aiming"] = false;
        float plusX = (zoomAtDistanceX - (offsetZX - 1.7f)) / 17f;
        float plusZ = ((offsetZZ + 1.5f) + zoomDistanceZ) / 17f;
        for (int i = 0; i < 17; i++)
        {
            if (zoomAtDistanceX >= offsetZX - 1.7f)
                zoomAtDistanceX -= plusX;
            if (zoomDistanceZ <= offsetZZ + 1.5f)
                zoomDistanceZ += plusZ;
            yield return new WaitForSeconds(0.01f);
        }
    }
    //Run 딜레이
    public IEnumerator RunOff()
    {
        float plusX = (offsetZX - zoomAtDistanceX) / 17f;
        float plusZ = (zoomDistanceZ - offsetZZ) / 17f;

        for (int i = 0; i < 17; i++)
        {
            if (zoomAtDistanceX <= offsetZX)
                zoomAtDistanceX += plusX;
            if (zoomDistanceZ >= offsetZZ)
                zoomDistanceZ -= plusZ;
            yield return new WaitForSeconds(0.01f);
        }
        cameraInput.status["Run"] = false;
    }
    public void GetItem(RaycastHit hit)
    {
        itemNotice.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 아이템 뷰함수 
            //StartCoroutine(ItemView());       
            string name = hit.collider.gameObject.name;
            //Debug.Log("발견" + " " + name);
            getItem = hit.collider.gameObject;
            if (name == " ")
                Debug.Log("아이템 습득 오류");
            else
                EventManager.Instance.PostNotification(EVENT_TYPE.GET_ITEM, this, name);
        }
    }
}

// 총기방향조절
/*  if (!cameraInput.status["CameraChange"])
        {
            camTransform.position = Vector3.Slerp(camTransform.position, zoomRigTransform.position, 1f);
            if (cameraInput.status["Aiming"])
            {
                Quaternion rotationC = rotation;
                rotationC.x = 0;
                rotationC.z = 0;
                Player.transform.rotation = rotationC;
                Debug.DrawRay(camTransform.position, camTransform.forward * 100f, Color.red);

                Vector3 chestDir = camTransform.position + (camTransform.forward * 50f);
                Vector3 chestOffset = new Vector3(0, -95, -71);

                //playerChest.LookAt(chestDir);
                //playerChest.rotation = playerChest.rotation * Quaternion.Euler(chestOffset);
                Vector3 gunDir = camTransform.position;
                gunDir += (camTransform.forward * 50f);
                //gunTransform.LookAt(gunDir);
            }
        }*/
