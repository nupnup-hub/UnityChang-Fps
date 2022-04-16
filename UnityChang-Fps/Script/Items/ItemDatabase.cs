using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<PItem> items = new List<PItem>();

    public WeaponSetting[] weapon;
    public ConsumableSetting[] consumalbe;

    void Awake()
    {
        int i = 0;
        for ( i = 0; i < weapon.Length; i++)
        {
            items.Add(new Weapon(weapon[i]));
            Debug.Log(i + " " + items[i].name);
        }
        for (int j = 0; j < consumalbe.Length; j++)
        {
            items.Add(new Consumalbe(consumalbe[j]));
            Debug.Log(i + " " + items[i + j].name);
        }
    }
    public PItem GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].id == id)
            {
                Debug.Log(items[i].info["num"] + " 개 소지중");
                if (items[i].type == ITEM.CONSUMABLE)
                    return new Consumalbe(items[i]);
                else
                    return new Weapon(items[i]);
            }
        //Debug.Log("item Search 실패");
        return new PItem();
    }
    public int GetId(string name)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].name == name)
            {
                //Debug.Log(items[i].Tooltip());
                return items[i].id;
            }
        return -1;
    }


}
