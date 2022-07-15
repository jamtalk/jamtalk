using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public Image effectImage;
    public Thrower505 thrower;
    public GameObject textElement;
    public RectTransform textLayout;
    public GameObject exampleElement;
    public RectTransform exampleLayout;
    public RectTransform layoutElement;

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
        var digraphs = current[index].Digraphs.ToString().ToLower();
        digraphsIndex = current[index].key.IndexOf(digraphs);
        var temp = current[index].key.Replace(digraphs, string.Empty);
        var tempList = new List<string>();
        foreach (var item in temp)
            tempList.Add(item.ToString());

        int iccorectIndex = 3;
        layoutCount = questionCount + iccorectIndex;

        var iccorect = GameManager.Instance.alphabets
            .Where(x => !tempList.Contains(x.ToString().ToLower()))
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(iccorectIndex)
            .ToString()
            .ToArray();

        var questions = tempList;
        var layoutList = new List<RectTransform>();

        for( int i = 0; i < iccorectIndex + questions.Count; i++)
        {
            var layouts = Instantiate(layoutElement, exampleLayout).GetComponent<RectTransform>();
            layoutList.Add(layouts);
        }
        layoutList = layoutList.OrderBy(x => Random.Range(0f, 100f)).ToList();

        for (int i = 0; i < layoutList.Count; i++)
        {
            var questionsElement = Instantiate(exampleElement, layoutList[i]).GetComponent<DoubleClick505>();

            if (questions.Count > i)
                questionsElement.Init(questions[i],i ,true);
            else
                questionsElement.Init(iccorect[i].ToString(),i, false);

            AddListener(questionsElement);
        }

        tempList.Insert(digraphsIndex, digraphs);
        for (int i = 0; i < tempList.Count; i++)
        {
            var toggleElement = Instantiate(textElement, textLayout).GetComponent<ToggleText505>();
            var isOn = true;
            if (i == digraphsIndex)
                isOn = false;
            toggleElement.Init(tempList[i], isOn);

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
        });;
    }

    protected virtual void ThrowElement(DoubleClick505 item)
    {
        item.gameObject.SetActive(false);
        thrower.Throw(item, textLayout, () =>
        {
            toggles[item.number].toggle.isOn = false;
            item.gameObject.SetActive(false);
            string.Join(",", toggles.Select(x => x.toggle.isOn));

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
                            Reset();
                            ShowQuestion();
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
        seq.Insert(1.5f , lastTween);

        seq.onComplete += callback;

        seq.Play();
    }
    private void Reset()
    {
        var targets = new List<GameObject>();
        for (int i = 0; i < layoutCount; i++)
        {
            if( i < toggles.Count)
                targets.Add(toggles[i].gameObject);
            targets.Add(exampleLayout.GetChild(i).gameObject);
        }

        for (int i = 0; i < targets.Count; i ++)
        {
            Destroy(targets[i]);
        }
        targets.Clear();
        toggles.Clear();

    }
}
