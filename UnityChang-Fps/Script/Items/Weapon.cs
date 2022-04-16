using System.Collections;
using UnityEngine;

public enum GUNTYPE
{
    AUTO,
    SEMIAUTO
}
[System.Serializable]
public class Weapon : PItem
{  
   
    public Weapon()
    {
        id = -1;       
    }
    public Weapon(PItem weapon)
    {
        AddInfo();
        this.name = weapon.name;
        this.id = weapon.id;
        this.des = weapon.des;
        this.icon = weapon.icon;
        this.type = weapon.type;       
        this.info["attack"] = weapon.info["attack"];
        this.info["ammo"] = weapon.info["ammo"];
        this.info["maxAmmo"] = weapon.info["maxAmmo"];
        this.info["rateOfFire"] = weapon.info["rateOfFire"];
        this.info["gunType"] = weapon.info["gunType"];
        this.info["equiped"] = 0;
        this.info["num"] = 0;
        this.info["maxNum"] = 1;
    }
    public void AddInfo()
    {
        info.Add("attack", 0);
        info.Add("ammo", 0);
        info.Add("maxAmmo", 0);
        info.Add("rateOfFire", 0);
        info.Add("gunType", 0);
        info.Add("equiped", 0);
    }
    public Weapon(WeaponSetting weapon)
    {
        AddInfo();
        this.name = weapon.gunName;
        this.id = weapon.gunId;
        this.des = weapon.gunDes;
        this.icon = weapon.gunIcon;
        this.type = weapon.type;
        info["attack"] = weapon.attack;
        info["ammo"] = weapon.ammo;
        info["maxAmmo"] = weapon.maxAmmo;
        info["rateOfFire"] = weapon.rateOfFire;
        info["gunType"] = (int)weapon.gunType;
        info["equiped"] = 0;
        info["num"] = 0;
        info["maxNum"] = 1;
    }
    
    public override int GetType()
    {
        return info["gunType"];
    }

    public override void Reset()
    {
        base.Reset();
        info["attack"] = 0;
        info["ammo"] = 0;
        info["maxAmmo"] = 0;
        info["rateOfFire"] = 0;      
        info["equiped"] = 0;
        info["num"] = 0;
    }
 
    public override string Tooltip()
    {
        string tooltip = "Name: <color=#ffffff>" + name + "</color>\nAttack: <color=#ffffff>" +
                                info["attack"] + "</color>\nAmmo: <color=#ffffff>" + info["ammo"] + "</color>\nDes: <color=#ffffff>" + des + "</color>";
        return tooltip;
    }
    
}
