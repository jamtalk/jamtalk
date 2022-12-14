using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL5_105 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_105;
    protected override bool CheckOver() => index == questionCount && !toggles.Select(x => x.toggle.isOn).Contains(true);
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private DigraphsWordsData[] current;
    private List<ToggleText505> toggles = new List<ToggleText505>();
    private int digraphsIndex = 0;
    private int layoutCount;
    int iccorectIndex = 3;

    public EventSystem eventSystem;
    public Image effectImage;
    public Thrower505 thrower;
    public GameObject textElement;
    public RectTransform textLayout;
    public DoubleClick505 exampleElement;
    public RectTransform exampleLayout;
    public RectTransform layoutElement;

    public List<DoubleClick505> elements = new List<DoubleClick505>();
    private List<RectTransform> layoutList = new List<RectTransform>();
    List<string> questions = new List<string>();

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();


        while (!isNext) yield return null;
        isNext = false;

        for (int j = 0; j < questions.Count - 1; j++)
        {
            var target = elements.Where(x => x.isCheck && x.gameObject.activeSelf)
                .OrderBy(x => Random.Range(0, 100)).First();

            guideFinger.DoMove(target.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    ClickMotion(target);
                    guideFinger.gameObject.SetActive(false);
                });
            });

            while (!isNext) yield return null;
            isNext = false;

        }
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();
        index = 0;
        StartCoroutine(ResetElement());
    }

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        var digraphs = current[index].IncludedDigraphs;
        digraphsIndex = current[index].key.IndexOf(digraphs);
        questions = current[index].key
            .Replace(digraphs, string.Empty)
            .Select(x => x.ToString().ToLower())
            .ToList();

        var inCorrct = GameManager.Instance.alphabets
            .Where(x => !questions.Contains(x.ToString().ToLower()))
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(iccorectIndex)
            .Select(x => x.ToString().ToLower())
            .ToArray();

        // bttuon layout 생성 
        for (int i = 0; i < iccorectIndex + questions.Count; i++)
        {
            var layouts = Instantiate(layoutElement, exampleLayout).GetComponent<RectTransform>();
            layoutList.Add(layouts);
        }

        var optionList = questions.Union(inCorrct).ToArray();
        var optionsDatas = new List<OptionsData>();

        for(int i = 0; i < optionList.Length; i++)
        {
            var tm = new OptionsData();
            var isCorrects = false;
            if (i < questions.Count)
                isCorrects = true;

            tm.Init(optionList[i], isCorrects);
            optionsDatas.Add(tm);
        }

        optionsDatas = optionsDatas.OrderBy(x => Random.Range(0, 100)).ToList();

        // button 생성
        for( int i = 0; i < layoutList.Count; i++)
        {
            var questionElement = Instantiate(exampleElement, layoutList[i]);
            questionElement.Init(optionsDatas[i].value, i, optionsDatas[i].isCorrect);
            AddListener(questionElement);
            elements.Add(questionElement);
        }

        // toggles 생성 
        questions.Insert(digraphsIndex, digraphs);
        for (int i = 0; i < questions.Count; i++)
        {
            var toggleElement = Instantiate(textElement, textLayout).GetComponent<ToggleText505>();
            toggleElement.toggle.interactable = false;
            var isOn = true;
            if (i == digraphsIndex)
                isOn = false;
            toggleElement.Init(questions[i], isOn);

            toggles.Add(toggleElement);
        }

        audioPlayer.Play(current[index].clip, () => isNext = true);
    }

    private void AddListener(DoubleClick505 button)
    {
        button.onClickFirst.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            if (!button.isCheck)
                return;

            ClickMotion(button);
        });
    }

    private void ClickMotion(DoubleClick505 button)
    {
        var target = toggles
                .Where(x => x.toggle.isOn)
                .Where(x => x.value == button.value).First();

        if (button.number >= digraphsIndex)
            button.number += 1;
        ThrowElement(button, target);
    }

    protected virtual void ThrowElement(DoubleClick505 item, ToggleText505 target)
    {
        eventSystem.enabled = false;
        item.gameObject.SetActive(false);

        var targetRt = target.GetComponent<RectTransform>();
        thrower.Throw(item, targetRt, () =>
        {
            target.toggle.isOn = false;
            item.gameObject.SetActive(false);
            string.Join(",", toggles.Select(x => x.toggle.isOn));
            eventSystem.enabled = true;
            isNext = true;
            if (!toggles.Select(x => x.toggle.isOn).Contains(true))
            {
                audioPlayer.Play(current[index].clip, () =>
                {
                    index += 1;
                    DoMove(() =>
                    {
                        if (CheckOver())
                            ShowResult();
                        else
                            if (isGuide) EndGuidnce();
                            else StartCoroutine(ResetElement());
                    });
                });
            }
        });
    }

    private void DoMove(TweenCallback callback)
    {
        var temp = effectImage.GetComponentsInChildren<DoubleClick505>();
        foreach (var item in temp)
            item.gameObject.SetActive(false);

        var seq = DOTween.Sequence();

        var firstTween = effectImage.transform.DOScaleX(0f, 1f);
        var lastTween = effectImage.transform.DOScale(1f, 1f);

        seq.Append(firstTween);
        seq.Insert(1.5f, lastTween);

        seq.onComplete += callback;

        seq.Play();
    }
    private IEnumerator ResetElement()
    {
        elements.Clear();
        foreach (var item in toggles)
            Destroy(item.gameObject);

        foreach (var item in layoutList)
            Destroy(item.gameObject);

        toggles.Clear();
        layoutList.Clear();

        yield return new WaitForEndOfFrame();

        ShowQuestion();
    }
}
public class OptionsData
{
    public string value { get; private set; }
    public bool isCorrect { get; private set; }

    public void Init(string value, bool isCorrect)
    {
        this.value = value;
        this.isCorrect = isCorrect;
    }
}
