using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    #region 프로퍼티
    public static PlayerAnimationManager Instance
    {
        get { return instance; }
        set { }
    }
    #endregion 
    #region 변수
    private static PlayerAnimationManager instance = null;    
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(this);
    }
    
}
