using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class FlowerBeh : MonoBehaviour
{
    //Debug Section
    [SerializeField]
    private TextMeshPro petalsText;

    [SerializeField]
    private int petals = 1;

    public FlowerColor flower_clr;

    private SpriteRenderer spr_rend;

    public int Petals { get => petals; set => petals = value; }

    void Start()
    {
        spr_rend = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        UpdatePetals();
        UpdateColor();
    }

    private void UpdatePetals()
    {
        petalsText.text = petals.ToString();
    }

    private void UpdateColor()
    {
        switch (flower_clr)
        {
            case FlowerColor.red:
                spr_rend.color = Color.red;
                break;
            case FlowerColor.green:
                spr_rend.color = Color.green;
                break;
            case FlowerColor.blue:
                spr_rend.color = Color.blue;
                break;
        }
    }

    public virtual void DiscountPetals(int count)
    {
        petals -= count;
        //OBJECT Pooling!!
        if (petals <= 0) gameObject.SetActive(false);
    }

}
