using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BeeCollisionManager : MonoBehaviour
{

    [Header("Feedbacks")]
    //Manages collision with other objects
    [SerializeField]
    private MMFeedbacks DamageFeedback;
    [SerializeField]
    private MMFeedbacks PointsFeedback;
    [SerializeField]
    private MMFeedbacks PanalFeedback;
    [SerializeField]
    private GameObject goBeeBeh;
    [SerializeField]
    private int multiplier = 1;

    [Header("Effects")]
    [SerializeField]
    private MMFeedbackSound pointsFeedbackSound;
    [SerializeField]
    private MMFeedbackFloatingText floatingText_low;
    [SerializeField]
    private MMFeedbackFloatingText floatingText_high;
    [SerializeField]
    private float pitchStep = 0.25f;
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
                        if (bee.GetComponent<FlowerBeh>().Petals == 1) multiplier = multiplier*2;
                        int temp_score = flowerScoreValue * multiplier ;
                        if (temp_score >= 50) EnableHigh(); else EnableLow();

                        //AudioManager.instance.PlayAudio(AudioClp.pickFlower);
                        bee.GetComponent<FlowerBeh>().DiscountPetals(1);
                        flwers_q.RemoveAt(0);
                        PointsFeedback?.PlayFeedbacks(this.transform.position,temp_score);
                        GameManager.instance.IncrementScore(temp_score);
                        ComboPlus();
                        
                    }
                }
                break;

            case "Panal":
                //Score points on singleton
                if (flwers_q.Count == 0)
                {
                    int temp_score = scoreValue * (multiplier + 1);
                    if (temp_score >= 50) EnableHigh(); else EnableLow();
                    //AudioManager.instance.PlayAudio(AudioClp.destination);
                    goBeeBeh.SendMessage("DestroyLineRenderer");
                    GameManager.instance.IncrementBees(1);
                    PointsFeedback?.PlayFeedbacks(this.transform.position, temp_score);
                    GameManager.instance.IncrementScore(temp_score);
                    PanalFeedback?.PlayFeedbacks();
                    goBeeBeh.SetActive(false);
                    DisableMultiplier();
                    
                }

                break;
            case "Bee":
                // Make sure this methods are called once
                if (gameObject.GetInstanceID() > bee.GetInstanceID())
                {
                    //AudioManager.instance.PlayAudio(AudioClp.crash);
                    if (!AudioManager.instance.GetPlayingBackgroundHard()) AudioManager.instance.PlayAudio(AudioClp.backgroundHard);
                    DamageFeedback?.PlayFeedbacks();
                    GameManager.instance.DecrementScore(scoreValue);
                    PointsFeedback?.PlayFeedbacks(this.transform.position, -scoreValue);
                    GameManager.instance.DecrementLives(1);
                    DisableMultiplier();
                }
                break;

            default:
                break;
        }


    }
    public void DisableMultiplier()
    {
        multiplier = 1;
        pointsFeedbackSound.MaxPitch = 1;
        pointsFeedbackSound.MinPitch = 1;
        EnableLow();
    }
    private void ComboPlus()
    {
        multiplier++;
        pointsFeedbackSound.MaxPitch+=pitchStep;
        pointsFeedbackSound.MinPitch+= pitchStep;
    }
    private void EnableHigh()
    {
        floatingText_low.Active = false;
        floatingText_high.Active = true;
    }
    private void EnableLow()
    {
        floatingText_low.Active = true;
        floatingText_high.Active = false;
    }
}
