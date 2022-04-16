using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour
{
    public  EnemySetting[] data;
    private static Dictionary<int, EnemyStatus> enemyData = new Dictionary<int, EnemyStatus>();
    public static Dictionary<int, EnemyStatus> Instance
    {
        get { return enemyData;  }
        private set { }   
    }
    private static EnemyDatabase enemyDatabase = null;
    public static EnemyDatabase InstanceDatabase
    {
        get { return enemyDatabase; }
        private set { }
    }
    void Awake()
    {
        if (enemyDatabase == null)
            enemyDatabase = this;

        if (enemyData == null)
            enemyData = new Dictionary<int, EnemyStatus>();

        for (int i = 0; i < data.Length; i++)
        {
            enemyData.Add(data[i].id - 5000, new EnemyStatus(data[i]));
        }
    } 
  

}
