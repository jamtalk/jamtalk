using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Parent_StudyLabel : MonoBehaviour
{
    public Text textAlhpabet;
    public Text textLevelProgress;
    public Text textNextLevel;
    public Slider slider;
    private void Awake()
    {
        var child = UserDataManager.Instance.CurrentChild;
        textAlhpabet.text = string.Format("알파벳 A부터 Z까지중 {0}까지 학습했습니다",GameManager.Instance.currentAlphabet);
        var currentLevelContentsCount = Enum.GetNames(typeof(eContents))
            .Select(x => (eContents)Enum.Parse(typeof(eContents), x))
            .Where(x => (int)x >= child.level * 100)
            .Where(x => (int)x < (child.level + 1) * 100)
            .Count();
        var current = (int)GameManager.Instance.currentContents % 100;

        var progress = (float)current / (float)currentLevelContentsCount * 100f;
        progress = Mathf.RoundToInt(progress);

        //하루에 컨텐츠 하나씩 진행한다는 전제
        var nextLevelLeft = Mathf.RoundToInt((currentLevelContentsCount - current) / 7f);
        textLevelProgress.text = string.Format("레벨 {0}의 {1}% 정도 진행했으며, 레벨 {2}까지 {3}주정도 소요됩니다",
            child.level,
            progress,
            child.level + 1,
            nextLevelLeft);
        slider.value = child.level;

        textNextLevel.text = string.Format("다음 {0}레벨 에서는 {1}", child.level + 1, GetNextLevelMessage(child.level + 1));

    }
    private string GetNextLevelMessage(int level)
    {
        switch (level)
        {
            case 1:
                return "1단계 컨텐츠 설명 메세지";
            case 2:
                return "2단계 컨텐츠 설명 메세지";
            case 3:
                return "3단계 컨텐츠 설명 메세지";
            case 4:
                return "4단계 컨텐츠 설명 메세지";
            case 5:
                return "5단계 컨텐츠 설명 메세지";
            case 6:
                return "6단계 컨텐츠 설명 메세지";
            default:
                return string.Empty;
        }
    }
}
