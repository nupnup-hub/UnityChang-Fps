using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum GUNNAME
{
    GLOCK,
    AR15,
    SHOTGUN
}

public class Gun : MonoBehaviour, EListener
{
    public GameObject[] muzzle;
    public GameObject[] aim;
    public GameObject[] aimUp;
    public GameObject[] aimDown;
    public GameObject[] aimRight;
    public GameObject[] aimLeft;
    public GameObject cam;
    public GameObject player;
    public int nowEquipId;
    public GameObject[] gunEffect;
    public GameObject damageEffectCanvas;
    public Image damageEffectImage;
    private BulletEffectSpawner bulletEffectSpawner;
    private Vector3[] aimUpOffset;
    private Vector3[] aimDownOffset;
    private Vector3[] aimRightOffset;
    private Vector3[] aimLeftOffset;
    private PAnimationController pAnimationController;
    private PGunSound pGunSound;  
      
    private bool fire, effect , nowShot;
    private float damage;
    void Awake()
    {
        AimInit();
        nowEquipId = -1;
        fire = effect =true;
        nowShot = false;
        damage = 0;
    }
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOM, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOMOFF, this);
        pAnimationController = player.GetComponent<PAnimationController>();
        bulletEffectSpawner = GameObject.FindWithTag("ImpactSpawner").GetComponent<BulletEffectSpawner>();
        pGunSound = GameObject.FindWithTag("ETCSound").GetComponent<PGunSound>();
     
    }
    public void Fire()
    {
        RaycastHit hit;
        int id = nowEquipId - 1001;
        pAnimationController.Fire();
        if (id != (int)GUNNAME.SHOTGUN)
        {
            nowShot = true;
            float x = (aimRight[id].transform.localPosition.x - aimLeft[id].transform.localPosition.x) / 5f;
            float y = (aimUp[id].transform.localPosition.y - aimDown[id].transform.localPosition.y) / 5f;
            x = Random.Range((-x / 2), x / 2);
            y = Random.Range((-y / 2), y / 2);
            Vector3 rayEnd = cam.transform.forward * 100;             
            //Debug.Log(rayEnd);
            rayEnd.x += x;
            rayEnd.y += y;
            //Debug.Log(x + " " + y + " " + rayEnd); 
            //rayEnd = rayEnd - muzzle[nowEquip.id - 1001].transform.position;
            Debug.DrawRay(muzzle[id].transform.position, rayEnd, Color.blue);
            if (!fire)
            {
                gunEffect[id].SetActive(true);
                StartCoroutine(GunEffect(id));
            }
            if (Physics.Raycast(cam.transform.position, rayEnd, out hit, 500.0f))
            {
                //Debug.Log("적중 " + hit.collider.gameObject.name);
             
                if (hit.collider.tag == "Enemy" || hit.collider.tag == "Head")
                {
                    pGunSound.PlayBulletImpactSound(id, (int)BULLETIMPACT.BODY, 1f);
                    bulletEffectSpawner.CreateEffect(0, hit.point, hit.normal);                 
                    Debug.Log(hit.collider.tag);
                    if (hit.collider.tag == "Head")
                    {
                        DamageAble damageAble =
                            hit.collider.gameObject.transform.GetComponent<DamageAble>();
                        Debug.Log (hit.collider.gameObject.transform.name);
                        damageAble.OnDamage(damage * 1.5f);
                        pGunSound.PlayBulletImpactSound(id, (int)BULLETIMPACT.HEAD, 1);
                    }                  
                    else
                    {
                        DamageAble damageAble 
                            = hit.collider.gameObject.GetComponent<DamageAble>();
                        damageAble.OnDamage(damage);
                        pGunSound.PlayBulletImpactSound(id, (int)BULLETIMPACT.BODY, 0);
                    }

                    if (effect)
                    {
                        effect = false;
                        StartCoroutine(GunEffect());
                    }
                }
                else if (hit.collider.tag == "Wood" || hit.collider.tag == "Slope_Wood")
                {
                    pGunSound.PlayBulletImpactSound(id, (int)BULLETIMPACT.WOOD, 0);
                    bulletEffectSpawner.CreateEffect(1, hit.point, hit.normal);
                }
                else if (hit.collider.tag == "Sand")
                {
                    pGunSound.PlayBulletImpactSound(id, (int)BULLETIMPACT.SAND, 0);
                    bulletEffectSpawner.CreateEffect(2, hit.point, hit.normal);
                }              
                else if (hit.collider.tag == "Concrete" || hit.collider.tag == "Slope_Concrete"
                    || hit.collider.tag == "Building" || hit.collider.tag == "Wall" || hit.collider.tag == "Slope_Concrete")
                {
                    pGunSound.PlayBulletImpactSound(id, (int)BULLETIMPACT.CONCRETE, 0);
                    bulletEffectSpawner.CreateEffect(4, hit.point, hit.normal);
                }
                //else if (hit.collider.tag == "Stone" || hit.collider.tag == "Slope_Stone")
                else
                {
                    pGunSound.PlayBulletImpactSound(id, (int)BULLETIMPACT.CONCRETE, 0);
                    bulletEffectSpawner.CreateEffect(3, hit.point, hit.normal);
                }
                Debug.DrawRay(hit.point,Vector3.up * 10 ,Color.red);                               
            }
            else
                Debug.Log("미적중");
            nowShot = false;
        }
        else
        {
            Vector3[] rayEnd = new Vector3[20];
            float insideX = ((aimRight[id].transform.localPosition.x - 10) - (aimLeft[id].transform.localPosition.x + 10)) / 5f;
            float insideY = ((aimUp[id].transform.localPosition.y - 10) - (aimDown[id].transform.localPosition.y + 10)) / 5f;
            //Debug.Log(insideX + "  in " + insideY);
            for (int i = 0; i < 5; i++)
            {
                float x = insideX;
                float y = insideY;
                rayEnd[i] = cam.transform.forward * 100;
                x = Random.Range((-x / 2), y / 2);
                y = Random.Range((-x / 2), y / 2);
                rayEnd[i].x += x;
                rayEnd[i].y += y;
            }
            insideX = ((aimRight[id].transform.localPosition.x + 10) - (aimLeft[id].transform.localPosition.x - 10)) / 5f;
            insideY = ((aimUp[id].transform.localPosition.y + 10) - (aimDown[id].transform.localPosition.y - 10)) / 5f;
            Debug.Log(insideX + " out " + insideY);
            for (int i = 0; i < 15; i++)
            {
                float x = insideX;
                float y = insideY;
                rayEnd[i + 5] = cam.transform.forward * 100;
                x = Random.Range((-x / 2), x / 2);
                y = Random.Range((-y / 2), y / 2);
                rayEnd[i + 5].x += x;
                rayEnd[i + 5].y += y;
            }

            for (int i = 0; i < 20; i++)
            {
                Debug.DrawRay(muzzle[id].transform.position, rayEnd[i], Color.blue);
                //Debug.Log(rayEnd[i]);
                if (Physics.Raycast(muzzle[id].transform.position, rayEnd[i], out hit, 100.0f))
                {
                    //테스트코드                   
                    //enemy.Damaged(nowEquip.name, 1f);
                }
            }

        }
        //Debug.Log("vertical" + aimUp[nowEquip.id - 1001].transform.localPosition.y +(-aimDown[nowEquip.id - 1001].transform.localPosition.y));    
        if (id != (int)GUNNAME.SHOTGUN)
            IncreaseAim(id);
    }
    public void Reload()
    {
        Debug.Log("reload");       
        pAnimationController.Reload();
    }
    public void CancelReload(bool zoom)
    {
        Debug.Log("cancel reload");
        pAnimationController.CancelReload(zoom);
    }
    public void OnAim()
    {
        if (nowEquipId != -1)
        {
            int i = nowEquipId - 1001;
            Debug.Log("i: " + i);
            aim[i].SetActive(true);
        }
    }
    public void OffAim()
    {
        int i = nowEquipId - 1001;
        aimUp[i].transform.localPosition = aimUpOffset[i];
        aimDown[i].transform.localPosition = aimDownOffset[i];
        aimRight[i].transform.localPosition = aimRightOffset[i];
        aimLeft[i].transform.localPosition = aimLeftOffset[i];
        aim[i].SetActive(false);
    }
    public void IncreaseAim(int id)
    {
        float offset = 0f;
        if (id == (int)GUNNAME.AR15)
            offset = 2.0f;
        else if (id == (int)GUNNAME.GLOCK)
            offset = 3.2f;

        Vector3 temp = aimUp[id].transform.position;
        temp.y += offset;
        aimUp[id].transform.position = temp;

        temp = aimDown[id].transform.position;
        temp.y -= offset;
        aimDown[id].transform.position = temp;

        temp = aimRight[id].transform.position;
        temp.x += offset;
        aimRight[id].transform.position = temp;

        temp = aimLeft[id].transform.position;
        temp.x -= offset;
        aimLeft[id].transform.position = temp;
    }
    public void ReduceAim()
    {
        pAnimationController.CancelFire();
        Vector3 temp;
        float offset = 0f;
        int i = nowEquipId - 1001;
        if (i == (int)GUNNAME.AR15)
            offset = 0.45f;
        else if (i == (int)GUNNAME.GLOCK)
            offset = 0.5f;

        if (aimUp[i].transform.localPosition.y > aimUpOffset[i].y)
        {
            temp = aimUp[i].transform.position;
            temp.y -= offset;
            aimUp[i].transform.position = temp;
        }
        if (aimDown[i].transform.localPosition.y < aimDownOffset[i].y)
        {
            temp = aimDown[i].transform.position;
            temp.y += offset;
            aimDown[i].transform.position = temp;
        }
        if (aimRight[i].transform.localPosition.x > aimRightOffset[i].x)
        {
            temp = aimRight[i].transform.position;
            temp.x -= offset;
            aimRight[i].transform.position = temp;
        }
        if (aimLeft[i].transform.localPosition.x < aimLeftOffset[i].x)
        {
            temp = aimLeft[i].transform.position;
            temp.x += offset;
            aimLeft[i].transform.position = temp;
        }
    }
    public void SendNowEquip(int id)
    {
        if (nowEquipId != -1)
            OffAim();
        nowEquipId = id;
    }
    public void AimInit()
    {
        aimUpOffset = new Vector3[aimUp.Length];
        aimDownOffset = new Vector3[aimDown.Length];
        aimRightOffset = new Vector3[aimRight.Length];
        aimLeftOffset = new Vector3[aimLeft.Length];
        for (int i = 0; i < aim.Length; i++)
        {
            aimUpOffset[i] = aimUp[i].transform.localPosition;
            aimDownOffset[i] = aimDown[i].transform.localPosition;
            aimRightOffset[i] = aimRight[i].transform.localPosition;
            aimLeftOffset[i] = aimLeft[i].transform.localPosition;
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.ZOOM)
        {
            if (nowEquipId != -1)
                OnAim();
        }
        else if (Event_Type == EVENT_TYPE.ZOOMOFF)
        { 
            OffAim();
        }
    }
    public IEnumerator GunEffect(int i)
    {
        if (!fire)
        {
            fire = true;
            yield return new WaitForSeconds(0.2f);
            gunEffect[i].SetActive(false);
            fire = false;
        }
        else
            yield return new WaitForSeconds(0);
    }
    public void GunDamage(float damage)
    {
        this.damage = damage;
    }
    private IEnumerator GunEffect()
    {
        damageEffectCanvas.SetActive(true);
        
        for (int i = 1; i < 20; i++)
        {
            damageEffectImage.color = new Color(damageEffectImage.color.r, damageEffectImage.color.g
                , damageEffectImage.color.b,(1f - ((1f / 20f) * i)));
            yield return new WaitForSeconds(0.001f);
            if(nowShot)
            {
                i = 1;
            }
        }
        effect = true;
        damageEffectCanvas.SetActive(false);             
        yield return new WaitForSeconds(0f);
    }
}
