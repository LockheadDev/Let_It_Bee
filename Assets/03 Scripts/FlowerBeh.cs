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

    public bool isColliding = false;

    private SpriteRenderer spr_rend;

    public FlowerColor flower_clr;

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

    public  void DiscountPetals(int count)
    {
        petals -= count;
        //OBJECT Pooling!!
        if (petals <= 0) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string temp_tag = collision.gameObject.tag;
        if (temp_tag == "Flower"|| temp_tag == "Panal") gameObject.SetActive(false);
    }


}
