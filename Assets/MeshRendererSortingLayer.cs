using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererSortingLayer : MonoBehaviour
{
    [SerializeField]
    private string sortingLayer;
    private void OnEnable()
    {
        GetComponent<MeshRenderer>().sortingLayerName=sortingLayer;
    }
}
