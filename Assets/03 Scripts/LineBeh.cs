using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBeh : MonoBehaviour
{
    [SerializeField]
    private GameObject lineCursor;
    private LineRenderer lr;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineCursor.transform.position = lr.GetPosition(lr.positionCount - 1);
    }
}
