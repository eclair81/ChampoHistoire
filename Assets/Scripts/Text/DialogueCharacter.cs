using UnityEngine;
using UnityEngine.UI;

public class DialogueCharacter : MonoBehaviour
{
    [SerializeField] private CurrentlyTalking iAm;

    [SerializeField] private Sprite happyImage;
    [SerializeField] private Sprite sadImage;
    [SerializeField] private Sprite angryImage;
    [SerializeField] private Sprite fearImage;

    [SerializeField] private Color talkingColor;
    [SerializeField] private Color notTalkingColor;

    private Image currentImage;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    public void UpdateCharacter(CurrentlyTalking who, Expression how)
    {
        if(iAm == who)
        {
            currentImage.color = talkingColor;
            switch (how)
            {
                case Expression.Happy:
                    currentImage.sprite = happyImage;
                    //Debug.Log(name + " is now happy");
                    break;
                case Expression.Sad:
                    currentImage.sprite = sadImage;
                    //Debug.Log(name + " is now sad");
                    break;
                case Expression.Angry:
                    currentImage.sprite = angryImage;
                    //Debug.Log(name + " is now angry");
                    break;
                case Expression.Fear:
                    currentImage.sprite = fearImage;
                    //Debug.Log(name + " is now fearful");
                    break;
            }
        }
        else
        {
            currentImage.color = notTalkingColor;
        }
    }
}
