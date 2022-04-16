using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Gun Data", order = 2)]
public class WeaponSetting : ScriptableObject
{
    [Header("Settings of objects")]
    public string gunName;
    public int gunId;
    public string gunDes;
    public Sprite gunIcon;
    public ITEM type;
    public GUNTYPE gunType;
    public bool equiped;

    [Header("Weapon stats")]
    public int attack;
    [Tooltip("Current Bullet")]
    public int ammo;
    [Tooltip("Max Bullet")]
    public int maxAmmo;
    [Tooltip("Speed per minute")]
    public int rateOfFire;

    [Header("Weapon Sound")]
    public AudioClip fireSound;
    public AudioClip magOut;
    public AudioClip magIn;
    public AudioClip equipSound;
    public AudioClip holsterSound;
    public AudioClip swapSound;
    public List<AudioClip> bulletImpactSound = new List<AudioClip>();
}
