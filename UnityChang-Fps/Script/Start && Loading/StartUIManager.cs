using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartUIManager : MonoBehaviour
{
    public Text[] title;
    public Text press;
    public Texture2D dropCursor;
    public Image logo;
    public AudioClip titleSound;
    public AudioClip titleSoundLast;
    public AudioClip bgm;
    private CursorMode cursorMode;
    private Vector2 hotSpot;
    private StartInput startInput;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        startInput = GetComponent<StartInput>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        SetDropCursor();
        for (int i = 0; i < title.Length; i++)
            title[i].enabled = false;
        logo.enabled = false;
        press.enabled = false;
        ShowTitle();
    }

    // Update is called once per frame
    public void SetDropCursor()
    {
        cursorMode = CursorMode.Auto;
        hotSpot = Vector2.zero;
        Cursor.SetCursor(dropCursor, hotSpot, cursorMode);
    }
    public void ShowTitle()
    {
        for (int i = 0; i < title.Length; i++)
        {
            StartCoroutine(ShowText(i, 1.5f + (0.3f * (i + 1))));
            if (i == title.Length - 1)
            {
                StartCoroutine(ShowPressText(1.5f + (0.3f * (i + 1))));
                StartCoroutine(ShowLogo(1.5f + (0.3f * (i + 1))));
            }          
        }
    }
    public IEnumerator ShowText(int i, float stTime)
    {
        float r = title[i].color.r;
        float g = title[i].color.g;
        float b = title[i].color.b;
        title[i].color = new Color(r, g, b, 0);
        title[i].enabled = true;
        yield return new WaitForSeconds(stTime);
        for (int j = 0; j < 10; j++)
        {
            title[i].color = new Color(r, g, b, 0.1f * (j + 1));
            yield return new WaitForSeconds(0.015f);
        }
        if (i == 11)
        {
            audioSource.PlayOneShot(titleSoundLast);
            yield return new WaitForSeconds(1f);
            audioSource.Play();
            audioSource.loop = true;
        }
        else
            audioSource.PlayOneShot(titleSound, 0.8f);
    }
    public IEnumerator ShowPressText(float stTime)
    {
        float r = press.color.r;
        float g = press.color.g;
        float b = press.color.b;
        press.color = new Color(r, g, b, 0);
        press.enabled = true;
        yield return new WaitForSeconds(0.5f + stTime);
        for (int j = 0; j < 10; j++)
        {
            press.color = new Color(r, g, b, 0.1f * (j + 1));
            yield return new WaitForSeconds(0.025f);
        }
        startInput.SetBool();
        StartCoroutine(PressTextEffect());
    }
    public IEnumerator ShowLogo(float stTime)
    {
        float r = logo.color.r;
        float g = logo.color.g;
        float b = logo.color.b;
        logo.color = new Color(r, g, b, 0);
        logo.enabled = true;
        yield return new WaitForSeconds(0.25f + stTime);
        for (int j = 0; j < 5; j++)
        {
            logo.color = new Color(r, g, b, 0.2f * (j + 1));
            yield return new WaitForSeconds(0.025f);
        }
    }
    public IEnumerator PressTextEffect()
    {
        float scaleX = press.transform.localScale.x;
        float scaleY = press.transform.localScale.y;
        float scaleZ = press.transform.localScale.z;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 100; i++)
        {
            float st = scaleX;
            for (int j = 0; j < 25; j++)
            {
                scaleX = st - (j + 1) * 0.004f;
                scaleY = st - (j + 1) * 0.004f;
                press.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                yield return new WaitForSeconds(0.03f);
            }
            yield return new WaitForSeconds(0.1f);
            float mid = scaleX;
            for (int j = 0; j < 25; j++)
            {
                scaleX = mid + (j + 1) * 0.004f;
                scaleY = mid + (j + 1) * 0.004f;
                press.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                yield return new WaitForSeconds(0.03f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }


}
