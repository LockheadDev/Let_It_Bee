using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class FlowerBeh : MonoBehaviour
{
    
    [Header("Flower Settings")]
    [SerializeField]
    public int petals = 1;
    public bool isColliding = false;
    [SerializeField]
    private int scoreValue = 20;

    [Space]

    [Header("GUI Settings")]
    [SerializeField]
    private TextMeshPro petalsText;

    

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
        Color clr =  FlowerEnum.GetColor(flower_clr);
        spr_rend.color = clr;
        petalsText.color = clr;
        
    }

    public  void DiscountPetals(int count)
    {
        petals -= count;
        if (petals <= 0)
        {
            GameManager.instance.IncrementScore(scoreValue);
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string temp_tag = collision.gameObject.tag;
        if (temp_tag == "Flower"|| temp_tag == "Panal") gameObject.SetActive(false);
    }


}
