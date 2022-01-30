using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour, IBeeMoveResponse
{
    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private float idleMovementSpeed = 1f;
    [SerializeField]
    private BeeState beeState;
    private Vector2 targetIdle;
    private bool lookinRight = true;
    private Vector2 temp_vec;
    private int beeLinePos = 0;
    private Queue<LineRenderer> lineRenderers = new Queue<LineRenderer>();
    private bool fire = false;
    private bool at_destination;
    private LineRenderer lrPath;

    public void Follow()
    {
        try
        {
            at_destination = false;

            temp_vec = lrPath.GetPosition(beeLinePos);
            Vector2 transform_position_v2 = transform.position;
            Vector2 final_pos_v2 = lrPath.GetPosition(lrPath.positionCount - 1);


            float step_mov = movementSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, temp_vec, step_mov);

            //Direction flag
            if (temp_vec.x < transform.position.x)
            {
                lookinRight = false;
            }
            else if (temp_vec.x > transform.position.x)
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
            Debug.Log(gameObject.name.ToString() + " " + gameObject.GetInstanceID().ToString() + " waiting for orders");
        }
    }

    public void Idle()
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
    private void DestroyLineRenderer()
    {
        lineRenderers.Clear();
        Destroy(lrPath.gameObject);
    }


}
