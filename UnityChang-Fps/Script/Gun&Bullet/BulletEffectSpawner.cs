using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EFFECT
{
    BLOOD,
    WOOD,
    METAL,
    SAND,
    STONE
}
public class BulletEffectSpawner : MonoBehaviour
{
    public GameObject[] bulletEffect;
    // Start is called before the first frame update
    
    void Start()
    {
        Init();
    }
    
    public void Init()
    {
        for (int i = 0; i < bulletEffect.Length; i++)
            bulletEffect[i].SetActive(false);
    }

    public void CreateEffect(int i, Vector3 pos , Vector3 nor)
    {
        GameObject copy = Instantiate(bulletEffect[i], pos, Quaternion.identity);
        copy.transform.rotation = Quaternion.FromToRotation(transform.forward, nor);
        copy.SetActive(true);
    }
    
}
