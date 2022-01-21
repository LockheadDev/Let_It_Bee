using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeBeh : MonoBehaviour
{
    private LineRenderer path_lr;

    [SerializeField]
    private BeeState beeState;
    private bool at_destination = true;
    private Vector2 targetIdle;

    public List<FlowerColor> flwers_q;

    [SerializeField]
    private float movementSpeed = 10f;

    [SerializeField]
    private float idleMovementSpeed = 1f;


    private bool fire = false;

    [Space]
    [Header("GUI Bee Settings")]
    public List<Image> imgs = new List<Image>();
    private void OnEnable()
    {
        foreach (Image image in imgs)
        {
            image.gameObject.SetActive(false);
        }
        UpdateGUI();
    }
    void Update()
    {
       
        switch (beeState)
        {
            case BeeState.idle:
                at_destination = true;
                IdleBeh();
                break;
            case BeeState.following:
                fire = false;
                DepurePathTrails();
                FollowTrail();
                break;
            default:
                break;
        }
        if(at_destination==true)
        {
            if (!fire)
            {
                fire = true;
                targetIdle = BeeSpawner.Instance.GetRandomPos();
            }
            beeState = BeeState.idle;
        }
        else if(at_destination==false)
        {
            beeState = BeeState.following;
        }
        if(transform.GetComponentInChildren<LineRenderer>()!=false )//If we have children paths we follow them...
        {
            beeState = BeeState.following;
        }
    }
    void UpdateGUI()
    {
        Color temp_clr = Color.clear;
        for (int i = 0; i < flwers_q.Count; i++)
        {
            switch (flwers_q[i])
            {
                case FlowerColor.red:
                    temp_clr = Color.red;
                    break;
                case FlowerColor.blue:
                    temp_clr = Color.blue;
                    break;
                case FlowerColor.green:
                    temp_clr = Color.green;
                    break;
            }
            imgs[i].color = temp_clr;
            imgs[i].gameObject.SetActive(true);
        }
    }
    void IdleBeh()
    {
        float step_mov = idleMovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetIdle, step_mov);
        if ((Vector2)transform.position == targetIdle) fire = false;
    }

    void DepurePathTrails()
    {
        LineRenderer[] lr = transform.GetComponentsInChildren<LineRenderer>();
        if(lr.Length>1)
        {
            Destroy(lr[0].gameObject);
        }
    }
    void FollowTrail()
    {
        try { 
            path_lr = transform.GetComponentInChildren<LineRenderer>().gameObject.GetComponent<LineRenderer>();
            at_destination = false;

            Vector2 temp_vec = path_lr.GetPosition(0);
            Vector2 transform_position_v2 = transform.position;
            Vector2 final_pos_v2 = path_lr.GetPosition(path_lr.positionCount - 1);

            float step_mov = movementSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, temp_vec, step_mov);

            //TODO: Rotation
            /*
            Vector2 newRot = Vector3.RotateTowards(transform.up, temp_vec, singleStepRot, 0.0f);
            transform.LookAt(temp_vec,Vector3.up); 
            */

            //TODO: BUG that line cuts out
            if (transform_position_v2 == temp_vec)
            {
                path_lr.SetPosition(0, path_lr.GetPosition(1));
                path_lr.Simplify(0.01f);
            }

            if (transform_position_v2 == final_pos_v2)
            {
                at_destination = true;
                beeState = BeeState.idle;
                Destroy(path_lr.gameObject);
            }


        }
        catch
        {
            at_destination = true;
            beeState = BeeState.idle;
            Debug.Log(gameObject.name.ToString() + " " + gameObject.GetInstanceID().ToString() +" waiting for orders");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Flower")
        {
            FlowerBeh flower = collision.gameObject.GetComponent<FlowerBeh>();

            if (flwers_q.Count > 0)
            {
                if (flower.flower_clr == flwers_q[0])
                {
                    flower.DiscountPetals(1);
                    flwers_q.RemoveAt(0);
                }
            }

        }
        else if(collision.gameObject.tag == "Panal" && flwers_q.Count == 0)
        {

            //TODO Score Points on singleton
            if(gameObject.transform.childCount>0)
            {
                Destroy(gameObject.transform.GetComponentInChildren<LineRenderer>().gameObject);
            }
            gameObject.SetActive(false);
        }

    }

}
