using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [SerializeField]
    private GameObject PR_line;
    [SerializeField]
    private GameObject cursor;

    [SerializeField]
    private float lineResolution = 0.5f;


    private GameObject currentLine;

    private LineRenderer lr;
    private EdgeCollider2D edgeCollider;
    public List<Vector2> mousePositions;

    private bool fire = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cursor.GetComponent<CursorBeh>().isTouchingBee)
        {
            CreateLine();
            fire = true;
        }
        else if (Input.GetMouseButtonUp(0) && !cursor.GetComponent<CursorBeh>().isTouchingBee)
        {
            //Clear previous positions to get ready to draw a new line
            mousePositions.Clear();

            //NOTE: we put the two points of our line in the same coordinate...
            mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            fire = false;
        }
        if (Input.GetMouseButton(0) && fire)
        {
            Vector2 tempMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempMousePos, mousePositions[mousePositions.Count - 1]) > lineResolution)
            {
                UpdateLine(tempMousePos);
            }
        }
    }

    private void CreateLine()
    {
        
        currentLine = Instantiate(PR_line,Vector3.zero, Quaternion.identity);
        lr = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();

        //Clear previous positions to get ready to draw a new line
        mousePositions.Clear();

        //NOTE: we put the two points of our line in the same coordinate...
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lr.SetPosition(0, mousePositions[0]);
        lr.SetPosition(1, mousePositions[1]);

        edgeCollider.points = mousePositions.ToArray();
    }

    private void UpdateLine(Vector2 newMousePos)
    {
        mousePositions.Add(newMousePos);
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, newMousePos);
        edgeCollider.points = mousePositions.ToArray();
    }
}
