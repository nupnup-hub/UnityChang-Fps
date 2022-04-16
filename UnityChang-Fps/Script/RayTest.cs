using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, obj.transform.position, out hit, 100f))
        {
            Debug.Log(hit.point);
            Debug.DrawRay(transform.position, obj.transform.position, Color.blue, 10f);
        }
    }
}
