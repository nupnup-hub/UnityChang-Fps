using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLETIMPACT
{
    CONCRETE,
    SAND,
    WOOD,
    BODY,
    HEAD
}
public class PGunSound : MonoBehaviour
{
    private ItemDatabase DB;
    private int size;
    private AudioSource audioSource;
    private AudioClip[] fireSound;
    private AudioClip[] magOut;
    private AudioClip[] magIn;
    private AudioClip[] equipSound;
    private AudioClip[] holsterSound;
    private AudioClip[] swapSound;
    // int = BULLETIMPACT , List = enum GUNNAME( GLOCK, AR15, SHOTGUN)
    private Dictionary<int, List<AudioClip>> bulletImpact;
    // Start is called before the first frame update
    void Start()
    {
        DB = GameObject.FindWithTag("Item Database").GetComponent<ItemDatabase>();
        size = DB.weapon.Length;
        audioSource = GetComponent<AudioSource>();
        fireSound = new AudioClip[size];
        magOut = new AudioClip[size];
        magIn = new AudioClip[size];
        equipSound = new AudioClip[size];
        holsterSound = new AudioClip[size];
        swapSound = new AudioClip[size];
        bulletImpact = new Dictionary<int, List<AudioClip>>();
        SetSound();
    }
    public void SetSound()
    {
        for (int i = 0; i < size; i++)
        {
            int index = DB.weapon[i].gunId - 1001;
            fireSound[index] = DB.weapon[i].fireSound;
            magOut[index] = DB.weapon[i].magOut;
            magIn[index] = DB.weapon[i].magIn;
            equipSound[index] = DB.weapon[i].equipSound;
            holsterSound[index] = DB.weapon[i].holsterSound;
            swapSound[index] = DB.weapon[i].swapSound;            
        }
        for(int i = 0; i < 5; i++)
        {
            bulletImpact.Add(i,new List<AudioClip>());
            bulletImpact[i].Add(DB.weapon[0].bulletImpactSound[i]);
            bulletImpact[i].Add(DB.weapon[1].bulletImpactSound[i]);
            bulletImpact[i].Add(DB.weapon[2].bulletImpactSound[i]);
        }        
    }
    public void PlayFireSound(int i, float volume)
    {
        if(volume == 0)
            audioSource.PlayOneShot(fireSound[i], 0.7f);
        else
            audioSource.PlayOneShot(fireSound[i], volume);
    }
    public void PlayMagOutSound(int i, float volume)
    {
        if (volume == 0)
            audioSource.PlayOneShot(magOut[i]);
        else
            audioSource.PlayOneShot(magOut[i], volume);
    }
    public void PlayMagInSound(int i, float volume)
    {
        if (volume == 0)
            audioSource.PlayOneShot(magIn[i]);
        else
            audioSource.PlayOneShot(magIn[i], volume);
    }  
    public void PlayEquipSound(int i, float volume)
    { 
        if (volume == 0)
            audioSource.PlayOneShot(equipSound[i]);
        else
            audioSource.PlayOneShot(equipSound[i], volume);
    }
    public void PlayHolsterSoundSound(int i, float volume)
    { 
        if (volume == 0)
            audioSource.PlayOneShot(holsterSound[i]);
        else
            audioSource.PlayOneShot(holsterSound[i], volume);
    }
    public void PlaySwapSound(int i, float volume)
    {
        if (volume == 0)
            audioSource.PlayOneShot(swapSound[i]);
        else
            audioSource.PlayOneShot(swapSound[i], volume);
    }
    public void PlayBulletImpactSound(int gunId, int soundId, float volume)
    {
        if (volume == 0)
            audioSource.PlayOneShot(bulletImpact[soundId][gunId]);
        else
            audioSource.PlayOneShot(bulletImpact[soundId][gunId], volume);
    }
}
