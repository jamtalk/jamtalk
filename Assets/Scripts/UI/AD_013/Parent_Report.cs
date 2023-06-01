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
        textAlphabet.text = string.Format("���ĺ�{0},{1}�� �����ϴ� �ܾ���� ���� ���ϰ� ���ּ���", GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1);
        textSpeak.text = string.Format("���ĺ�{0},{1}�� ������ �����ϰ� �����ּ���", GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1);
        textMessage.text = "������ ��ȭ�� ����� �غ����� ū ������ �ɰ̴ϴ�.";
    }
}
