using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePool : MonoBehaviour
{
    public static BeePool Instance;
    [SerializeField]
    private GameObject PR_Bee;

    [Space]

    [Header("Pool Settings")]
    [SerializeField]
    private int beePoolSize = 20;
    public List<GameObject> beePool;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;


        GameObject go_temp;
        for (int i = 0; i < beePoolSize; i++)
        {
            go_temp = Instantiate(PR_Bee);
            go_temp.transform.SetParent(transform);
            go_temp.SetActive(false);
            beePool.Add(go_temp);

        }
    }

    public GameObject GetBee()
    {
        foreach (GameObject go in beePool)
        {
            if (!go.activeInHierarchy) return go;
        }
        return null;
    }
    public int GetActiveBees()
    {
        int temp = 0;
        foreach (GameObject go in beePool)
        {
            if (go.activeInHierarchy) temp++;
        }
        return temp;
    }
    public int GetActiveBeeIndex()
    {
        for (int i = 0; i < beePoolSize; i++)
        {
            if (beePool[i].activeInHierarchy) return i;
        }
        return -1;
    }

}