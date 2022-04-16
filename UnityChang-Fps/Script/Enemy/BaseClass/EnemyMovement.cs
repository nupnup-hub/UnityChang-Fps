using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    public bool isRun;
    public bool moveSlope;
    protected GameObject player;
    protected Rigidbody rigidbody;
    private Vector3 basePos;
    private float setDirTimer;
    private bool setDirKey;
    private Vector3[] norDir;
    private Vector3 dir;
    // Update is called once per frame

    public virtual void SetMovement()
    {
        if (setDirKey)
        {
            setDirKey = false;
            SetDir();
        }
        Vector3 moveDistance = dir * (speed * 0.5f) * Time.deltaTime;
        if (dir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDistance);
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, newRotation, 6f * Time.deltaTime);
        }
        MoveStair(dir);
    }
    public void SetDir()
    {
        int ran = Random.Range(0, 8);
        dir = norDir[ran];
        if (Vector3.Distance(basePos, transform.position) > 10f)
        {
            Vector3 xz = (basePos - transform.position);
            Vector3 x = new Vector3(xz.x, 0, 0).normalized;
            Vector3 z = new Vector3(0, 0, xz.z).normalized;
            xz = xz.normalized;
            ran = Random.Range(0, 7);
            if (ran == 0 && xz != Vector3.zero)
                dir = xz;
            else if (ran == 1 && x != Vector3.zero)
                dir = x;
            else if (ran == 2 && z != Vector3.zero)
                dir = z;
        }

        StartCoroutine(SetDirDelay());
    }
    public virtual void Init(float speed)
    {
        this.speed = speed;
        runSpeed = speed * 1.5f;
        isRun = false;
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody>();
        basePos = transform.position;
        setDirTimer = Time.time;
        setDirKey = true;
        norDir = new Vector3[8];
        norDir[0] = new Vector3(1, 0, 0);
        norDir[1] = new Vector3(-1, 0, 0);
        norDir[2] = new Vector3(0, 0, 1);
        norDir[3] = new Vector3(0, 0, -1);
        norDir[4] = new Vector3(0.71f, 0, 0.71f);
        norDir[5] = new Vector3(0.71f, 0, -0.71f);
        norDir[6] = new Vector3(-0.71f, 0, 0.71f);
        norDir[7] = new Vector3(-0.71f, 0, -0.71f);
    }
    public virtual bool Enter()
    {
        return true;
    }
    public virtual void MoveStair(Vector3 dis)
    {
        float slopeOffsetY = 2f;
        Vector3 moveDistance;
        if (dis == Vector3.zero)
            moveDistance = (player.transform.position - transform.position).normalized;
        else
            moveDistance = dir * (speed * 0.5f) * Time.deltaTime;
        float distanceS = Vector3.Distance(transform.position, (transform.position + moveDistance));
        Vector3 raySlope = transform.position;
        raySlope.y = raySlope.y + 0.05f;
        Vector3 raySlopeE = transform.forward;        
        raySlopeE.y = 0;
        RaycastHit slope;
        float triangleX, triangleY, triangleL;
        Vector3 dotA, dotB, dotC;

        RaycastHit downSlope1;
        Vector3 rayDownSlope = transform.position + (transform.forward * distanceS);
        float triangleX_D, triangleY_D, triangleL_D;
        Vector3 dotA_D, dotB_D, dotC_D;
       //Debug.Log("무브");
        //밑변 x 높이 y 빗변L과 점 a b c를 구한다 x y L에 비율을 계산 moveDistance와 y값을 수정
        //빗변을 이용해 경사로를 이동           
        if (Physics.Raycast(raySlope, raySlopeE, out slope, 1f))
        {
            //Debug.Log("위경사");
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
                    float triBottom = distanceS;
                    float triHeight = (distanceS / (triangleX / triangleL)) * (triangleY / triangleL);
                    float hypotenuse = distanceS / (triangleX / triangleL);
                    // moveDistanc를 삼각형 빗변에  밑변에 비율만큼 으로 수정한 빗변길이
                    float modifiedMDL = hypotenuse * ((triBottom * triBottom) / (hypotenuse * hypotenuse));
                    float modifiedMDY = modifiedMDL * (triangleY / triangleL);
                    moveDistance.y = moveDistance.y + modifiedMDY;
                    if (moveDistance != Vector3.zero && !double.IsNaN(moveDistance.y))
                        rigidbody.MovePosition(transform.position + moveDistance);
                    moveSlope = true;
                }
                else
                    rigidbody.MovePosition(transform.position + moveDistance);
            }
            else
                rigidbody.MovePosition(transform.position + moveDistance);
        }
        else if (Physics.Raycast(rayDownSlope, -Vector3.up, out downSlope1, 1.0f) && downSlope1.point.y + 0.03f < transform.position.y)
        {
            if ((downSlope1.collider.tag == "Slope_Grass" || downSlope1.collider.tag == "Slope_Concrete")
                || (downSlope1.collider.tag == "Slope_Stone" || downSlope1.collider.tag == "Slope_Wood"))
            {
                // Debug.Log("내리막 a" + downSlope1.collider.gameObject.name +" "+downSlope1.point.y +" <"+ transform.position.y);
                //Debug.DrawRay(rayDownSlope, -Vector3.up * 1.0f, Color.red);
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
                //Debug.DrawRay(dotA_D, (dotC_D - dotA_D) * 1f, Color.red);
                //Debug.DrawRay(dotB_D, (dotA_D - dotB_D) * 1f, Color.blue);
                //Debug.DrawRay(dotC_D, (dotB_D - dotC_D) * 1f, Color.green);
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
                if (moveDistance != Vector3.zero && !double.IsNaN(moveDistance.y))
                    rigidbody.MovePosition(transform.position + moveDistance);
                //Debug.Log("내리막높이: " + triHeight + " " + modifiedMDY);
                //Debug.Log("triX: " + triangleX_D/ triangleL_D + " triY: " + triangleY_D/triangleL_D + " triL: " + triangleL_D / triangleL_D);
                moveSlope = true;
            }
            else
                rigidbody.MovePosition(transform.position + moveDistance);
        }
        else
            rigidbody.MovePosition(transform.position + moveDistance);
    }
    public IEnumerator SetDirDelay()
    {
        //Debug.Log("딜레이");
        yield return new WaitForSeconds(3f);
        setDirKey = true;
    }
}
