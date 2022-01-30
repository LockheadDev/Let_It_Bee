using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BeeUIResponse : MonoBehaviour, IUIResponse
{
    private List<FlowerColor> flwers_q;
    public List<Image> imgs = new List<Image>();

    public void InitGUI()
    {
        //GUI Handlers
        foreach (Image image in imgs)
        {
            image.gameObject.SetActive(false);
        }
        UpdateGUI();
    }
    public void UpdateGUI()
    {
        flwers_q = GetComponent<BeeBeh>().flwers_q;
        Color temp_clr = Color.clear;
        //Clear Images
        foreach (Image img in imgs)
        {
            img.gameObject.SetActive(false);
        }
        //Fill Images
        for (int i = 0; i < flwers_q.Count; i++)
        {
            temp_clr = FlowerEnum.GetColor(flwers_q[i]);
            imgs[i].color = temp_clr;
            imgs[i].gameObject.SetActive(true);
        }
    }
}
