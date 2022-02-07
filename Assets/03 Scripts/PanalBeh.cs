using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PanalBeh : MonoBehaviour
{
    [SerializeField]
    private MMFeedbacks beeFeedback;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<BeeBeh>() == null) return;
        BeeBeh temp_bee = collision.gameObject.GetComponent<BeeBeh>();
        if (temp_bee.flwers_q.Count == 0) beeFeedback.PlayFeedbacks();

    }
}
