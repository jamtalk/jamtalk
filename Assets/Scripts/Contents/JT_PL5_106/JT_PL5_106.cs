using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL5_106 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_106;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private DigraphsWordsData current;
    private List<string> questionList = new List<string>();

    public RectTransform[] layouts;
    public GameObject starElement;


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
            .First();

        ShowQuestion();
    }
    private void ShowQuestion()
    {
        var digraphs = current.Digraphs.ToString().ToLower();
        var digraphsIndex = current.key.IndexOf(digraphs);
        var temp = current.key.Replace(digraphs, string.Empty);

        var first = string.Empty;
        var last = string.Empty;

        if (digraphsIndex != 0)
        {
            first = current.key.Substring(0, digraphsIndex);
            questionList.Add(first);
        }

        if(digraphsIndex != current.key.Length - digraphs.Length)
        {
            last = current.key.Substring(digraphsIndex + digraphs.Length);
            questionList.Add(last);
        }

        questionList.Add(digraphs);

        var randomLayout = layouts.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < questionList.Count ; i++)
        {
            var element = Instantiate(starElement, randomLayout[i]).GetComponent<StarElement506>();
            element.Init(questionList[i]);
        }
    }
}
