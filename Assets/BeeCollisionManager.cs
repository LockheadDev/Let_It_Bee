using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BeeCollisionManager : MonoBehaviour
{

    //Manages collision with other objects
    [SerializeField]
    private MMFeedbacks DamageFeedback;
    [SerializeField]
    private MMFeedbacks PointsFeedback;
    [SerializeField]
    private GameObject goBeeBeh;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        List<FlowerColor> flwers_q = goBeeBeh.GetComponent<BeeBeh>().flwers_q;
        int scoreValue = goBeeBeh.GetComponent<BeeBeh>().scoreValue;
        int flowerScoreValue = goBeeBeh.GetComponent<BeeBeh>().flowerScoreValue;

        GameObject bee = collision.gameObject;
        string go_tag = bee.tag;

        switch (go_tag)
        {
            case "Flower":
                if (flwers_q.Count > 0)
                {
                    if (bee.GetComponent<FlowerBeh>().flower_clr == flwers_q[0])
                    {
                        bee.GetComponent<FlowerBeh>().DiscountPetals(1);
                        flwers_q.RemoveAt(0);
                        AudioManager.instance.PlayAudio(AudioClp.pickFlower);
                        PointsFeedback?.PlayFeedbacks(this.transform.position,flowerScoreValue);
                        GameManager.instance.IncrementScore(flowerScoreValue);
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
                    PointsFeedback?.PlayFeedbacks(this.transform.position, scoreValue);
                    goBeeBeh.SetActive(false);
                }

                break;
            case "Bee":
                // Make sure this methods are called once
                if (gameObject.GetInstanceID() > bee.GetInstanceID())
                {
                    AudioManager.instance.PlayAudio(AudioClp.crash);
                    if (!AudioManager.instance.GetPlayingBackgroundHard()) AudioManager.instance.PlayAudio(AudioClp.backgroundHard);
                    DamageFeedback?.PlayFeedbacks();
                    GameManager.instance.DecrementScore(scoreValue);
                    PointsFeedback?.PlayFeedbacks(this.transform.position, -scoreValue);
                    GameManager.instance.DecrementLives(1);
                }
                break;

            default:
                break;
        }


    }
}
