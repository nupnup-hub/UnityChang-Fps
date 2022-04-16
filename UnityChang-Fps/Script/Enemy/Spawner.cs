using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemys;
    private bool key = false;
    private void Start()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SetActive(false);          
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && key == false)
        {
            for(int i = 0; i < enemys.Length; i++)
            {
                enemys[i].SetActive(true);
                key = true;
                Destroy(this, 10f);
            }
        }
    }
}
