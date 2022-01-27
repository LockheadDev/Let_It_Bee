using UnityEngine.UI;
using UnityEngine;
public class HeartLiveResponse : MonoBehaviour, ILiveResponse
{
    [Header("Lives GUI settings")]
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private Sprite emptyHeart;
    [SerializeField]
    private Sprite fullHeart;
    public void SetLive(int num)
    {
        if (images.Length >= num)
        {
            foreach (Image img in images)
            {
                img.sprite = emptyHeart;
            }
            for (int i = 0; i < num; i++)
            {
                images[i].sprite= fullHeart;
            }
        }
        else { Debug.Log("Not enough heart images!"); }
    }
}