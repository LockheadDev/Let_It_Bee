using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject PR_Bee;
    [Space]

    [Header("Pool Settings")]
    [SerializeField]
    private int beePoolSize = 20;
    private List<GameObject> beePool;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go_temp;
        for (int i = 0; i < beePoolSize; i++)
        {
            go_temp = Instantiate(PR_Bee);
            go_temp.SetActive(false);
            beePool.Add(PR_Bee);
        }

    }

    private void SpawnBee(int destinations, Vector2 pos)
    {
        try
        {
            GameObject temp_go = GetAvailableBee();

        }
        catch { Debug.Log("No bees available from pool!"); }
    }

    private GameObject GetAvailableBee()
    {
        foreach ( GameObject go in beePool)
        {
            if(!go.activeSelf)
            {
                return go;
            }
        }
        return null;
    }
    void Update()
    {
        
    }
}
