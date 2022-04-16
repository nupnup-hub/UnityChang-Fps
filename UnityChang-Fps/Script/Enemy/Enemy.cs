using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioClip[] damaged;
    private AudioSource audioSource;
    private bool shotgunSoundKey;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shotgunSoundKey = true;
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Damaged(int i, float volume)
    {
        if (i == 0)
            audioSource.PlayOneShot(damaged[0], volume);
        else if (i == 1)
            audioSource.PlayOneShot(damaged[1], volume);
        else if (i == 2 && shotgunSoundKey)
        {
            Debug.Log("재생");         
            audioSource.PlayOneShot(damaged[2], volume);
            audioSource.PlayOneShot(damaged[2], volume);        
            StartCoroutine(ShotgunSoundControl());
        }

        Debug.Log("피격");
    }
    public IEnumerator ShotgunSoundControl()
    {
        shotgunSoundKey = false;
        yield return new WaitForSeconds(0.01f);
        shotgunSoundKey = true;
    }
}
