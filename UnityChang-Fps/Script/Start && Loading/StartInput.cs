using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartInput : MonoBehaviour
{
    public AudioClip startSound;
    private bool key;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        key = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(key);
        if (key)
        {           
            if(Input.anyKey)
            {
                audioSource.PlayOneShot(startSound);
                SceneManager.LoadScene("Loading");
            }
        }
    }
    public void SetBool()
    {
        //Debug.Log("변경");
        key = true;
    }
}
