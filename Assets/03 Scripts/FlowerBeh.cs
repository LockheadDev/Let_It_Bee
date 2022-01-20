using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBeh : MonoBehaviour
{
    [SerializeField]
    private int petals;

    public FlowerColor flower_clr;
    private SpriteRenderer spr_rend;

    void Start()
    {
        spr_rend = GetComponent<SpriteRenderer>();
        //PETALS
        int temp_rand = Random.Range(1, 6);
        petals = temp_rand;

        //COLOR
        temp_rand = Random.Range(1, 4);
        switch (temp_rand)
        {
            case 1:
                flower_clr = FlowerColor.red;
                spr_rend.color = Color.red;
                break;
            case 2:
                flower_clr = FlowerColor.green;
                spr_rend.color = Color.green;
                break;
            case 3:
                flower_clr = FlowerColor.blue;
                spr_rend.color = Color.blue;
                break;
            default:
                break;
        }
         
    }
    private void Update()
    {
        UpdateColor();
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
        if (petals <= 0) Destroy(gameObject);
    }

}
