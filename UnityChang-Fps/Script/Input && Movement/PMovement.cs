using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMovement : MonoBehaviour, EListener
{
    private Rigidbody prigidbody;
    public GameObject mainCamera;
    private AudioSource playerAudio;
    public AudioClip[] SlideSounds;
    public AudioClip[] jumpSounds;
    private PLand pland;
    private CameraController cameraController;
    private Animator animator;
    private float speed;
    private float walkspeed = 7.0f;
    private float runspeed = 20.0f;
    private float acceleration = 0.0f;
    public float rotateSpeed = 10f;
    private Vector3 moveDistance;
    private float jumpPower, slidePower;
    private bool jumpCheck;
    private float modelY;
    private float slopeOffsetY;
    private float jumpTimer;
    private float fallingTimer;
    private float slideTimer;
    private bool jumpClash;
    private bool wait = true;

    // Start is called before the first frame update
    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.USE_BOOSTER, this);       
        prigidbody = GetComponent<Rigidbody>();
        pland = GetComponent<PLand>();        
        cameraController = mainCamera.GetComponent<CameraController>();
        playerAudio = GetComponent<AudioSource>();       
        speed = walkspeed;
        acceleration = 0.6f;
        modelY = transform.localScale.y / 2;
        jumpPower = 250f;
        slidePower = 50f;
        jumpCheck = false;
        slopeOffsetY = 2f;
        jumpTimer = 0f;
        fallingTimer = 0f;
        slideTimer = 0f;
        jumpClash = false;
        wait = false;
    }
    // Update is called once per frame
    

    public void Move(float moveX, float moveZ, PLAYER type)
    {
        //Debug.Log(speed);            

        if (type == PLAYER.RUN)
        {
            if (speed < runspeed)
                speed += acceleration;
            else if (speed > runspeed)
                speed = runspeed;

            if (pland.footstep != 0 && pland.soundTimer)
                pland.PlayLoadSound(0.42f);
        }
        else if (type == PLAYER.WALK)
        {
            acceleration = 0.6f;
            speed = walkspeed;
            if (pland.footstep != 0 && pland.soundTimer)
                pland.PlayLoadSound(0.6f);
        }
        // 카메라에 로컬좌표 오른쪽방향
        Vector3 mainR = mainCamera.transform.right;
        mainR.y = 0;
        // 카메라에 로컬좌표 앞방향
        Vector3 mainF = mainCamera.transform.forward;
        mainF.y = 0;
        //카메라에 기준해 방향키 값이 바뀐다
        moveDistance = (mainR * moveX) + (mainF * moveZ);
        //거리벡터 = 정규화시킨 거리벡터 * 시간 * 프레임률
        moveDistance = moveDistance.normalized * speed * Time.deltaTime;

        //Debug.Log(moveDistance);
        if (moveDistance != Vector3.zero)
        {
            //거리벡터 방향값
            Quaternion newRotation = Quaternion.LookRotation(moveDistance);
            //거리벡터 방향으로 회전보간
            prigidbody.rotation = Quaternion.Slerp(prigidbody.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
        CheckCollision();

        // 경사로 위로 진행
        float distanceS = Vector3.Distance(transform.position, (transform.position + moveDistance));
        // Debug.Log(" f: " + Vector3.Distance(transform.position, transform.position + (transform.forward * distanceS)) + " m: " +
        // Vector3.Distance(transform.position, (transform.position + moveDistance)));
        Vector3 raySlope = transform.position;
        raySlope.y = raySlope.y + 0.05f;
        Vector3 raySlopeE = transform.forward;
        raySlopeE.y = 0;
        RaycastHit slope;
        float triangleX, triangleY, triangleL;
        Vector3 dotA, dotB, dotC;
        Debug.DrawRay(raySlope, raySlopeE * 1f, Color.yellow);
        // 경사로 밑으로 진행          

        RaycastHit downSlope1, downSlope2;
        Vector3 rayDownSlope = transform.position + (transform.forward * distanceS);
        Debug.DrawRay(rayDownSlope, raySlopeE * 1f, Color.green);
        rayDownSlope.y = rayDownSlope.y;
        float triangleX_D, triangleY_D, triangleL_D;
        Vector3 dotA_D, dotB_D, dotC_D;

        //밑변 x 높이 y 빗변L과 점 a b c를 구한다 x y L에 비율을 계산 moveDistance와 y값을 수정
        //빗변을 이용해 경사로를 이동           
        if (Physics.Raycast(raySlope, raySlopeE, out slope, 1f))
        {
            Debug.Log("경사");
            //Debug.Log(" 비교: " + distanceS + " " + Vector3.Distance(slope.point, transform.position));
            if ((slope.collider.tag == "Slope_Grass" || slope.collider.tag == "Slope_Concrete")
                || (slope.collider.tag == "Slope_Stone" || slope.collider.tag == "Slope_Wood"))
            {

                dotA = slope.point;
                //Vector3 dotBRay = transform.position + ((slope.point - transform.position) * 2);
                Vector3 dotBRay = dotA + ((dotA - transform.position) * 2);
                triangleX = Vector3.Distance(dotA, dotBRay);
                Debug.DrawRay(slope.point, Vector3.up * 10f, Color.red);
                //Debug.DrawRay(dotBRay, Vector3.up * 10f, Color.blue);
                dotBRay.y = dotA.y;
                dotB = dotBRay;
                dotBRay.y = dotBRay.y + slopeOffsetY + 1f;
                if (Physics.Raycast(dotBRay, -Vector3.up, out slope, 3f))
                {
                    Debug.DrawRay(dotBRay, -Vector3.up * 2f, Color.yellow);
                    dotC = slope.point;
                    triangleY = Vector3.Distance(dotC, dotB);
                    triangleL = Vector3.Distance(dotC, dotA);
                    /*
                    Debug.Log("triX:" + triangleX  / triangleL + " triY:" + triangleY / triangleL + " tirL:" + triangleL / triangleL);
                    Debug.Log("SIN:" + triangleY / triangleL + " COS:" + triangleX / triangleL + " TAN:" + triangleY / triangleX);
                    Debug.Log("mD: "+ moveDistance +  " 밑변: "+ distanceS+" 높이: " + (distanceS / (triangleX / triangleL)) *(triangleY / triangleL) + 
                        " 빗변 " + distanceS/ (triangleX / triangleL));
                    */
                    float triBottom = distanceS;
                    float triHeight = (distanceS / (triangleX / triangleL)) * (triangleY / triangleL);
                    float hypotenuse = distanceS / (triangleX / triangleL);
                    // moveDistanc를 삼각형 빗변에  밑변에 비율만큼 으로 수정한 빗변길이
                    float modifiedMDL = hypotenuse * ((triBottom * triBottom) / (hypotenuse * hypotenuse));
                    float modifiedMDY = modifiedMDL * (triangleY / triangleL);                 
                    moveDistance.y = moveDistance.y + modifiedMDY;                 
                    if(moveDistance != Vector3.zero && !double.IsNaN(moveDistance.y))
                        prigidbody.MovePosition(transform.position + moveDistance);
                    Debug.DrawRay(dotA, (dotC - dotA) * 1f, Color.red);
                    Debug.DrawRay(dotB, (dotA - dotB) * 1f, Color.blue);
                    Debug.DrawRay(dotC, (dotB - dotC) * 1f, Color.black);
                }
                else
                    prigidbody.MovePosition(transform.position + moveDistance);
            }
            else
                prigidbody.MovePosition(transform.position + moveDistance);
        }
        else if (Physics.Raycast(rayDownSlope, -Vector3.up, out downSlope1, 1.0f) && downSlope1.point.y + 0.03f < transform.position.y)
        {
            if ((downSlope1.collider.tag == "Slope_Grass" || downSlope1.collider.tag == "Slope_Concrete")
                || (downSlope1.collider.tag == "Slope_Stone" || downSlope1.collider.tag == "Slope_Wood"))
            {
                Debug.Log("내리막 ");

                // Debug.Log("내리막 a" + downSlope1.collider.gameObject.name +" "+downSlope1.point.y +" <"+ transform.position.y);
                Debug.DrawRay(rayDownSlope, -Vector3.up * 1.0f, Color.red);
                //Debug.DrawRay(downSlope.point, -Vector3.right, Color.cyan);
                dotA_D = transform.position;
                dotC_D = downSlope1.point;
                dotB_D = downSlope1.point;
                dotB_D.y = transform.position.y;
                triangleX_D = Vector3.Distance(dotA_D, dotB_D);
                triangleY_D = Vector3.Distance(dotB_D, dotC_D);
                triangleL_D = Vector3.Distance(dotC_D, dotA_D);
                /* Debug.Log("triX: " + triangleX_D + " triY: " + triangleY_D + " triL: " + triangleL_D);
                 Debug.Log("sin: " + triangleY_D / triangleL_D + " cos: " + triangleX_D / triangleL_D + " tan: " + triangleY_D / triangleX_D);
                 Debug.Log("mD" + moveDistance + " 밑변: " + distanceS + " 높이: " + (triangleY_D / triangleL_D) * (distanceS / (triangleX_D / triangleL_D))
                     + " 빗변: " + distanceS / (triangleX_D / triangleL_D));
                 Debug.Log(moveDistance);
                 */
                Debug.DrawRay(dotA_D, (dotC_D - dotA_D) * 1f, Color.red);
                Debug.DrawRay(dotB_D, (dotA_D - dotB_D) * 1f, Color.blue);
                Debug.DrawRay(dotC_D, (dotB_D - dotC_D) * 1f, Color.green);
                float triBottom = distanceS;
                float triHeight = (distanceS / (triangleX_D / triangleL_D)) * (triangleY_D / triangleL_D);
                float hypotenuse = distanceS / (triangleX_D / triangleL_D);
                // moveDistanc를 삼각형 빗변에  밑변에 비율만큼 으로 수정한 빗변길이
                float modifiedMDL = hypotenuse * ((triBottom * triBottom) / (hypotenuse * hypotenuse));
                float modifiedMDY = modifiedMDL * (triangleY_D / triangleL_D);
                //Debug.Log("new L: " + modifiedMDL + " new Y " + modifiedMDY);
                //moveDistance = moveDistance * (modifiedMDL / distanceS);
                moveDistance.y = moveDistance.y - modifiedMDY;
                //moveDistance.y = moveDistance.y - triHeight;
                //Debug.Log("내리막 b" + downSlope1.point.y + "< " + dotA_D.y + " m.y" + moveDistance.y);
               if(moveDistance != Vector3.zero && !double.IsNaN(moveDistance.y))
                    prigidbody.MovePosition(transform.position + moveDistance);
                //Debug.Log("내리막높이: " + triHeight + " " + modifiedMDY);
                //Debug.Log("triX: " + triangleX_D/ triangleL_D + " triY: " + triangleY_D/triangleL_D + " triL: " + triangleL_D / triangleL_D);
            }
            else
                prigidbody.MovePosition(transform.position + moveDistance);
        }
        else
            prigidbody.MovePosition(transform.position + moveDistance);


        prigidbody.angularVelocity = Vector3.zero;
    }

    private void CheckCollision()
    {
        Vector3 rayStart = transform.position;
        rayStart.y += modelY * 2f - 1f;
        RaycastHit hit;
        /*Debug.DrawRay(rayStart, transform.forward * 3f, Color.blue);
       Debug.DrawRay(rayStart, (transform.forward + transform.right) *3f, Color.red);
       Debug.DrawRay(rayStart, (transform.forward + (-transform.right)) * 3f, Color.black);
       Debug.DrawRay(rayStart, transform.right * 3f, Color.yellow);
       Debug.DrawRay(rayStart, -transform.right * 3f, Color.cyan);*/
        // 건물 벽 등 오브젝트하고거리유지
        float space = 3.5f;
        if (Physics.Raycast(rayStart, Vector3.forward, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
        else if (Physics.Raycast(rayStart, -Vector3.forward, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
        else if (Physics.Raycast(rayStart, Vector3.right, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
        else if (Physics.Raycast(rayStart, -Vector3.right, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
        else if (Physics.Raycast(rayStart, (Vector3.forward + Vector3.right).normalized, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
        else if (Physics.Raycast(rayStart, (Vector3.forward + (-Vector3.right)).normalized, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
        else if (Physics.Raycast(rayStart, ((-Vector3.forward) + Vector3.right).normalized, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
        else if (Physics.Raycast(rayStart, ((-Vector3.forward) + (-Vector3.right)).normalized, out hit, space) && (hit.collider.tag != "Player"))
        {
            Vector3 hitDir = transform.position - hit.point;
            float positionHX = Mathf.Abs(hitDir.x);
            float positionHZ = Mathf.Abs(hitDir.z);
            if (positionHX > positionHZ)
            {
                if (Mathf.Abs(transform.forward.x + hitDir.x) < Mathf.Abs(hitDir.x))
                    moveDistance.x = 0;
            }
            else
            {
                if (Mathf.Abs(transform.forward.z + hitDir.z) < Mathf.Abs(hitDir.z))
                    moveDistance.z = 0;
            }
        }
    }
   
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.USE_BOOSTER)
        {
            StartCoroutine(useBooster());
        }
       
    }
    private IEnumerator useBooster()
    {
        Debug.Log("약시작");
        float standardWspeed, standardRspeed;
        standardWspeed = walkspeed;
        standardRspeed = runspeed;
        walkspeed *= 2;
        runspeed *= 2f;
        yield return new WaitForSeconds(15f);
        walkspeed = standardWspeed;
        runspeed = standardRspeed;
        Debug.Log("약끝");
    }
}
