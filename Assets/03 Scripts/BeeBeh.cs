using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int scoreValue = 10;


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

    private IUIResponse beeUIResponse;
    public bool LookinRight { get => lookinRight; set => lookinRight = value; }

    private void Awake()
    {
        beeUIResponse = GetComponent<BeeUIResponse>();
    }
    private void Start()
    {
        beeLinePos = 0;
        beeState = BeeState.idle;
        fire = false;
        at_destination = true;
   
    }
    private void OnEnable()
    {
        //Sanity restart on enable
        Start();
        beeUIResponse.UpdateGUI();
    }
    void Update()
    {
        beeUIResponse.UpdateGUI();
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
                targetIdle = GameObject.Find("BeeSpawner").GetComponent<BeeSpawner>().GetRandomPos();
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
    void IdleBeh()
    {
        float step_mov = idleMovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetIdle, step_mov);
        if ((Vector2)transform.position == targetIdle) fire = false;

        //Sprite direction
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
        if (lineRenderers.Count > 1)
        {
            Destroy(lineRenderers.Dequeue().gameObject);
        }
        lrPath = lineRenderers.Peek();
        beeState = BeeState.following;

    }
    #endregion


    public void DestroyLineRenderer()
    {
        lineRenderers.Clear();
        Destroy(lrPath.gameObject);
    }

}
