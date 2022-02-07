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
    private MMFeedbacks BadPointsFeedback;
    [SerializeField]
    private MMFeedbacks PointsHighFeedback;
    [SerializeField]
    private MMFeedbacks PointsLowFeedback;
    [SerializeField]
    private MMFeedbacks PanalFeedback;
    [SerializeField]
    private GameObject goBeeBeh;
    [SerializeField]
    private int multiplier = 1;

    [Header("Pitch Effects")]
    [SerializeField]
    private List<MMFeedbackSound> pointsFeedbackSounds = new List<MMFeedbackSound>();
    [SerializeField]
    private float pitchStep = 0.25f;

    private void Start()
    {
        //We obtain elements for pitch changing
        foreach (var item in PointsLowFeedback.Feedbacks)
        {
            if (item.Label == "FX_Sound") pointsFeedbackSounds.Add((MMFeedbackSound)item);
        }
        foreach (var item in PointsHighFeedback.Feedbacks)
        {
            if (item.Label == "FX_Sound") pointsFeedbackSounds.Add((MMFeedbackSound)item);
        }
    }
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
                        //Score calculation
                        if (bee.GetComponent<FlowerBeh>().Petals == 1) multiplier = multiplier*2;
                        int temp_score = flowerScoreValue * multiplier ;

                        //Feedbacks
                        PlayOnScore(temp_score);

                        //Game variabes changes
                        GameManager.instance.IncrementScore(temp_score);

                        //Bee variable changes
                        bee.GetComponent<FlowerBeh>().DiscountPetals(1);
                        flwers_q.RemoveAt(0);

                        //Multiplier behaviour
                        ComboPlus();
                        
                    }
                }
                break;

            case "Panal":
                //Score points on singleton
                if (flwers_q.Count == 0)
                {
                    //Score calculation
                    int temp_score = scoreValue * (multiplier + 1);

                    //Feedbacks
                    PlayOnScore(temp_score);
                    PanalFeedback?.PlayFeedbacks();

                    //Game variables changes
                    GameManager.instance.IncrementBees(1);
                    GameManager.instance.IncrementScore(temp_score);

                    //Manage despawning
                    goBeeBeh.SendMessage("DestroyLineRenderer");
                    goBeeBeh.SetActive(false);
                    DisableMultiplier();

                }

                break;
            case "Bee":
                // Make sure this methods are called once
                if (gameObject.GetInstanceID() > bee.GetInstanceID())
                {
                    
                    //Feedbacks
                    PlayOnScore(-scoreValue);
                    DamageFeedback?.PlayFeedbacks();
                    MusicManger.instance.PlayDamageMusic();


                    //Game variables changes
                    GameManager.instance.DecrementLives(1);
                    GameManager.instance.DecrementScore(scoreValue);
                    DisableMultiplier();
                }
                break;

            default:
                break;
        }


    }

    private void PlayOnScore(int score) //Play different feedbacks based on score
    {
        if(score<0)
        {
            BadPointsFeedback?.PlayFeedbacks(transform.position, score);

        }
        else if (score>=0 && score <50)
        {
            PointsLowFeedback?.PlayFeedbacks(transform.position, score);
        }
        else if (score>=50)
        {
            PointsHighFeedback?.PlayFeedbacks(transform.position, score);
        }
    }
    public void DisableMultiplier()
    {
        multiplier = 1;
        Debug.Log("reset");
        foreach (MMFeedbackSound sound in pointsFeedbackSounds)
        {
            sound.MaxPitch = 1;
            sound.MinPitch = 1;
        }
    }
    private void ComboPlus()
    {
        multiplier++;
        foreach (MMFeedbackSound sound in pointsFeedbackSounds)
        {
            sound.MaxPitch += pitchStep;
            sound.MinPitch += pitchStep;
        }
    }
}
