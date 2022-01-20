using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlowerSpawner : MonoBehaviour
{
        private BoxCollider2D b_col;
        private float absBound_x, absBound_y;

        [SerializeField]
        private GameObject PR_Flower;

        [Space]

        [Header("Pool Settings")]
        [SerializeField]
        private int flowerPoolSize = 20;
        public List<GameObject> flowerPool;
        [Space]

        [Header("Flower Settings")]
        [SerializeField]
        private int minPetals = 2;
        [SerializeField]
        private int maxPetals = 5;
        [SerializeField]
        private FlowerColor colorBuffer;

        [Space]

        [Header("Spawner Settings")]
        [SerializeField]
        private int maxFlowersOnScreen = 10;
        [SerializeField]
        private int minFlowersOnScreen = 5;
        [SerializeField]
        private float minIntervalSec = 1f;
        [SerializeField]
        private float maxIntervalSec = 10f;


    void Start()
    {
        GameObject go_temp;
        for (int i = 0; i < flowerPoolSize; i++)
        {
            go_temp = Instantiate(PR_Flower);
            go_temp.SetActive(false);
            flowerPool.Add(PR_Flower);
        }

        //Spawn Bounds
        if (gameObject.TryGetComponent(typeof(BoxCollider2D), out Component component))
        {
            b_col = component.gameObject.GetComponent<BoxCollider2D>();
            absBound_x = b_col.size.x / 2;
            absBound_y = b_col.size.y / 2;
        }
        else Debug.Log("Not spawning area found");

        
    }
        
        void Update()
        {
        
        if(transform.childCount<minFlowersOnScreen)
        {
            SpawnFlowerRand();
        }
        else if (transform.childCount>maxFlowersOnScreen)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        else
        {
            if (!IsInvoking()) 
            {
                //Escaneamos que flor no hay y colocamos ese color para que sea el siguiente en spawnear

                float rand_secs = Random.Range(minIntervalSec, maxIntervalSec);
                var values = System.Enum.GetValues(typeof(FlowerColor));
                foreach (FlowerColor clr in values)
                {
                    if (!ScanColor(clr))
                    {
                        colorBuffer = clr;
                        Invoke("SpawnBufferFlower", rand_secs);
                        return;
                    }
                }
                Invoke("SpawnFlowerRand", rand_secs);
            }

        }
        }

    private void SpawnBufferFlower()
    {
        int temp_petals = Random.Range(minPetals, maxPetals + 1);// Seleccionamos cantidad de petalos aleatorio
        float randPos_x = Random.Range(-absBound_x, absBound_x);
        float randPos_y = Random.Range(-absBound_y, absBound_y);
        Vector2 tempSpawnPoint = new Vector2(randPos_x, randPos_y); // Get random position

        SpawnFlower(colorBuffer, temp_petals, tempSpawnPoint);
    }
    private void SpawnFlowerRand()
        {
        int temp_petals = Random.Range(minPetals, maxPetals + 1);// Seleccionamos cantidad de petalos aleatorio

        FlowerColor temp_clr = (FlowerColor)Random.Range(0,System.Enum.GetValues(typeof(FlowerColor)).Length);// Seleccionamos un color aleatorio
            
        float randPos_x = Random.Range(-absBound_x, absBound_x);
        float randPos_y = Random.Range(-absBound_y, absBound_y);
        Vector2 tempSpawnPoint = new Vector2(randPos_x, randPos_y); // Get random position

        SpawnFlower(temp_clr, temp_petals, tempSpawnPoint);

        }

       private void SpawnFlower(FlowerColor clr, int petals, Vector2 pos)
        {
            PR_Flower.GetComponent<FlowerBeh>().flower_clr = clr;
            PR_Flower.GetComponent<FlowerBeh>().Petals = petals;

            GameObject go = Instantiate(PR_Flower, pos, Quaternion.identity);
            go.transform.SetParent(transform);

        }
    private bool ScanColor(FlowerColor clr)
    {
        foreach (Transform child in transform)
        {
            if(child.gameObject.GetComponent<FlowerBeh>().flower_clr== clr)return true;
        }
        return false;

    }
    private void OnDrawGizmos()
    {
        Vector2 top_left = new Vector2(-absBound_x, absBound_y);
        Vector2 top_right = new Vector2(absBound_x, absBound_y);
        Vector2 bot_left = new Vector2(-absBound_x, -absBound_y);
        Vector2 bot_right = new Vector2(absBound_x, -absBound_y);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(top_left, top_right);
        Gizmos.DrawLine(top_left, bot_left);
        Gizmos.DrawLine(bot_right, top_right);
        Gizmos.DrawLine(bot_right, bot_left);

    }
}

