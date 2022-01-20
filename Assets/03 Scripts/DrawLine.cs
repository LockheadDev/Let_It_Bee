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

    private bool fire = false;


    private bool isTouchingBee = false;
    private GameObject currentBee;
    public bool IsTouchingBee { get => isTouchingBee; /*set => isTouchingBee = value;*/ }


    void Update()
    {
        UpdateCursor();
        GetInput();
    }

    private void CreateLine()
    {
        
        currentLine = Instantiate(PR_line,Vector3.zero, Quaternion.identity);
        lr = currentLine.GetComponent<LineRenderer>();

        //Clear previous positions to get ready to draw a new line
        mousePositions.Clear();

        //NOTE: we put the two points of our line in the same coordinate...
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lr.SetPosition(0, mousePositions[0]);
        lr.SetPosition(1, mousePositions[1]);

    }

    
    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && isTouchingBee)
        {
            CreateLine();
            fire = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ClearLine();

            // Note: we put the line as a child of the bee so we can track it better
           try
            {
                currentLine.transform.parent = currentBee.transform;
                currentBee = null;
            }
            catch
            {
                Debug.Log("Path couldn't get assigned to a bee!");
            }

            fire = false;
        }
        if (Input.GetMouseButton(0) && fire)
        {
            Draw();
        }
    }

    #region LineBehaviour
    
    private void Draw()
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

    private void ClearLine()
    {
        //Clear previous positions to get ready to draw a new line
        mousePositions.Clear();

        //NOTE: we put the two points of our line in the same coordinate...
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    #endregion

    #region Cursor Behaviour
    private void UpdateCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = mousePosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bee")
        {
            isTouchingBee = true;
            currentBee = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bee")
        {
            isTouchingBee = false;
            
        }
    }

    #endregion

    
}
