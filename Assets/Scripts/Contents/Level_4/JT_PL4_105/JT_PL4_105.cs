using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class JT_PL4_105 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_105;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private DigraphsWordsData current;
    private string digraphsValue;
    private Vector3 defaultPosition;

    public Button currentButton;
    public RectTransform wordLayout;
    public GameObject wordElement;
    public GameObject digraphsElement;
    public GameObject charactor;
    public GameObject balloon;
    public RectTransform firstRect;
    public RectTransform lastRect;
    public EventSystem eventSystem;
    protected override void Awake()
    {
        base.Awake();
        currentButton.onClick.AddListener(() => audioPlayer.Play(current.clip));
        defaultPosition = currentButton.transform.position;
        MakeQuestion();
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        Clear();
        eventSystem.enabled = true;
        wordLayout.gameObject.SetActive(true);
        balloon.gameObject.SetActive(true);
        currentButton.image.fillAmount = 1f;

        currentButton.image.sprite = current.sprite;
        currentButton.image.preserveAspect = true;

        var digraphs = current.Digraphs.ToString().ToLower();
        var pairDigraphs = current.PairDigrpahs.ToString().ToLower();

        if (!current.key.Contains(current.Digraphs.ToString().ToLower()))
            digraphsValue = pairDigraphs;
        else
            digraphsValue = digraphs;

        var digraphsIndex = current.key.IndexOf(digraphsValue);
        var currentTemp = current.key.Replace(digraphsValue, string.Empty);
        var tempList = new List<string>();

        foreach (var item in currentTemp)
            tempList.Add(item.ToString());
        tempList.Insert(digraphsIndex, digraphsValue);

        for(int i = 0; i < tempList.Count; i++)
        {
            if (digraphsValue == tempList[i])
            {
                var textElemet = Instantiate(digraphsElement, wordLayout).GetComponent<wordElement405>();
                textElemet.Init(digraphs, pairDigraphs);
                DigraphsButtonAddListener(textElemet.digraphsButton, textElemet.text);
                DigraphsButtonAddListener(textElemet.pairButton, textElemet.pairText);
            }
            else
            {
                var textElemet = Instantiate(wordElement, wordLayout).GetComponent<wordElement405>();
                textElemet.Init(tempList[i]);
            }
        }

        audioPlayer.Play(current.clip);
    }

    private void DigraphsButtonAddListener(Button button, Text text)
    {
        button.onClick.AddListener(() =>
        {
            if(digraphsValue.Contains(text.text))
            {
                index += 1;
                audioPlayer.Play(current.clip);
                
                DoMove(() =>
                {
                    if (CheckOver())
                        ShowResult();
                    else
                        MakeQuestion();
                });
            }
        });
    }

    private void DoMove(TweenCallback callback)
    {
        eventSystem.enabled = false;
        wordLayout.gameObject.SetActive(false);
        balloon.gameObject.SetActive(false);

        Sequence seq = DOTween.Sequence();

        var duration = 1f;
        Tween startTween = currentButton.transform.DOMove(firstRect.position, duration);
        Tween endTween = currentButton.transform.DOMove(lastRect.position, duration);
        Tween fillTween = currentButton.image.DOFillAmount(0, 1f);

        startTween.SetEase(Ease.Linear);
        fillTween.SetEase(Ease.Linear);

        seq.Append(startTween);
        seq.Append(endTween);
        seq.Insert(duration, fillTween);

        seq.onComplete += callback;
        seq.Play();
    }

    private void Clear()
    {
        currentButton.transform.position = defaultPosition;

        var targets = new List<GameObject>();
        for (int i = 0; i < wordLayout.childCount; i++)
            targets.Add(wordLayout.GetChild(i).gameObject);
        for (int i = 0; i < targets.Count; i++)
            Destroy(targets[i]);
        targets.Clear();
    }
    protected override void ShowResult()
    {
        base.ShowResult();
        eventSystem.enabled = true;
    }
}
