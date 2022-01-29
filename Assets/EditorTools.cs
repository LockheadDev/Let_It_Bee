using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTools : MonoBehaviour
{
    [SerializeField]
    private bool deletePrefs = false;
    private void Update()
    {
        if(deletePrefs==true)
        {
            PlayerPrefs.DeleteAll();
            deletePrefs = false;
        }
    }
}
