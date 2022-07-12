using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL5_102 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_102;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int index = 0;
    private int questionCount = 3;
    private DigraphsWordsData[] current;

    public SlotMachine502 slotMachine;
    public Button tempButton;

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();

        AddListener(tempButton);
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();
    }

    private void AddListener(Button button)
    {
        button.onClick.AddListener(() =>
        {
            slotMachine.Clear();
            slotMachine.Sloting(current, () =>
            {
                if (CheckOver())
                    ShowResult();
            });
        });
    }
}
