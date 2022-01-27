using System.Collections;
using TMPro;
using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeBeh : MonoBehaviour
{
    [Header("Bee Status")]
    [SerializeField]
    private BeeState beeState;
    [SerializeField]
    private bool at_destination;
    public List<FlowerColor> flwers_q;

    [Space]
    
    [Header("Bee settings")]
    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private float idleMovementSpeed = 1f;
    [SerializeField]
    private int scoreValue = 10;

    [Space]

    [Header("GUI Bee Settings")]
    public List<Image> imgs = new List<Image>();

    [Header("Feedbacks")]
    [SerializeField]
    private MMFeedbacks DamageFeedback;

    //Bee behaviour
    private Vector2 targetIdle;
    private bool lookinRight = true;
    private Vector2 temp_vec;

    //Input Beh
    private bool fire = false;
    private int beeLinePos = 0;

    //Line Beh
        //-FIFO structure for always having just one path
    private Queue<LineRenderer> lineRenderers = new Queue<LineRenderer>();
    private LineRenderer lrPath;

    public bool LookinRight { get => lookinRight; set => lookinRight = value; }

    private void Start()
    {
        beeLinePos = 0;
        beeState = BeeState.idle;
        fire = false;
        at_destination = true;
        DamageFeedback.GetComponentInChildren<MMFeedbackTMPColor>().TargetTMPText = GameObject.Find("ScoreNum").gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        //Sanity variable restart 
        Start();
        //GUI Handlers
        foreach (Image image in imgs)
        {
            image.gameObject.SetActive(false);
        }
        UpdateGUI();
       
    }
    void Update()
    {
        UpdateGUI();
        switch (beeState)
        {
            case BeeState.stopped:
                
                break;
            case BeeState.idle:
                at_destination = true;
                IdleBeh();
                break;
            case BeeState.following:
                fire = false;
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
        //Clear Images
        foreach (Image img in imgs)
        {
            img.gameObject.SetActive(false);
        }
        //Fill Images
        for (int i = 0; i < flwers_q.Count; i++)
        {
            temp_clr = FlowerEnum.GetColor(flwers_q[i]);
            imgs[i].color = temp_clr;
            imgs[i].gameObject.SetActive(true);
        }
    }
    void IdleBeh()
    {
        float step_mov = idleMovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetIdle, step_mov);
        if ((Vector2)transform.position == targetIdle) fire = false;

        //Sprite rotation
        if (targetIdle.x < transform.position.x) lookinRight = false;
        else
        {
            lookinRight = true;
        }
    }

    void FollowTrail()
    {
        try { 
            at_destination = false;

            temp_vec = lrPath.GetPosition(beeLinePos);
            Vector2 transform_position_v2 = transform.position;
            Vector2 final_pos_v2 = lrPath.GetPosition(lrPath.positionCount - 1);
            

            float step_mov = movementSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, temp_vec, step_mov);

            //Direction flag
            if ( temp_vec.x < transform.position.x)
            {
                lookinRight = false;
            }
            else if(temp_vec.x > transform.position.x)
            {
                lookinRight = true;
            }

            //TODO: Rotation
            /*
            Vector2 newRot = Vector3.RotateTowards(transform.up, temp_vec, singleStepRot, 0.0f);
            transform.LookAt(temp_vec,Vector3.up); 
            */

            //TODO: BUG that line cuts out
            
            if ((Vector2)transform.position == temp_vec)
            {
                for (int i = 0; i < beeLinePos; i++)
                {
                    lrPath.SetPosition(i, transform.position);
                }
                
                beeLinePos++;
                if (beeLinePos >= lrPath.positionCount) beeLinePos = 0;
            }

            if (transform_position_v2 == final_pos_v2)
            {
                beeLinePos = 0;
                at_destination = true;
                beeState = BeeState.idle;
                DestroyLineRenderer();
            }


        }
        catch
        {
            at_destination = true;
            beeState = BeeState.idle;
            Debug.Log(gameObject.name.ToString() + " " + gameObject.GetInstanceID().ToString() +" waiting for orders");
        }
    }

    #region BeeInput
    private void OnMouseDown()
    {
        
        DrawLine.Instance.ClearLine();
        DrawLine.Instance.CreateLine();
        lineRenderers.Enqueue(DrawLine.Instance.CurrentLine.GetComponent<LineRenderer>());
        
    }
    private void OnMouseDrag()
    {
        beeState = BeeState.stopped;
        DrawLine.Instance.Draw();
    }
    private void OnMouseUp()
    {
        DrawLine.Instance.ClearLine();
        beeLinePos = 0;
        if(lineRenderers.Count>1)
        {
            Destroy(lineRenderers.Dequeue().gameObject);
        }
        lrPath = lineRenderers.Peek();
        beeState = BeeState.following;

    }
    #endregion

    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        string go_tag = collision.gameObject.tag;
        switch (go_tag)
        {
            case "Flower":
                if (flwers_q.Count > 0)
                {
                    if (go.GetComponent<FlowerBeh>().flower_clr == flwers_q[0])
                    {
                        go.GetComponent<FlowerBeh>().DiscountPetals(1);
                        flwers_q.RemoveAt(0);
                    }
                }
                break;

            case "Panal":
                //Score points on singleton
                if (flwers_q.Count == 0)
                {
                    DestroyLineRenderer();
                    GameManager.instance.IncrementScore(scoreValue);
                    gameObject.SetActive(false);
                }
                break;
            case "Bee":
                
                // Make sure this methods are called once
                if (gameObject.GetInstanceID() > go.GetInstanceID())
                {
                    DamageFeedback?.PlayFeedbacks();
                    GameManager.instance.DecrementScore(scoreValue);
                    GameManager.instance.DecrementLives(1);
                }
                break;

            default:
                break;
        }
       

    }

    private void DestroyLineRenderer()
    {
        lineRenderers.Clear();
        Destroy(lrPath.gameObject);
    }
    #endregion

}
