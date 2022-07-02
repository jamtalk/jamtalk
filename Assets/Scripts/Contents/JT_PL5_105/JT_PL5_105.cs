using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL5_105 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_105;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private DigraphsSource[] current;

    public Text currentText;
    public DoubleClickButton doubleClickButton;


    protected override void Awake()
    {
        base.Awake();
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        var digraphs = current[index].type.ToString().ToLower();

        var temp = current[index].value.Replace(digraphs, string.Empty);
        // var questions =  eAlphabet linq 로 가져와서 temp 추가하여 보기 만들기 

        // currentText > digraphs 만 나타나야 함 


    }


}
