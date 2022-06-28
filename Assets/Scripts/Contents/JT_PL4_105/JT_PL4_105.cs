using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_105 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_105;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private DigraphsSource current;

    public Image currentImage;
    public RectTransform wordLayout;
    public GameObject wordElement;
    public GameObject digraphsElement;

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        currentImage.sprite = current.sprite;
        currentImage.preserveAspect = true;

        var digraphs = current.type.ToString().ToLower();
        var currentTemp = current.value.Replace(digraphs, string.Empty);
        var tempList = new List<string>();
        foreach (var item in currentTemp)
            tempList.Add(item.ToString());
        tempList.Add(digraphs);

        for(int i = 0; i < tempList.Count; i++)
        {
            if (current.type.ToString() == tempList[i].ToUpper())
            {
                var textElemet = Instantiate(digraphsElement, wordLayout).GetComponent<wordElement405>();
                textElemet.Init(current.type);  // oi oy 등으로 변경되게 수정 필요 
                DigraphsButtonAddListener(textElemet.currentButton, textElemet.worngButton);
            }
            else
            {
                var textElemet = Instantiate(wordElement, wordLayout).GetComponent<wordElement405>();
                textElemet.Init(tempList[i]);
            }
        }


    }

    private void DigraphsButtonAddListener(Button currentButton, Button worngButton)
    {
        currentButton.onClick.AddListener(() =>
        {
            
        });

        worngButton.onClick.AddListener(() =>
        {

        });
    }
}
