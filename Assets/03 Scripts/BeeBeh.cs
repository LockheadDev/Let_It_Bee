using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBeh : MonoBehaviour
{
    private LineRenderer path_lr;

    [SerializeField]
    private bool at_destination = true;
    public List<FlowerColor> flwers_q;

    private float movement_speed = 0.01f;

    private void Start()
    {
        int temp_rand_q_length = Random.Range(1, 5);
        for (int i = 0; i < temp_rand_q_length; i++)
        {
            flwers_q.Add(RandomFlowerClr());
        }

    }
    void Update()
    {
        DepurePathTrails();
        FollowTrail();
    }

    void DepurePathTrails()
    {
        foreach (Transform child in transform)
        {
            if (transform.childCount > 1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }

    void FollowTrail()
    {
        try
        {
            at_destination = false;
            path_lr = transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
            Vector2 temp_vec = path_lr.GetPosition(0);
            Vector2 transform_position_v2 = transform.position;
            Vector2 final_pos_v2 = path_lr.GetPosition(path_lr.positionCount-1);
            
            transform.position = Vector2.MoveTowards(transform.position, temp_vec, movement_speed);
            
            //TODO: Rotation
            /*
            Vector2 newRot = Vector3.RotateTowards(transform.up, temp_vec, singleStepRot, 0.0f);
            transform.LookAt(temp_vec,Vector3.up); 
            */
            
            //TODO: BUG that line cuts out
            if(transform_position_v2 == temp_vec)
            {
                path_lr.SetPosition(0, path_lr.GetPosition(1));
                path_lr.Simplify(0.00001f);
            }

            if (transform_position_v2 == final_pos_v2)
            {
                at_destination = true;
                Destroy(path_lr.gameObject);
            }


        }
        catch
        {
            Debug.Log("No path found!");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Flower")
        {
            FlowerBeh flower = collision.gameObject.GetComponent<FlowerBeh>();
            if(flower.flower_clr==flwers_q[0])
            {
                flower.DiscountPetals(1);
                flwers_q.RemoveAt(0);
            }

        }
        else if(collision.gameObject.tag == "Panal" && flwers_q.Count == 0)
        {
            //TODO Score Points on singleton

            //TODO Object pooling
            Destroy(gameObject);
        }

    }

    #region FlowerBehaviour
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

    #endregion
}
