using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewerPage : UserInfoScene
{
    public Text textRegistDate;
    public Text textToday;
    public Text textTotalDay;
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

        // 칭찬 목록 생성
    }

    private string GetDate(DateTime time)
    {
        return string.Format("{0}-{1}-{2}",
            time.Year,
            (time.Month < 10 ? "0" : "") + time.Month,
            (time.Day < 10 ? "0" : "") + time.Day);
    }
}
