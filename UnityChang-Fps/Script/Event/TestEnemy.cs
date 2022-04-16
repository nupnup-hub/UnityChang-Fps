using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    
    private float damage = 40f;
    private void OnTriggerStay(Collider col)
    {
        Debug.Log("충돌" +col.gameObject.name);
        if (col.gameObject.tag == "Player")
        {
            PHealth target = col.gameObject.GetComponent<PHealth>();
            target.OnDamage(damage);
        }
    }
}
