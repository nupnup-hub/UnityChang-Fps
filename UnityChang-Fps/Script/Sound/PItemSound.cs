using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PItemSound : MonoBehaviour
{
    private ItemDatabase DB;
    private int size;
    private AudioSource audioSource;
    private AudioClip[] useSound;
    
    // Start is called before the first frame update
    void Start()
    {
        DB = GameObject.FindWithTag("Item Database").GetComponent<ItemDatabase>();
        size = DB.consumalbe.Length;
        audioSource = GetComponent<AudioSource>();
        int ammoCount = 0;
        for (int i = 0; i < size; i++)
            if (DB.consumalbe[i].itemId < 3001)
                ammoCount++;
        useSound = new AudioClip[size - ammoCount];
        SetSound();
    }
    public void SetSound()
    {
        for (int i = 0; i < size; i++)
        {
            if (DB.consumalbe[i].itemId > 3000)
            {
                int index = DB.consumalbe[i].itemId - 3001;
                useSound[index] = DB.consumalbe[i].useSound;
                Debug.Log("생성");
            }
        }
    }
    public void PlayUseSound(int i, float volume)
    {
        if(volume == 0)
            audioSource.PlayOneShot(useSound[i]);
        else
            audioSource.PlayOneShot(useSound[i], volume);
    }
}
