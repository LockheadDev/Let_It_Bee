using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BeeBeh : MonoBehaviour
{
    [Header("Bee Status")]
    [SerializeField]
    public BeeState beeState;
    [SerializeField]
    private bool at_destination;
    public bool onCombo = false;
    public List<FlowerColor> flwers_q;

    [Space]
    
    [Header("Bee settings")]
    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private float idleMovementSpeed = 1f;
    [SerializeField]
    public int scoreValue = 10;
    [SerializeField]
    public int flowerScoreValue = 10;
    [SerializeField]
    private DrawLine drawLine;


    //Bee behaviour
    private Vector2 targetIdle;
    private bool lookinRight = true;
    private Vector2 temp_vec;

    //Input Beh
    private bool fire = false;
    private int beeLinePos = 0;

    //Line Beh
    //-FIFO structure for always having just one path
    [HideInInspector]
    public Queue<LineRenderer> lineRenderers = new Queue<LineRenderer>();
    [HideInInspector]
    public LineRenderer lrPath;

    private IUIResponse beeUIResponse;
   
    private SpriteRenderer spr;

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
        spr = GetComponent<SpriteRenderer>();
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
        if (!onCombo) gameObject.BroadcastMessage("DisableMultiplier");
        switch (beeState)
        {
            case BeeState.stopped:
                onCombo = false;
                break;
            case BeeState.idle:
                onCombo = false;
                at_destination = true;
                IdleBeh();
                break;
            case BeeState.following:
                onCombo = true;
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

    private void LateUpdate()
    {
        //Sprite rotation
        try
        {
            if (!LookinRight)
            {
                spr.flipX = true;
            }
            else
            {
                spr.flipX = false;
            }

        }
        catch (System.Exception)
        {
            Debug.Log("No direction found for sprite rotation!");
            throw;
        }
}
    void IdleBeh()
    {
        onCombo = false;
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
            onCombo = true;
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

    private void OnMouseDown()
    {
        drawLine.ClearLine();
        drawLine.CreateLine();
        lineRenderers.Enqueue(drawLine.CurrentLine.GetComponent<LineRenderer>());

    }
    private void OnMouseDrag()
    {
        beeState = BeeState.stopped;
        drawLine.Draw();
    }
    private void OnMouseUp()
    {
        drawLine.ClearLine();
        beeLinePos = 0;
        if (lineRenderers.Count > 1)
        {
            Destroy(lineRenderers.Dequeue().gameObject);
        }
        lrPath = lineRenderers.Peek();
        beeState = BeeState.following;

    }


    public void DestroyLineRenderer()
    {
        lineRenderers.Clear();
        Destroy(lrPath.gameObject);
    }

}
