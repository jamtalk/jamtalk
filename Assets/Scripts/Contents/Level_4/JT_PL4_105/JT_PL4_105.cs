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

    public Animator ani;

    private wordElement405 correctButtons;



    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isNext) yield return null;
        isNext = false;
        var target = correctButtons.values.Where(x => x.text == current.IncludedDigraphs).First();

        guideFinger.DoMove(target.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                guideFinger.gameObject.SetActive(false);
                DigraphsButtonAddListener(target);
            });
        });
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();
        index = 0;
    }


    protected override void Awake()
    {
        ani.SetBool("Eat", true);
        base.Awake();
        currentButton.onClick.AddListener(() =>
        {
            ani.SetBool("Eat", true);
            audioPlayer.Play(current.clip);
        });
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
                textElemet.digraphsButton.onClick.AddListener(() => DigraphsButtonAddListener(textElemet.text));
                textElemet.pairButton.onClick.AddListener(() => DigraphsButtonAddListener(textElemet.pairText));
                correctButtons = textElemet;
                Debug.Log("if");
            }
            else
            {
                var textElemet = Instantiate(wordElement, wordLayout).GetComponent<wordElement405>();
                textElemet.Init(tempList[i]);
            }
        }

        audioPlayer.Play(current.clip, () => isNext = true);
    }

    private void DigraphsButtonAddListener(Text text)
    {
        if (digraphsValue.Contains(text.text))
        {
            index += 1;
            audioPlayer.Play(current.clip);
            StartCoroutine(Eat());
            DoMove(() =>
            {
                if (CheckOver())
                    ShowResult();
                else
                {
                    if (isGuide) EndGuidnce();
                    MakeQuestion();
                }
            });
        }
    }

    private IEnumerator Eat()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        ani.SetBool("Eat", true);
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
