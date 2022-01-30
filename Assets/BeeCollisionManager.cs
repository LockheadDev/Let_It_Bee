using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BeeCollisionManager : MonoBehaviour
{

    [SerializeField]
    private MMFeedbacks DamageFeedback;
    [SerializeField]
    private GameObject goBeeBeh;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        List<FlowerColor> flwers_q = goBeeBeh.GetComponent<BeeBeh>().flwers_q;
        int scoreValue = goBeeBeh.GetComponent<BeeBeh>().scoreValue;

        GameObject go = collision.gameObject;
        string go_tag = collision.gameObject.tag;
        switch (go_tag)
        {
            case "Flower":
                if (flwers_q.Count > 0)
                {
                    if (go.GetComponent<FlowerBeh>().flower_clr == flwers_q[0])
                    {
                        go.GetComponent<FlowerBeh>().DiscountPetals(1);
                        flwers_q.RemoveAt(0);
                        AudioManager.instance.PlayAudio(AudioClp.pickFlower);
                    }
                }
                break;

            case "Panal":
                //Score points on singleton
                if (flwers_q.Count == 0)
                {
                    goBeeBeh.SendMessage("DestroyLineRenderer");
                    AudioManager.instance.PlayAudio(AudioClp.destination);
                    GameManager.instance.IncrementScore(scoreValue);
                    GameManager.instance.IncrementBees(1);
                    goBeeBeh.SetActive(false);
                }

                break;
            case "Bee":
                // Make sure this methods are called once
                if (gameObject.GetInstanceID() > go.GetInstanceID())
                {
                    AudioManager.instance.PlayAudio(AudioClp.crash);
                    if (!AudioManager.instance.GetPlayingBackgroundHard()) AudioManager.instance.PlayAudio(AudioClp.backgroundHard);
                    DamageFeedback?.PlayFeedbacks();
                    GameManager.instance.DecrementScore(scoreValue);
                    GameManager.instance.DecrementLives(1);
                }
                break;

            default:
                break;
        }


    }
}
