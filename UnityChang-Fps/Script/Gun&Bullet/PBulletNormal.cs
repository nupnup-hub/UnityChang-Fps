using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBulletNormal : MonoBehaviour
{
    //public AudioClip damagedSound;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 6f);
    }
    
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Enemy")
        {
            Debug.Log("적중");
            Destroy(gameObject);
            //damageAble
        }
    }
}
