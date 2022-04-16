using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 1)]
public class ConsumableSetting : ScriptableObject
{
    [Header("Settings of objects")]
    public string itemName;
    public int itemId;
    public string itemDes;
    public Sprite itemIcon;
    public ITEM type;
    public CONSUMABLETYPE itemType;

    [Header("Item stats")]
    public int itemPower;
    [Tooltip("Current possession")]
    public int itemNum;
    [Tooltip("Maximum possession")]
    public int itemMaxNum;

    [Header("Item Sound")]
    public AudioClip useSound;   
}
