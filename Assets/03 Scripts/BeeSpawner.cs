using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    public static BeeSpawner Instance;

    [Header("Spawner Settings")]
    [SerializeField]
    private int maxActiveBees = 10;
    [SerializeField]
    private int minActiveBees = 5;
    [SerializeField]
    private float minIntervalTime = 5f;
    [SerializeField]
    private float maxIntervalTime = 30f;
    [SerializeField]
    private float padd = 3f;

    [Space]

    [Header("Bee spawn settings")]
    [SerializeField]
    private int minFlowers_q = 1;
    [SerializeField]
    private int maxFlowers_q= 4;
    private BoxCollider2D b_col;
    private float absBound_x, absBound_y; //Idling bounds
    private float h, w;
    private int currentSection = 0;

    public float AbsBound_x { get => absBound_x; set => absBound_x = value; }
    public float AbsBound_y { get => absBound_y; set => absBound_y = value; }

    private void Start()
    {
        Instance = this;

        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        w = width / 2;
        h = height / 2;

        if (gameObject.TryGetComponent(typeof(BoxCollider2D), out Component component))
        {
            b_col = component.gameObject.GetComponent<BoxCollider2D>();
            absBound_x = b_col.size.x / 2;
            absBound_y = b_col.size.y / 2;
        }
        else Debug.Log("Not idling area found");
    }
    private void Update()
    {
        //Bees limitations at a time
        if (BeePool.Instance.GetActiveBees() < minActiveBees)
        {
            SpawnRandomBee();
        }
        else if (BeePool.Instance.GetActiveBees() > maxActiveBees)
        {
            BeePool.Instance.beePool.RemoveAt(BeePool.Instance.GetActiveBeeIndex());
            
        }

        //Invoke Random Bee
        float temp_sec = Random.Range(minIntervalTime,maxIntervalTime);
        if(!IsInvoking())
        {
            Invoke("SpawnRandomBee", temp_sec);
        }
    }
    private void SpawnRandomBee()
    {

        float pos_x = 0;
        float pos_y = 0;
        int latcher = Random.Range(0, 4);
        if (currentSection == latcher) latcher = 3; // Spawn on top..
        currentSection = latcher;
        switch (latcher)
        {
            case 0:
                pos_x = -w - padd;
                pos_y = Random.Range(-h, h);
                break;
            case 1:
                pos_x = w + padd;
                pos_y = Random.Range(-h, h);
                break;
            case 2:
                pos_y = -h - padd;
                pos_x = Random.Range(-w, w);
                break;
            case 3:
                pos_y = h + padd;
                pos_x = Random.Range(-w, w);
                break;
        }

        Vector2 randPos = new Vector2(pos_x, pos_y);
        List<FlowerColor> que = GetRandomColorList();

        SpawnBee(randPos,que);

    }
    private void SpawnBee(Vector2 pos,List<FlowerColor> que)
    {
        GameObject temp_go = BeePool.Instance.GetBee();
        temp_go.transform.position = pos;
        temp_go.GetComponent<BeeBeh>().flwers_q = que;
        temp_go.SetActive(true);
    }

    List<FlowerColor> GetRandomColorList()
    {
        List<FlowerColor> flwers_q = new List<FlowerColor>();
        int temp_rand_q_length = Random.Range(minFlowers_q, maxFlowers_q+1);
        for (int i = 0; i < temp_rand_q_length; i++)
        {
            label: 
            FlowerColor temp_color = RandomFlowerClr();
            if (i!=0)
            {
                if(temp_color == flwers_q[i-1])
                {
                    goto label;
                }
            }
            flwers_q.Add(temp_color);
        }
        return flwers_q;
    }
    private FlowerColor RandomFlowerClr()
    {
        int temp_int = Random.Range(0, 4);
        switch (temp_int)
        {
            case 1:
                return FlowerColor.red;
            case 2:
                return FlowerColor.green;
            case 3:
                return FlowerColor.blue;
        }
        return FlowerColor.red;

    }
    public Vector2 GetRandomPos()
    {
        float randPos_x = Random.Range(-absBound_x, absBound_x);
        float randPos_y = Random.Range(-absBound_y, absBound_y);
        Vector2 tempSpawnPoint = new Vector2(randPos_x, randPos_y); // Get random position
        return tempSpawnPoint;
    }
    private void OnDrawGizmos()
    {
        //Draw Spawning limits
        Vector2 top_left = new Vector2(-w, h);
        Vector2 top_right = new Vector2(w, h);
        Vector2 bot_left = new Vector2(-w, -h);
        Vector2 bot_right = new Vector2(w, -h);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(top_left, top_right);
        Gizmos.DrawLine(top_left, bot_left);
        Gizmos.DrawLine(bot_right, top_right);
        Gizmos.DrawLine(bot_right, bot_left);

        //Draw Idling area
         top_left = new Vector2(-absBound_x, absBound_y);
         top_right = new Vector2(absBound_x, absBound_y);
         bot_left = new Vector2(-absBound_x, -absBound_y);
         bot_right = new Vector2(absBound_x, -absBound_y);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(top_left, top_right);
        Gizmos.DrawLine(top_left, bot_left);
        Gizmos.DrawLine(bot_right, top_right);
        Gizmos.DrawLine(bot_right, bot_left);

    }
}