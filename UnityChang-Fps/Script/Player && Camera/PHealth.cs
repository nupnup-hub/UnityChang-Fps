using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PHealth : PEntity, EListener
{
    private AudioSource playerAudio;
    public AudioClip[] damagedSound;
    public AudioClip dieSound;   
    public bool isNoDamage { get; private set;}
    public GameObject bloodCanvas;
    public Image bloodScreen;
    public GameObject deadScreen;
    public Image hpBar;   
    // Start is called before the first frame update

    public void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, this);
        EventManager.Instance.AddListener(EVENT_TYPE.DAMAGED, this);
        EventManager.Instance.AddListener(EVENT_TYPE.USE_AIDKIT, this);
        isNoDamage = false;
        bloodCanvas.SetActive(false);
        deadScreen.SetActive(false);
        bloodScreen.color = Color.clear;
        hpBar.fillAmount = 1f;
    }
    public override void OnDamage(float damage)
    {
        if (!isNoDamage && !isDie)
        {
            base.OnDamage(damage);
            Debug.Log(damage);        
            if (isDie)
            {
                Debug.Log("사망사운드on");
                EventManager.Instance.PostNotification(EVENT_TYPE.DEAD, this, healthPoint);
            }
            else if (!isDie)
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.DAMAGED, this, healthPoint);
                isNoDamage = true;              
            }
        }
        else
            Debug.Log("무적");
            
    }
    public virtual void RestoreHp(float figual)
    {
        Debug.Log(figual);
        base.RestoreHp(figual);
        StartCoroutine(PlusHpBar());
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        StartCoroutine(MinusHpBar());       
        if (Event_Type == EVENT_TYPE.DEAD)
        {
            Debug.Log("사망");
            playerAudio.PlayOneShot(dieSound);
            bloodScreen.color = new Color(1, 0, 0, 1f);
            bloodCanvas.SetActive(true);
            deadScreen.SetActive(true);
            //임시코드
            gameObject.SetActive(false);
        }
        else if (Event_Type == EVENT_TYPE.DAMAGED)
        {
            Debug.Log("피격!");
            int i = Random.Range(0, 3);
            playerAudio.PlayOneShot(damagedSound[i]);
            StartCoroutine(NoDamageTime());
        }
        else if(Event_Type == EVENT_TYPE.USE_AIDKIT)
        { 
            Debug.Log("회복!" + (float)Param);
            RestoreHp((float)Param);
        }
    }

    private IEnumerator NoDamageTime()
    {
        bloodCanvas.SetActive(true);
        for (int i = 1; i < 16; i++)
        {
            bloodScreen.color = new Color(1, 0, 0, (1f  - ((1f / 15f) * i)));
            yield return new WaitForSeconds(0.1f);
        }
        bloodCanvas.SetActive(false);
        bloodScreen.color = Color.clear;
        isNoDamage = false;
    }
    private IEnumerator PlusHpBar()
    {
        yield return new WaitForSeconds(0.05f);
        float nowHpBar = healthPoint / 100f;
        float plus = nowHpBar - hpBar.fillAmount;
        for (int i = 0; i < 10; i++)
        {
            hpBar.fillAmount = hpBar.fillAmount + (plus / 10f);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0f);
    }
    private IEnumerator MinusHpBar()
    {
        float nowHpBar = healthPoint / 100f;
        float minus = hpBar.fillAmount - nowHpBar;
        for (int i = 0; i < 10; i++)
        {
            hpBar.fillAmount = hpBar.fillAmount- (minus / 10f);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0f);
    }
    
}
