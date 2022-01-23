using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSequencer : MonoBehaviour
{


    [Header("Cycle sprites")]
    [SerializeField]
    private float cycleSpeed = 1f;
    [SerializeField]
    private int cycleCount = 0;
    [SerializeField]
    private Sprite[] sprites;

    //Private vairables
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length == 0) Debug.LogError(gameObject.GetInstanceID().ToString() + " " + gameObject.name + " " + " - no sprites found");
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsInvoking())
        {
            Invoke("CycleNextSprite", cycleSpeed);
        }
    }

    private void CycleNextSprite()
    {
        spriteRenderer.sprite = sprites[cycleCount];
        cycleCount++;
        if (cycleCount == sprites.Length) cycleCount = 0;

    }
}
