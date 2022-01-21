using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPool : MonoBehaviour
{
    public static FlowerPool Instance;
    [SerializeField]
    private GameObject PR_Flower;

    [Space]

    [Header("Pool Settings")]
    [SerializeField]
    private int flowerPoolSize = 20;
    public List<GameObject> flowerPool;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;


        GameObject go_temp;
        for (int i = 0; i < flowerPoolSize; i++)
        {
            go_temp = Instantiate(PR_Flower);
            go_temp.transform.SetParent(transform);
            go_temp.SetActive(false);
            flowerPool.Add(go_temp);

        }
    }

    public GameObject GetFlower()
    {
        foreach (GameObject go in flowerPool)
        {
            if (!go.activeInHierarchy) return go;
        }
        return null;
    }
    public int GetActiveFlowers()
    {
        int temp = 0;
        foreach (GameObject go in flowerPool)
        {
            if (go.activeInHierarchy) temp++;
        }
        return temp;
    }
    public bool ScanColor(FlowerColor clr)
    {
        foreach (GameObject go in flowerPool)
        {
            if (go.activeInHierarchy && go.GetComponent<FlowerBeh>().flower_clr == clr) return true;
        }
        return false;
    }
    public int GetActiveFlowerIndex()
    {
        for (int i = 0; i < flowerPoolSize; i++)
        {
            if (flowerPool[i].activeInHierarchy) return i;
        }
        return -1;
    }

}
