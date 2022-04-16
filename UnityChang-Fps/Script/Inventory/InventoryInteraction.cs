using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SLOT
{
    ITEM,
    EQUIP
}
public class InventoryInteraction : MonoBehaviour
{
    public GameObject[] slot;
    public GameObject dragSlot;
    public GameObject slotFrame;
    public AudioClip rightClickSound;
    public AudioClip leftClickSound;
    public AudioClip dragDrop;
    private AudioSource audioSource;
    public Text tooltipText;
    public Image dropEffect;
    private Inventory inventory;
    private InventoryManager invenManager;
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private float distance;
    private bool oneClick, onDropIcon, inputDropIcon;
    private string clickSlot;
    private float[] height, width;
    private Dictionary<string, int> slotIndex = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Awake()
    {
        dragSlot.SetActive(false);
        slotFrame.SetActive(false);
        inventory = GetComponent<Inventory>();
        invenManager = GetComponent<InventoryManager>();       
        gr = GameObject.FindWithTag("Inventory").GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        distance = 10f;
        oneClick = onDropIcon = inputDropIcon = false;
        clickSlot = " ";
        height = new float[2];
        width = new float[2];
        height[0] = 118f;
        width[0] = 115f;
        height[1] = 130f;
        width[1] = 495f;
        MatchSlotAndIndex();
        for (int i = 0; i < slot.Length; i++)
            OffIcon(i);
    }
    void Start()
    {
        audioSource = GameObject.FindWithTag("ETCSound").GetComponent<AudioSource>();
    }
    void Update()
    {
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            //Debug.Log(results.Count);
            onDropIcon = false;
            for (int i = 0; i < results.Count; i++)
                if (results[i].gameObject.name == "Drop Icon")
                {
                    Debug.Log("당청");
                    onDropIcon = true;
                    if (Input.GetMouseButtonDown(0))
                    {
                        inputDropIcon = true;
                        audioSource.PlayOneShot(leftClickSound);
                        invenManager.SetDropCursor();
                        StartCoroutine(DropEffect());
                    }
                }
            if (!onDropIcon && !inputDropIcon)
            {
                OpenToolTip(obj);
                if (oneClick)
                {
                    if (results.Count > 1)
                    {
                        obj = results[1].gameObject;
                        FrameOn(slotIndex[obj.name]);
                    }
                }
                else
                    FrameOn(slotIndex[obj.name]);
                if (Input.GetMouseButtonDown(0) && !oneClick)
                {
                    audioSource.PlayOneShot(rightClickSound);
                    if (!inventory.EmptySlotCheck(slotIndex[obj.name]))
                    {
                        clickSlot = obj.name;
                        //Debug.Log(clickSlot + " click");
                        SetDragIcon(slotIndex[clickSlot]);
                        oneClick = true;
                    }
                }
                else if (Input.GetMouseButtonDown(1) && !oneClick)
                {
                    Debug.Log("사용");
                    audioSource.PlayOneShot(leftClickSound);
                    RightClick(slotIndex[obj.name]);
                }

                if (Input.GetMouseButtonUp(0) && oneClick)
                {
                    oneClick = false;
                    if (results.Count > 1)
                    {
                        obj = results[1].gameObject;
                        audioSource.PlayOneShot(dragDrop);
                        //Debug.Log("드래그 초이스" + obj.name);
                        SlotChange(slotIndex[clickSlot], slotIndex[obj.name]);
                        OffDragIcon(slotIndex[clickSlot]);
                    }
                    else
                    {
                        //Debug.Log("실패" + clickSlot);
                        SlotChangeFail(slotIndex[clickSlot]);
                    }
                }
            }
            else if (oneClick && onDropIcon)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log(clickSlot);
                    OffIcon(slotIndex[clickSlot]);
                    inventory.RemoveSlot(slotIndex[clickSlot]);
                    OffDragIcon(slotIndex[clickSlot]);
                    oneClick = false;
                }
            }
            if (inputDropIcon)
            {
                if (obj.name != "Drop Icon")
                    FrameOn(slotIndex[obj.name]);

                if (Input.GetMouseButtonUp(0))
                {
                    if (obj.name != "Drop Icon")
                    {
                        OffIcon(slotIndex[obj.name]);
                        inventory.RemoveSlot(slotIndex[obj.name]);
                        inputDropIcon = false;
                        invenManager.SetDefaultCursor();
                    }
                } 
            }
        }
        else
        {
            tooltipText.text = " ";
            slotFrame.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.I) || Input.GetMouseButtonUp(1))
        {
            inputDropIcon = false;
            invenManager.SetDefaultCursor();
        }
        if (oneClick)
            DragIcon();
    }
    //슬롯 초기화
    public void MatchSlotAndIndex()
    {
        for (int i = 0; i < slot.Length; i++)
            slotIndex.Add(slot[i].name, i);
    }
    public void RightClick(int i)
    {
        inventory.UseItem(i);
    }
    //아이콘 On
    public void SetIcon(int i, Sprite icon)
    {
        Image slotIcon = slot[i].GetComponent<Image>();
        slotIcon.sprite = icon;
        slotIcon.color = new Color(255, 255, 255, 255);
        slot[i].SetActive(true);
    }
    //아이콘 Off
    public void OffIcon(int i)
    {
        Image slotIcon;
        slotIcon = slot[i].GetComponent<Image>();
        slotIcon.sprite = null;
        slotIcon.color = new Color(0, 0, 0, 0);
    }
    //드래그 아이콘 On
    public void SetDragIcon(int i)
    {
        //Debug.Log("SetDrag");
        Image slotIcon = slot[i].GetComponent<Image>();
        Image dragIcon = dragSlot.GetComponent<Image>();
        dragIcon.sprite = slotIcon.sprite;
        dragIcon.color = slotIcon.color;
        dragSlot.transform.position = slot[i].transform.position;
        slotIcon.color = new Color(0, 0, 0, 0);
        dragIcon.raycastTarget = true;
        dragSlot.SetActive(true);
    }
    //드래그 아이콘 Off
    public void OffDragIcon(int i)
    {
        //초기화 하지 않아도 잘동작하나 찝찝하면 초기화 ㄱ
        //Debug.Log("offDrag");
        Image dragIcon = dragSlot.GetComponent<Image>();
        dragIcon.sprite = null;
        dragIcon.color = new Color(0, 0, 0, 0);
        dragIcon.raycastTarget = false;
        dragSlot.SetActive(false);
        //slot[i].SetActive(true);
    }
    //아이콘 드래그
    public void DragIcon()
    {
        dragSlot.transform.position = Input.mousePosition;
    }
    //슬롯 체인지
    public void SlotChange(int a, int b)
    {
        if (a != b && inventory.ChangeSlot(a, b))
        {
            Image slotIcon1 = slot[a].GetComponent<Image>();
            Image slotIcon2 = slot[b].GetComponent<Image>();
            Image dragIcon = dragSlot.GetComponent<Image>();
            slotIcon1.sprite = slotIcon2.sprite;
            slotIcon1.color = slotIcon2.color;
            slotIcon2.sprite = dragIcon.sprite;
            slotIcon2.color = dragIcon.color;
            if (slotIcon1.sprite == null)
                slotIcon1.color = new Color(0, 0, 0, 0);
        }
        else
            SlotChangeFail(a);
        //slot[b].SetActive(true);
    }
    //슬롯 체인지 실패
    public void SlotChangeFail(int i)
    {
        Image slotIcon = slot[i].GetComponent<Image>();
        Image dragIcon = dragSlot.GetComponent<Image>();
        slotIcon.color = dragIcon.color;
        //dragIcon.sprite = null;
        //dragIcon.color = new Color(0, 0, 0, 0);
        OffDragIcon(i);
    }
    //툴팁 On     
    public void OpenToolTip(GameObject obj)
    {

        if (obj.name == dragSlot.name)
        {
            //Debug.Log("dragIcon");
            return;
        }
        //Debug.Log(obj.name);
        //Debug.Log(slotIndex[obj.name]);
        string text = inventory.ItemTooltipCheck(slotIndex[obj.name]);
        if (text != " ")
        {
            //Debug.Log(obj.name + " " + "툴팁 오픈");
            tooltipText.text = text;
        }
    }
    //프레임 On
    public void FrameOn(int i)
    {
        slotFrame.transform.position = slot[i].transform.position;
        RectTransform rect = slotFrame.GetComponent<RectTransform>();
        if (i >= slot.Length - 2)
        {
            rect.sizeDelta = new Vector3(width[1], height[1]);
        }
        else
        {
            rect.sizeDelta = new Vector3(width[0], height[0]);
        }
        slotFrame.SetActive(true);
    }
    //DropEffect
    public IEnumerator DropEffect()
    {
        float a = 0;
        for (int i = 0; i < 10; i++)
        {
            a += 0.1f;
            dropEffect.color = new Color(dropEffect.color.r, dropEffect.color.g, dropEffect.color.b, a);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 10; i++)
        {
            a -= 0.1f;
            dropEffect.color = new Color(dropEffect.color.r, dropEffect.color.g, dropEffect.color.b, a);
            yield return new WaitForSeconds(0.001f);
        }
    }

}


