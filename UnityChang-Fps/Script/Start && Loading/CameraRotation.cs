using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
   
    // Update is called once per frame
    void FixedUpdate()
    {
       transform.Rotate(0, 10f * Time.deltaTime, 0);
    }
}
