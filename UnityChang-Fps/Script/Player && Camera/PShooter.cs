using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PShooter : MonoBehaviour, EListener
{
    public GameObject[] guns;
    public GameObject pEtcSoundObject;
    public Text gunText;
    public Text ammoText;
    public List<PItem> equipGun = new List<PItem>();
    private Gun gun;
    private Inventory inven;
    private PGunSound pGunSound;
    private PAnimationController pAnimationController;
    private int equipSlotN;
    private int gunSlotIndex;
    private bool[] choiceGun;
    private bool wait, zoom, reloadDelay, activateGun;
    private int nowChoice;
    private float[] cycleTime;
    private float nowTime, nowFireTime, gunChangeDelay, reloadTime, reloadCycle, nowChangeTime;
    private int plusAmmo;
    // Start is called before the first frame update

    void Start()
    {
        wait = zoom = reloadDelay = activateGun = false;
        EventManager.Instance.AddListener(EVENT_TYPE.OPEN_INVENTORY, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOM, this);
        EventManager.Instance.AddListener(EVENT_TYPE.ZOOMOFF, this);
        gun = GameObject.FindWithTag("Guns").GetComponent<Gun>();
        inven = GameObject.FindWithTag("Inven").GetComponent<Inventory>();
        pGunSound = pEtcSoundObject.GetComponent<PGunSound>();
        pAnimationController = GetComponent<PAnimationController>();
        nowChangeTime = nowTime  = 0;
        gunChangeDelay = 0.8f;
        reloadCycle = 2.0f;
        plusAmmo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && nowChoice == 1 && nowChangeTime + gunChangeDelay < Time.time & !zoom)
            {
                ChangeSlot(1);
                nowChoice = 0;
                nowChangeTime = Time.time;
                ActvieEquipObject();
                ActivateEquip();
                if (reloadDelay)
                {
                    reloadDelay = false;
                    gun.CancelReload(zoom);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && nowChoice == 0 && nowChangeTime + gunChangeDelay < Time.time && !zoom)
            {
                ChangeSlot(2);
                nowChoice = 1;
                nowChangeTime = Time.time;
                ActvieEquipObject();
                ActivateEquip();
                if (reloadDelay)
                {
                    reloadDelay = false;
                    gun.CancelReload(zoom);
                }
            }

            if (equipGun[nowChoice] != null && equipGun[nowChoice].id != -1)
            {
                if (reloadDelay)
                {
                    if (reloadTime + reloadCycle < Time.time)
                    {
                        Debug.Log("장전끝");
                        pGunSound.PlayMagInSound(equipGun[nowChoice].id - 1001, 0);
                        equipGun[nowChoice].info["ammo"] += plusAmmo;
                        inven.MinusAmmo(equipGun[nowChoice].id + 1000, plusAmmo);
                        UpdateGunInfo();
                        reloadDelay = false;                                              
                        gun.CancelReload(zoom);
                    }                     
                }
                else if (((Input.GetKey(KeyCode.R) && equipGun[nowChoice].info["ammo"] < equipGun[nowChoice].info["maxAmmo"] )
                    || equipGun[nowChoice].info["ammo"] <= 0) && !reloadDelay)
                {
                    int restAmmo = inven.SearchAmmo(equipGun[nowChoice].id + 1000);
                    int needAmmo = equipGun[nowChoice].info["maxAmmo"] - equipGun[nowChoice].info["ammo"];
                    pAnimationController.CancelFire();
                    if (restAmmo > 0)
                    {
                        Debug.Log("장전");
                        reloadDelay = true;
                        reloadTime = Time.time;
                        if (restAmmo >= needAmmo)
                            plusAmmo = needAmmo;
                        else
                            plusAmmo = restAmmo;
                        gun.Reload();
                        pGunSound.PlayMagOutSound(equipGun[nowChoice].id - 1001, 0);
                    }
                }
                else if (zoom)
                {
                    if (equipGun[nowChoice].info["gunType"] == (int)GUNTYPE.AUTO && Input.GetMouseButton(0))
                    {
                        if (nowTime + cycleTime[nowChoice] < Time.time)
                        {
                            gun.Fire();
                            Debug.Log(equipGun[nowChoice].id);
                            pGunSound.PlayFireSound(equipGun[nowChoice].id - 1001, 0.5f);
                            equipGun[nowChoice].info["ammo"]--;
                            UpdateGunInfo();
                            nowTime = Time.time;
                        }
                    }
                    else if (equipGun[nowChoice].info["gunType"] == (int)GUNTYPE.SEMIAUTO && Input.GetMouseButtonDown(0))
                    {
                        if (nowTime + cycleTime[nowChoice] < Time.time)
                        {
                            gun.Fire();
                            Debug.Log(equipGun[nowChoice].id);
                            pGunSound.PlayFireSound(equipGun[nowChoice].id - 1001, 0.5f);
                            equipGun[nowChoice].info["ammo"]--;
                            UpdateGunInfo();
                            nowTime = Time.time;
                        }
                    }
                    else
                    {
                        gun.ReduceAim();
                    }
                }                 
            }
        }
    }

    public void Init(int n)
    {
        for (int i = 0; i < n; i++)
            equipGun.Add(null);
        for (int i = 0; i < 3; i++)
        {
            guns[i].SetActive(false);
        }
        equipSlotN = n;
        choiceGun = Enumerable.Repeat<bool>(false, equipSlotN).ToArray();
        cycleTime = Enumerable.Repeat<float>(0, equipSlotN).ToArray();
        //Debug.Log(choiceGun.Length);
        choiceGun[0] = true;
        gunSlotIndex = 0;
        nowChoice = 0;
    }
    public string CheckEquip()
    {
        if (equipGun[nowChoice] != null && equipGun[nowChoice].id != -1)
            return equipGun[nowChoice].name;
        else
            return "EMPTY";
    }
    //inventory에서 장비상태를 가져와 이전 장비를 비활성화하고 현장비를 활성화
    public void UpdateEquip(PItem item, int index)
    {
        if (choiceGun[index])
        {
            //Debug.Log("index" + index);
            if (equipGun[index] != null && equipGun[index].id != -1)
                DisarmGun(equipGun[index].id);
            if (item.id != -1)
            {
                //guns[item.id - 1001].SetActive(true);               
                gunSlotIndex = item.id - 1001;
                if (equipGun[index] != null && item.id != equipGun[index].id)
                    if (reloadDelay)
                    {
                        reloadDelay = false;
                        gun.CancelReload(zoom);
                    }
            }         
        }
        equipGun[index] = item;      
        //UpdateGunInfo();
    }
    //키보드 입력에 따라 장비 활성화
    public void ActvieEquipObject()
    {
        //for (int i = 0; i < guns.Length; i++)
        //Debug.Log("in " + gunSlotIndex);
        guns[gunSlotIndex].SetActive(false);

        for (int i = 0; i < choiceGun.Length; i++)
            if (choiceGun[i] && equipGun[i] != null && equipGun[i].id != -1)
            {
                //guns[equipGun[i].id - 1001].SetActive(true);
                gunSlotIndex = equipGun[i].id - 1001;
                //Debug.Log("active: " + equipGun[i].name + " 손" + i);
            }      
        UpdateGunInfo();
        gun.SendNowEquip(equipGun[nowChoice].id);
        if(equipGun[nowChoice].id != -1 && equipGun[nowChoice] !=null)
            gun.GunDamage(equipGun[nowChoice].info["attack"]);
        if(zoom)
            gun.OnAim();
    }
    //장비 활성화 해제
    public void DisarmGun(int i)
    {
        if (i != -1)
        {
            Debug.Log(guns[i - 1001].name + " 해재" );
            guns[i - 1001].SetActive(false);
            //StartCoroutine(ActiveTurm(i, false, 0.1f));
        }
    }
    public void ChangeSlot(int n)
    {
        n--;
        for (int i = 0; i < choiceGun.Length; i++)
        {
            if (i != n)
                choiceGun[i] = false;
            else if (i == n)
            {
                choiceGun[i] = true;
                //Debug.Log(n + " 번 손준비");
            }
        }
    }
    // 한발당 시간 간격 구하기
    public void SetGunFireCycle()
    {
        for (int i = 0; i < equipSlotN; i++)
            if (equipGun[i] != null && equipGun[i].id != -1)
                cycleTime[i] = 1 / (equipGun[i].info["rateOfFire"] / 60f);
    }
    public void UpdateGunInfo()
    {
        if (equipGun[nowChoice] == null || equipGun[nowChoice].id == -1)
        {
            gunText.text = " ";
            ammoText.text = " ";
            pAnimationController.ChangWeaponClip("EMPTY");
        }
        else
        {                    
            gunText.text = equipGun[nowChoice].name;
            int restAmmo = inven.SearchAmmo(equipGun[nowChoice].id + 1000);
            if (restAmmo == -1)
                restAmmo = 0;
            ammoText.text = equipGun[nowChoice].info["ammo"] + " / " + restAmmo;           
        }      
    }
    public void ActivateEquip()
    {
        if (equipGun[nowChoice] == null || equipGun[nowChoice].id == -1)
            pAnimationController.ChangWeaponClip("EMPTY");
        else
        {
            pAnimationController.ChangWeaponClip(equipGun[nowChoice].name);            
            StartCoroutine(ActiveTurm());            
        }
    }
   
    public IEnumerator ActiveTurm()
    {
        if (!activateGun)
        {
            activateGun = true;
            yield return new WaitForSeconds(0.7f);
            if (equipGun[nowChoice] != null && equipGun[nowChoice].id != -1)
                guns[equipGun[nowChoice].id - 1001].SetActive(true);
            activateGun = false;
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }                   
    }
    public void SendAmmoInfo(int num)
    {

    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.OPEN_INVENTORY)
        {
            if ((bool)Param)
                wait = true;
            else
            {
                wait = false;
                SetGunFireCycle();
                if (equipGun[nowChoice] != null)
                {
                    gun.SendNowEquip(equipGun[nowChoice].id);
                    gun.GunDamage(equipGun[nowChoice].info["attack"]);
                }
            }
        }
        else if (Event_Type == EVENT_TYPE.ZOOM)
        {
            zoom = true;
        }
        else if (Event_Type == EVENT_TYPE.ZOOMOFF)
        {
            zoom = false;
        }
    }
    public float GetDamage()
    {
        return equipGun[nowChoice].info["attack"];
    }
}
