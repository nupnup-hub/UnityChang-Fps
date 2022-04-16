using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
public class Inventory : MonoBehaviour, EListener
{
    public List<PItem> items = new List<PItem>();
    public GameObject dataBase;
    public GameObject pEtcSoundObject;
    private ItemDatabase DB;
    private InventoryInteraction invenInteraction;
    private PShooter pshooter;
    private AudioSource audioSource;
    private PGunSound pGunSound;
    private PItemSound pItemSound;
    private int equipNum, itemsNum;
    // Start is called before the first frame update
    private void Awake()
    {
        DB = dataBase.GetComponent<ItemDatabase>();
        invenInteraction = GetComponent<InventoryInteraction>();
        pshooter = GameObject.Find("unitychan").GetComponent<PShooter>();    
        CreateItemSlot();
        pshooter.Init(equipNum);
    }
    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.GET_ITEM, this);
        audioSource = pEtcSoundObject.GetComponent<AudioSource>();
        pGunSound = pEtcSoundObject.GetComponent<PGunSound>();
        pItemSound = pEtcSoundObject.GetComponent<PItemSound>();       
    }
    public void CreateItemSlot()
    {
        for (int i = 0; i < invenInteraction.slot.Length; i++)
        {
            items.Add(new PItem());
            //Debug.Log(items[i].ToString());
        }
        equipNum = 2;
        itemsNum = items.Count - equipNum;
    }  

    public bool AddItem(int id)
    {
        Debug.Log("Plus id: " + id);
        //아이템이 해당슬롯에 존재하며 갯수가 최대치 미만
        for (int i = 0; i < itemsNum; i++)
            if (items[i].id == id && items[i].info["num"] < items[i].info["maxNum"])
            {
                if (items[i].type == ITEM.CONSUMABLE)
                {
                    if (items[i].info["itemType"] == (int)CONSUMABLETYPE.AMMO)
                        items[i].info["num"] += items[i].info["maxNum"] / 4;
                    else
                        items[i].info["num"]++;

                    if (items[i].info["num"] > items[i].info["maxNum"])
                    {
                        int j = FindEmptySlot();
                        int plus = items[i].info["num"] - items[i].info["maxNum"];
                        if (j != -1)
                        {
                            items[j] = DB.GetItem(id);
                            items[j].info["num"] += plus;
                            invenInteraction.SetIcon(j, items[j].icon);
                            //Debug.Log(items[j].info["num"]+"개");
                        }
                        else
                        {
                            items[i].info["num"] = items[i].info["maxNum"];
                            return false;
                        }
                    }
                    if (items[itemsNum].id + 1000 == items[i].id || items[itemsNum + 1].id + 1000 == items[i].id)
                        pshooter.UpdateGunInfo();
                    return true;
                }               
            }

        //빈슬롯에 아이템 넣기
        int k = FindEmptySlot();
        if (k != -1)
        { 
            items[k] = DB.GetItem(id);
            //Debug.Log("new" + items[k].id);
            if (items[k].type == ITEM.CONSUMABLE)
            {
                if (items[k].info["itemType"] == (int)CONSUMABLETYPE.AMMO)
                { 
                    items[k].info["num"] += items[k].info["maxNum"] / 4;

                }
                else
                    items[k].info["num"]++;
                if (items[itemsNum].id + 1000 == items[k].id || items[itemsNum + 1].id + 1000 == items[k].id)
                    pshooter.UpdateGunInfo();
            }
            else
                items[k].info["num"]++;
            invenInteraction.SetIcon(k, items[k].icon);
            return true;
        }
        else
            return false;
    }
    
    public void UseItem(int i)
    {
        if (items[i].id == -1)
            return;

        if (items[i].type == ITEM.GUN)
            ChangeGun(i);
        else if (items[i].type == ITEM.CONSUMABLE)
        {
            if (items[i].GetType() != (int)CONSUMABLETYPE.AMMO)
            {
                //Debug.Log("use Item");
                if (items[i].GetType() == (int)CONSUMABLETYPE.MEDICAL)
                {
                    //Debug.Log("potion");
                    EventManager.Instance.PostNotification(EVENT_TYPE.USE_AIDKIT, this, (float)items[i].info["power"]);
                    pItemSound.PlayUseSound(items[i].id - 3001, 0);
                }
                else if (items[i].GetType() == (int)CONSUMABLETYPE.BOOSTER)
                {   
                    //Debug.Log("booster");
                    EventManager.Instance.PostNotification(EVENT_TYPE.USE_BOOSTER, this, items[i].info["power"]);
                    pItemSound.PlayUseSound(items[i].id - 3001, 0.5f);
                }                
                items[i].info["num"]--;
                if (items[i].info["num"] == 0)
                    RemoveSlot(i);                
            }
            else
                Debug.Log("is Ammo");
        }
    }
    public void ChangeGun(int i)
    {
        if (i >= itemsNum)
        {
            for (int j = 0; j < itemsNum; j++)
                if (items[j].id == -1)
                {
                    PItem temp = items[j];                   
                    items[j] = items[i];
                    items[i] = temp;
                    invenInteraction.OffIcon(i);
                    invenInteraction.SetIcon(j, items[j].icon);
                    pGunSound.PlayHolsterSoundSound(items[j].id - 1001, 0);
                    SendEquipInfo();
                    return;
                }          
        }
        else
        {
            PItem temp;
            for (int j = itemsNum; j < items.Count; j++)
                if (items[j].id == -1)
                {
                     temp = items[j];                    
                    items[j] = items[i];
                    items[i] = temp;
                    //items[j] = new Weapon(items[i]);
                    //items[i] = new PItem();
                    invenInteraction.OffIcon(i);
                    invenInteraction.SetIcon(j, items[j].icon);                    
                    pGunSound.PlayEquipSound(items[j].id - 1001,0);
                    SendEquipInfo();
                    //System.GC.Collect();
                    //Debug.Log("사용량: " + Profiler.GetMonoUsedSize());
                    return;
                }
            temp = items[i];
            items[i] = items[itemsNum];
            items[itemsNum] = temp;
            
            invenInteraction.SetIcon(i, items[i].icon);
            invenInteraction.SetIcon(itemsNum, items[itemsNum].icon);
            pGunSound.PlayEquipSound(items[itemsNum].id - 1001 , 0);
            SendEquipInfo();
            // 아이콘바꾸기
        }
        
    }

    public bool ChangeSlot(int a, int b)
    { 
        if (a >= itemsNum)
        {
            if (items[b].type != ITEM.GUN)
                return false;
        }
        else if (b >= itemsNum)
        {
            Debug.Log(items[a].type);
            if (items[a].type != ITEM.GUN)
                return false;
        }
        if (items[a].type == ITEM.GUN)
        {
            //Weapon temp = new Weapon();
            //temp.Copy(items[a]);
            PItem temp = items[a];
            items[a] = items[b];
            items[b] = temp;
            if (a >= itemsNum)
            {
                pGunSound.PlayHolsterSoundSound(items[b].id - 1001, 0);
            }
            else
            {
                pGunSound.PlayEquipSound(items[b].id - 1001, 0);
            }
        }
        else
        {
            //Consumalbe temp = new Consumalbe();
            //temp.Copy(items[a]);
            if (items[a].id == items[b].id && items[b].info["num"] < items[b].info["maxNum"])
            {
                //Debug.Log("교환?");
                items[b].info["num"] += items[a].info["num"];
                if (items[b].info["num"] > items[b].info["maxNum"])
                {
                    int rest = items[b].info["num"] - items[b].info["maxNum"];
                    items[b].info["num"] = items[a].info["maxNum"];
                    items[a].info["num"] = rest;
                }
                else
                    RemoveSlot(a);
            }
            else
            {
                PItem temp = items[a];
                items[a] = items[b];
                items[b] = temp;
            }
        }
        SendEquipInfo();
        return true;
    }
    public int SearchAmmo(int id)
    {
        for (int i = 0; i < itemsNum; i++)
            if (items[i].id == id)
                if (items[i].info["num"] > 0)
                    return items[i].info["num"];
        return -1;
    }
    public void MinusAmmo(int id, int n)
    {
        for (int i = 0; i < itemsNum; i++)
            if (items[i].id == id)
            {
                items[i].info["num"] -= n;
                if (items[i].info["num"] <= 0)
                    RemoveSlot(i);
            }
    }
    public void RemoveSlot(int i)
    { 
        //Debug.Log(items[i].id);
        items[i] = new PItem();
        invenInteraction.OffIcon(i);

        SendEquipInfo();
    }
    public void SendEquipInfo()
    {
        Debug.Log("sendInfo");
        pshooter.UpdateEquip(items[itemsNum], 0);
        pshooter.UpdateEquip(items[itemsNum + 1], 1);
        pshooter.UpdateGunInfo();
        pshooter.ActivateEquip();
    }
    public string ItemTooltipCheck(int i)
    {
        //Debug.Log("ToolTIp "+ items[i].id);
        if (items[i].id != -1)
            return items[i].Tooltip();
        else
            return " ";
    }
    public bool EmptySlotCheck(int i)
    {
        if (items[i].id != -1)
            return false;
        else
            return true;
    }
    public int FindEmptySlot()
    {
        for (int i = 0; i < itemsNum; i++)
            if (items[i].id == -1)
                return i;
        return -1;
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.GET_ITEM)
        {
            int id = DB.GetId((string)Param);
            if (id != -1)
            {
                if (AddItem(id))
                    EventManager.Instance.PostNotification(EVENT_TYPE.SUCCESS_GET_ITEM, this, true);
                else
                    EventManager.Instance.PostNotification(EVENT_TYPE.SUCCESS_GET_ITEM, this, false);
            }

        }
    }
}
