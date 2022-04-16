using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AnimtationLayer
{
    public enum MOVEMNET
    {
         STANDING,
         WALKING,
         RUNNING,
         JUMPING,
         SLIDING
    }
    public enum AIMING
    {
        NONE = 1000,
        IDLE, 
        ZOOM,
        FIRE,
        RELOAD,
    } 
}
public enum GUN
{
    GLOCK17,
    AR15,
    SHOTGUN
}
public interface PlayerAnimationList  
{
    void OnAnimation(AnimtationLayer.MOVEMNET movement, AnimtationLayer.AIMING aiming, GUN gun, Component Sender, object Param = null);
}
