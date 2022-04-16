using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE
{  
    FALLING,
    ZOOM, 
    ZOOMOFF,
    RUN,
    RUNOFF,
    GAME_END,     
    OPEN_INVENTORY,
    STOP_GAME,
    AMMO_CHANGE,
    USE_BOOSTER,
    USE_AIDKIT,
    EQUIP,
    GET_ITEM,
    GET_ITEM_AMMO,
    GET_WEAPON,
    SUCCESS_GET_ITEM,
    DAMAGED,
    DEAD,
}
public interface EListener 
{
     void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);
}
