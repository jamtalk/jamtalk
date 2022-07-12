using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL5_105 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_105;
    protected override bool CheckOver() => !toggles.Select(x => x.toggle.isOn).Contains(true);
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private DigraphsWordsData[] current;
    private List<ToggleText505> toggles = new List<ToggleText505>();
    private int digraphsIndex = 0;

    public Thrower505 thrower;
    public GameObject textElement;
    public RectTransform textLayout;
    public GameObject exampleElement;
    public RectTransform exampleLayout;

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
        var questions = tempList;

        for (int i = 0; i < questions.Count; i++)
        {
            var questionsElement = Instantiate(exampleElement, exampleLayout).GetComponent<DoubleClick505>();
            questionsElement.Init(questions[i]);
            AddListener(questionsElement, i);
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
    }

    private void AddListener(DoubleClick505 button, int number)
    {
        button.onClick.RemoveAllListeners();
        button.onClickFirst.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            Debug.Log(button.text.text);
            // phanics sound 출력 
        });

        button.onClick.AddListener(() =>
        {
            if (number >= digraphsIndex)
                number += 1;
            ThrowElement(button, number);
        });
    }

    protected virtual void ThrowElement(DoubleClick505 item, int number)
    {
        thrower.Throw(item, textLayout, () =>
        {
            toggles[number].toggle.isOn = false;
            item.gameObject.SetActive(false);
            string.Join(",", toggles.Select(x => x.toggle.isOn));

            index += 1;;
            if (index == toggles.Count - 1)
            {
                if (CheckOver())
                    ShowResult();
            }
        });
    }
}
