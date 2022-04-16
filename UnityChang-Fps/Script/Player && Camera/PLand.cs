    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공중인지 아닌지 판단 그리고 이동시 발소리 판단해서 PFootStep호출 
public class PLand : MonoBehaviour
{
    //public bool isLand { get;  set; }
    public int footstep { get; set; }
    public GameObject Shoes;
    private AudioSource loadAudio;
    public AudioClip[] loadSounds;
    public bool soundTimer , isFall;
    private RaycastHit hit;
    private int nowClip;
    private float fallTimer;
    // 0. noting 1. stone 2.grass 3. concrete   
    // Start is called before the first frame update
    void Awake()
    {
        //isLand = true;
        footstep = 0;
        loadAudio = Shoes.GetComponent<AudioSource>();
        soundTimer = true;
        nowClip = -1;
        isFall = false;
        fallTimer = 0;
    }
    private void FixedUpdate()
    {
        PlayerAirCheck();
    }
    private void PlayerAirCheck()
    {
        Vector3 rayStart = transform.position;
        rayStart.y += 0.1f;
        Vector3 rayEnd = -Vector3.up;      
        if (Physics.Raycast(rayStart, rayEnd, out hit, 1f) && (hit.collider.tag != "Player"))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.tag == "Grass" || hit.collider.tag == "Slope_Grass")
                footstep = 1;
            else if (hit.collider.tag == "Concrete" || hit.collider.tag == "Slope_Concrete")
                footstep = 2;
            else if (hit.collider.tag == "Stone" || hit.collider.tag == "Slope_Stone")
                footstep = 3;
            else if (hit.collider.tag == "Wood" || hit.collider.tag == "Slope_Wood")
                footstep = 4;
            else if (hit.collider.tag == "Sand" )
                footstep = 5;
            else
                footstep = 0;

            nowClip = footstep;
            fallTimer = 0;
            //Debug.Log("땅");
            if (isFall)
            {
                isFall = false;
                EventManager.Instance.PostNotification(EVENT_TYPE.FALLING, this, false);
            }
        }
        else
        {
            footstep = 0;
            fallTimer += 1f;
            //Debug.Log("공중");
        }
        if(fallTimer > 10)
        {
            isFall = true;
            EventManager.Instance.PostNotification(EVENT_TYPE.FALLING, this, true);
        }
        Debug.DrawRay(rayStart, rayEnd * 1f, Color.blue);
    }   
  
    public void PlayLoadSound(float timer)
    {   
        StartCoroutine(PlayAudio(timer));
    }

    private IEnumerator PlayAudio(float timer)
    {                
        loadAudio.PlayOneShot(loadSounds[footstep-1]);       
        soundTimer = false;        
        yield return new WaitForSeconds(timer);
        soundTimer = true;
    }
}
