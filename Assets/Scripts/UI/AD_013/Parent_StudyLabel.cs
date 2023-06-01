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
        textAlhpabet.text = string.Format("���ĺ� A���� Z������ {0}���� �н��߽��ϴ�",GameManager.Instance.currentAlphabet);
        var currentLevelContentsCount = Enum.GetNames(typeof(eContents))
            .Select(x => (eContents)Enum.Parse(typeof(eContents), x))
            .Where(x => (int)x >= child.level * 100)
            .Where(x => (int)x < (child.level + 1) * 100)
            .Count();
        var current = (int)GameManager.Instance.currentContents % 100;

        var progress = (float)current / (float)currentLevelContentsCount * 100f;
        progress = Mathf.RoundToInt(progress);

        //�Ϸ翡 ������ �ϳ��� �����Ѵٴ� ����
        var nextLevelLeft = Mathf.RoundToInt((currentLevelContentsCount - current) / 7f);
        textLevelProgress.text = string.Format("���� {0}�� {1}% ���� ����������, ���� {2}���� {3}������ �ҿ�˴ϴ�",
            child.level,
            progress,
            child.level + 1,
            nextLevelLeft);
        slider.value = child.level;

        textNextLevel.text = string.Format("���� {0}���� ������ {1}", child.level + 1, GetNextLevelMessage(child.level + 1));

    }
    private string GetNextLevelMessage(int level)
    {
        switch (level)
        {
            case 1:
                return "1�ܰ� ������ ���� �޼���";
            case 2:
                return "2�ܰ� ������ ���� �޼���";
            case 3:
                return "3�ܰ� ������ ���� �޼���";
            case 4:
                return "4�ܰ� ������ ���� �޼���";
            case 5:
                return "5�ܰ� ������ ���� �޼���";
            case 6:
                return "6�ܰ� ������ ���� �޼���";
            default:
                return string.Empty;
        }
    }
}
