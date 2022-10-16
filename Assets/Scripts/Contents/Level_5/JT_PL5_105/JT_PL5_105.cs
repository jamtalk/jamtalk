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
    public GameObject exampleElement;
    public RectTransform exampleLayout;
    public RectTransform layoutElement;

    public List<DoubleClick505> elements = new List<DoubleClick505>();
    private List<RectTransform> layoutList = new List<RectTransform>();

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
        var temp = current[index].key.Replace(digraphs, string.Empty).ToArray();
        foreach (var item in temp)
            Debug.Log(item);
        List<string> questions = new List<string>();
        Debug.Log(digraphsIndex);
        foreach (var item in temp)
            questions.Add(item.ToString());

        var inCorrct = GameManager.Instance.alphabets
            .Where(x => !questions.Contains(x.ToString().ToLower()))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToString()
            .Take(iccorectIndex)
            .ToArray();
        Debug.Log(inCorrct.Length);

        for (int i = 0; i < iccorectIndex + questions.Count; i++)
        {
            var layouts = Instantiate(layoutElement, exampleLayout).GetComponent<RectTransform>();
            layoutList.Add(layouts);
        }

        for(int i = 0; i < questions.Count; i++)
        {
            var questionsElement = Instantiate(exampleElement, layoutList[i]).GetComponent<DoubleClick505>();
            questionsElement.Init(questions[i], i, true);
            AddListener(questionsElement);
            elements.Add(questionsElement);
        }
        for (int i = 0; i < inCorrct.Length; i++)
        {
            var questionsElement = Instantiate(exampleElement, layoutList[i + questions.Count]).GetComponent<DoubleClick505>();
            questionsElement.Init(inCorrct[i].ToString(), i, false);
            AddListener(questionsElement);
            elements.Add(questionsElement);
        }

        questions.Insert(digraphsIndex, digraphs);
        for (int i = 0; i < questions.Count; i++)
        {
            var toggleElement = Instantiate(textElement, textLayout).GetComponent<ToggleText505>();
            var isOn = true;
            if (i == digraphsIndex)
                isOn = false;
            toggleElement.Init(questions[i], isOn);

            toggles.Add(toggleElement);
        }

        audioPlayer.Play(current[index].clip);
    }

    private void AddListener(DoubleClick505 button)
    {
        button.onClickFirst.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            if (!button.isCheck)
                return;

            if (button.number >= digraphsIndex)
                button.number += 1;
            ThrowElement(button);
        });
    }

    protected virtual void ThrowElement(DoubleClick505 item)
    {
        eventSystem.enabled = false;
        item.gameObject.SetActive(false);
        var target = toggles
            .Where(x => x.toggle.isOn)
            .Where(x => x.value == item.value).First().GetComponent<RectTransform>();
        thrower.Throw(item, target, () =>
        {
            toggles[item.number].toggle.isOn = false;
            item.gameObject.SetActive(false);
            string.Join(",", toggles.Select(x => x.toggle.isOn));
            eventSystem.enabled = true;

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
                        {
                            StartCoroutine(ResetElement());
                        }
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
