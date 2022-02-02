using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    [SerializeField]
    private GameObject PR_line;

    [SerializeField]
    private float lineResolution = 0.01f;

    private GameObject currentLine;
    private LineRenderer lr;
    public List<Vector2> mousePositions;


    private bool isTouchingBee = false;
    public bool IsTouchingBee { get => isTouchingBee; /*set => isTouchingBee = value;*/ }
    public GameObject CurrentLine { get => currentLine; set => currentLine = value; }

    #region LineBehaviour
    public void CreateLine()
    {

        currentLine = Instantiate(PR_line, Vector3.zero, Quaternion.identity);
        lr = currentLine.GetComponent<LineRenderer>();

        //Clear previous positions to get ready to draw a new line
        mousePositions.Clear();

        //NOTE: we put the two points of our line in the same coordinate...
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lr.SetPosition(0, mousePositions[0]);
        lr.SetPosition(1, mousePositions[1]);

    }
    public void Draw()
    {
        Vector2 tempMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(tempMousePos, mousePositions[mousePositions.Count - 1]) > lineResolution)
        {
            UpdateLine(tempMousePos);
        }
    }
    private void UpdateLine(Vector2 newMousePos)
    {
        mousePositions.Add(newMousePos);
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, newMousePos);
    }

    public void ClearLine()
    {
        //Clear previous positions to get ready to draw a new line
        mousePositions.Clear();

        //NOTE: we put the two points of our line in the same coordinate...
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    #endregion

    
}
