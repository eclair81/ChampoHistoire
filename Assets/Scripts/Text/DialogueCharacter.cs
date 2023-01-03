using UnityEngine;
using UnityEngine.UI;

public class DialogueCharacter : MonoBehaviour
{
    [SerializeField] private CurrentlyTalking iAm;
    [HideInInspector] private SpritesExpression thisCharacterSprites;

    [SerializeField] private Color talkingColor;
    [SerializeField] private Color notTalkingColor;

    private Vector3 posBase;
    private Vector2 scaleBase;

    [SerializeField] private AnimationCurve upDownCurve;
    [SerializeField] private AnimationCurve tantrumCurve;
    private Animation whichAnim;
    private float lastFrameCurve;
    private float animTimer = 0f;
    private bool doAnim = false;

    private Image currentImage;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
        doAnim = false;
        posBase = transform.position;
        scaleBase = transform.localScale;

    }

    private void Update()
    {
        if (doAnim)
        {
            Anim();
        }
    }

    //Call this function from the DialogueManager with each new dialogue text box 
    public void UpdateCharacter(CurrentlyTalking who, Expression how, Animation anim)
    {
        if(iAm == who)
        {
            currentImage.color = talkingColor;
            switch (how)
            {
                case Expression.Neutral:
                    currentImage.sprite = thisCharacterSprites.neutral;
                    break;
                case Expression.Happy:
                    currentImage.sprite = thisCharacterSprites.happy;
                    break;
                case Expression.Sad:
                    currentImage.sprite = thisCharacterSprites.sad;
                    break;
                case Expression.Angry:
                    currentImage.sprite = thisCharacterSprites.angry;
                    break;
                case Expression.Fear:
                    currentImage.sprite = thisCharacterSprites.fear;
                    break;
            }

            Keyframe lastFrame;
            switch (anim)
            {
                case Animation.UpDown:
                    whichAnim = Animation.UpDown;
                    lastFrame = upDownCurve[upDownCurve.length - 1];
                    lastFrameCurve = lastFrame.time;
                    doAnim = true;
                    break;
                case Animation.Tantrum:
                    whichAnim = Animation.Tantrum;
                    lastFrame = tantrumCurve[tantrumCurve.length - 1];
                    lastFrameCurve = lastFrame.time;
                    doAnim = true;
                    break;
            }
        }
        else
        {
            currentImage.color = notTalkingColor;
            StopAnim();
        }
    }

    //Only call this function to update this character's info
    public void UpdateCharacterInfo(CurrentlyTalking whoAmI, SpritesExpression spriteExpressionSet)
    {
        iAm = whoAmI;
        thisCharacterSprites = spriteExpressionSet;
    }

    public CurrentlyTalking WhoAmI()
    {
        return iAm;
    }

    private void Anim()
    {
        animTimer += Time.deltaTime;
        if (animTimer <= lastFrameCurve)
        {
            switch (whichAnim)
            {
                case Animation.UpDown:
                    transform.position = posBase + new Vector3(0, upDownCurve.Evaluate(animTimer));
                    break;
                case Animation.Tantrum:
                    transform.localScale = new Vector3(-1f + tantrumCurve.Evaluate(animTimer), 1, 1);
                    break;
            }
            return;
        }
        animTimer = 0f;
        transform.position = posBase;
        transform.localScale = scaleBase;
    }

    public void StopAnim()
    {
        doAnim = false;
        animTimer = 0f;
        transform.position = posBase;
        transform.localScale = scaleBase;
    }
}
