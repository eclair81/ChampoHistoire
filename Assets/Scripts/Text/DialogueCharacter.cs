using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCharacter : MonoBehaviour
{
    [SerializeField] private CurrentlyTalking iAm;
    [HideInInspector] private SpritesExpression thisCharacterSprites;

    [SerializeField] private Color talkingColor;
    [SerializeField] private Color notTalkingColor;

    private Vector3 posBase;
    private Vector3 posOutStage;
    private Vector2 scaleBase;

    [HideInInspector] public bool ok;

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
        posOutStage = new Vector3(1200, transform.position.y, 0);
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
                case Animation.NoAnim:
                    currentImage.color = notTalkingColor; //this case is used for the staging -> if no animation, character stays grey 
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
        currentImage.sprite = thisCharacterSprites.neutral;
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

    public IEnumerator EnterStage()
    {
        currentImage.sprite = thisCharacterSprites.neutral;
        ok = false;
        transform.position = posOutStage;
        int i = 1;
        while (transform.position.x > posBase.x )
        {
            transform.position = new Vector3(posOutStage.x - (i*5), posBase.y, 0);
            i++;
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = posBase;
        ok = true;
    }

    public IEnumerator LeaveStage()
    {
        ok = false;
        int i = 1;
        while (transform.position.x < posOutStage.x)
        {
            transform.position = new Vector3(posBase.x + (i * 5), posBase.y, 0);
            i++;
            yield return new WaitForSeconds(0.01f);
        }
        ok = true;
    }
}
