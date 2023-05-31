using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ViewerPage : UserInfoScene
{
    [Header("학습 시간")]
    public Text textRegistDate;
    public Text textToday;
    public Text textTotalDay;

    [Space(10)]
    [Header("남은 학습목록")]
    public Image leftElement;

    [Space(10)]
    [Header("칭찬목록")]
    public GameObject commendation;
    public override void Init()
    {
        base.Init();
        var date = UserDataManager.Instance.CurrentUser.RegistedDate;

        textRegistDate.text = GetDate(date);
        textToday.text = GetDate(DateTime.Now);
        textTotalDay.text = string.Format("{0}일째 학습 중!", (DateTime.Now - date).Days + 1);

        GetLearningDeatil();
    }

    private void GetLearningDeatil()
    {
        // 남은 학습 목록 생성 
        var min = UserDataManager.Instance.CurrentChild.level * 100;
        var max = (UserDataManager.Instance.CurrentChild.level+1) * 100;
        var contents = Enum.GetNames(typeof(eContents))
            .Select(x => (eContents)Enum.Parse(typeof(eContents), x))
            .Where(x => (int)x >= min)
            .Where(x => (int)x < max)
            .Take(5)
            .ToArray();
        for(int i = 0;i < contents.Length; i++)
        {
            var item = Instantiate(leftElement, leftElement.transform.parent);
            SetImage(item, contents[i]);
            item.gameObject.SetActive(true);
        }

        // 칭찬 목록 생성
    }

    private void SetImage(Image image, eContents contents)
    {
        Addressables.LoadAssetAsync<Sprite>(contents.ToString()).Completed += (op) =>
        {
            image.sprite = op.Result;
            image.preserveAspect = true;
        };
    }

    private string GetDate(DateTime time)
    {
        return string.Format("{0}-{1}-{2}",
            time.Year,
            (time.Month < 10 ? "0" : "") + time.Month,
            (time.Day < 10 ? "0" : "") + time.Day);
    }
}
