using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlowerSpawner : MonoBehaviour
{
    

        private BoxCollider2D b_col;
        private float absBound_x, absBound_y;

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
        if(FlowerPool.Instance.GetActiveFlowers()<minFlowersOnScreen)
        {
            colorBuffer = (FlowerColor)Random.Range(0, System.Enum.GetValues(typeof(FlowerColor)).Length);// Seleccionamos un color aleatorio
            SpawnBufferFlower();
        }
        else if (FlowerPool.Instance.GetActiveFlowers() > maxFlowersOnScreen)
        {
            FlowerPool.Instance.flowerPool[FlowerPool.Instance.GetActiveFlowerIndex()].SetActive(false);
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
                    if (!FlowerPool.Instance.ScanColor(clr))
                    {
                        colorBuffer = clr;
                        Invoke("SpawnBufferFlower", rand_secs);
                        return;
                    }
                }

                colorBuffer = (FlowerColor)Random.Range(0, System.Enum.GetValues(typeof(FlowerColor)).Length);// Seleccionamos un color aleatorio
                Invoke("SpawnBufferFlower", rand_secs);
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

       private void SpawnFlower(FlowerColor clr, int petals, Vector2 pos)
        {
        GameObject go = FlowerPool.Instance.GetFlower();

        if (go == null)
        {
            Debug.LogError("Not flowers available in pool!");
            return;
        }
        else
        {
            go.transform.position = pos;
            go.GetComponent<FlowerBeh>().flower_clr = clr;
            go.GetComponent<FlowerBeh>().Petals = petals;

            go.SetActive(true);
        }
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

