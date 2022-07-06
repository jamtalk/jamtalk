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
    private DigraphsSource current;
    private List<string> questionList = new List<string>();

    public RectTransform[] layouts;
    public GameObject starElement;

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == GameManager.Instance.currentDigrpahs)
            .First();

        ShowQuestion();
    }
    private void ShowQuestion()
    {
        var digraphs = current.type.ToString().ToLower();
        var digraphsIndex = current.value.IndexOf(digraphs);
        var temp = current.value.Replace(digraphs, string.Empty);

        var first = current.value.Substring(0, digraphsIndex);
        var last = current.value.Substring(digraphsIndex);
        questionList.Add(first);
        questionList.Add(digraphs);
        questionList.Add(last);

        for (int i = 0; i < questionList.Count ; i++)
        {
            var element = Instantiate(starElement, layouts[i]).GetComponent<Text>();
        }
    }
}
