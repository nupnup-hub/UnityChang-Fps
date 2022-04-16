using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEtcSound : MonoBehaviour, EListener
{
    public AudioClip getItem;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();       
    }
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SUCCESS_GET_ITEM, this);      
    }
    // Update is called once per frame
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        if (Event_Type == EVENT_TYPE.SUCCESS_GET_ITEM)
        {
            audioSource.PlayOneShot(getItem);
        }
    }
}