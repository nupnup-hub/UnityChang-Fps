using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM
{
    GUN,
    CONSUMABLE, 
}
[System.Serializable]
public class PItem
{
    public string name;
    public int id;
    public string des;
    public Sprite icon;
    public ITEM type;
    public Dictionary<string, int> info = new Dictionary<string, int>();

    public PItem()
    {
        id = -1;       
    }
   
    public virtual void Reset()
    {
        this.name = " ";
        this.id = -1;
        this.des = " ";    
        this.icon = null;
        info.Clear();
    }
    public virtual int GetType() { return -1; }
    
    public virtual string Tooltip()
    {
        return " ";
    }
  
}


