using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public TMP_Text ButtonVariantText1, ButtonVariantText2;
    public Image ava;
    public TMP_Text question;
    public TMP_Text nickname;

    public Animator buttonTurnArroundBtn1;
    public Animator buttonTurnArroundBtn2;

    public const string turnArroundAnimationName = "Around";

    private void Awake()
    {
        instance = this;
    }
    public void Init()
    {
        instance = GetComponent<UI>();
        buttonTurnArroundBtn1.StopPlayback();
    }

    public void UpdateUI(Cart newCart)
    {
        // question.text = newCart.question;
        question.text = "";
        StartCoroutine(PrintQuestionText(newCart.question, newCart.question.Length));
        ButtonVariantText1.text = newCart.answerVariant1;
        ButtonVariantText2.text = newCart.answerVariant2;
        ava.sprite = newCart.ava;
        PlayCartsAnimation();
    }

    public void PlayCartsAnimation()
    {
        buttonTurnArroundBtn1.Play(turnArroundAnimationName);
        buttonTurnArroundBtn2.Play(turnArroundAnimationName);
    }
    IEnumerator PrintQuestionText(string text, int lenght, int i = 0)
    {
        AnswerButton.isDisabled = true;
        yield return new WaitForSeconds(Game.speedText);
        question.text += text[i++];
        if (i < lenght)
        {
            StartCoroutine(PrintQuestionText(text, lenght, i));
        }
        else
        {
            Game.instance.UpdateSpeedText(false);
            AnswerButton.isDisabled = false;
        }
    }
}
