using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parent_Report : MonoBehaviour
{
    public Text textAlphabet;
    public Text textSpeak;
    public Text textMessage;

    private void Awake()
    {
        textAlphabet.text = string.Format("알파벳{0},{1}로 시작하는 단어들을 많이 접하게 해주세요", GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1);
        textSpeak.text = string.Format("알파벳{0},{1}를 꾸준히 발음하게 도와주세요", GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1);
        textMessage.text = "간단한 대화를 영어로 해보세요 큰 도움이 될겁니다.";
    }
}
