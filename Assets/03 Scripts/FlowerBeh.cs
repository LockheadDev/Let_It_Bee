using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
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

    [Space]
    
    [Header("Feedbacks")]
    [SerializeField]
    private MMFeedbacks highlightFeedback;
    [SerializeField]
    private MMFeedbacks pickupFeedback;
    [SerializeField]
    private MMFeedbacks DispawnFeedback;



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

        if(petals ==1 ) highlightFeedback.PlayFeedbacks();
        pickupFeedback.PlayFeedbacks();

        if (petals <= 0)
        {
            GameManager.instance.IncrementScore(scoreValue);
            Dissapear();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string temp_tag = collision.gameObject.tag;
        if (temp_tag == "Panal")Dissapear();
        if (temp_tag == "Flower" && collision.gameObject.GetInstanceID() > gameObject.GetInstanceID()) Dissapear();
    }

    private void Dissapear()
    {
        DispawnFeedback.PlayFeedbacks();
        Invoke("SetInactive",DispawnFeedback.TotalDuration);
    }
    private void SetInactive()
    {
        gameObject.SetActive(false);
    }

}
