using System.Collections;
using UnityEngine;

public enum CONSUMABLETYPE
{
    MEDICAL,
    BOOSTER,
    AMMO
}
[System.Serializable]
public class Consumalbe : PItem
{

    public Consumalbe()
    {
        id = -1;
    }
    public void AddInfo()
    {
        info.Add("power", 0);
        info.Add("num", 0);
        info.Add("maxNum", 0);
        info.Add("itemType", 0);

    }
    public Consumalbe(PItem consumable)
    {
        AddInfo();
        this.name = consumable.name;
        this.id = consumable.id;
        this.des = consumable.des;
        this.icon = consumable.icon;
        this.type = consumable.type;
        //this.info = consumable.info;
        this.info["power"] = consumable.info["power"];
        this.info["maxNum"] = consumable.info["maxNum"];
        this.info["itemType"] = consumable.info["itemType"];
        this.info["num"] = 0;
    }
    public Consumalbe(ConsumableSetting consumable)
    {
        this.name = consumable.itemName;
        this.id = consumable.itemId;
        this.des = consumable.itemDes;
        this.icon = consumable.itemIcon;
        this.type = consumable.type;
        info["power"] = consumable.itemPower;
        info["num"] = consumable.itemNum;
        info["maxNum"] = consumable.itemMaxNum;
        info["itemType"] = (int)consumable.itemType;

    }
    public override int GetType()
    {
        return info["itemType"];
    }

    public override void Reset()
    {
        base.Reset();
        info["power"] = 0;
        info["num"] = 0;
        info["maxNum"] = 0;
    }
    public override string Tooltip()
    {
        string tooltip = "Name: <color=#ffffff>" + name + "</color>\nType: <color=#ffffff>" +
                                 +info["power"] + "</color>\nDes: <color=#ffffff>" + des +
                                "</color>\n소지수: <color=#ffffff>" + info["num"] + "</color>";
        return tooltip;
    }
    
}

