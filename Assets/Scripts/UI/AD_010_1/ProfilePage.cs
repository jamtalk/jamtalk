using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePage : UserInfoScene
{
    public Text textName;
    public Text textAge;
    public Text textCurrentLevel;
    public Text textNextLevel;
    public Text textPoints;
    public Sprite[] spritesLevel;
    public Image imageCurrentLevel;
    public Image imageNextLevel;
    public LevelMessageData[] messages;
    public override void Init()
    {
        base.Init();

        GetProfileDetail();
    }

    private void GetProfileDetail()
    {
        var awardParam = new Award_logParam();
        RequestManager.Instance.Request(awardParam, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            if(result.code != eErrorCode.Success)
            {
                Debug.LogError("오류");
            }
            else
            {
                // 수집한 업적 목록에 업적 ( ex 아이콘 ) 배치 
            }
        });

        RequestManager.Instance.Request(awardParam, (res) => // param 변경 예정 
        {
            var result = res.GetResult<ActRequestResult>();

            if (result.code != eErrorCode.Success)
            {
                Debug.LogError("오류");
            }
            else
            {
                var child = UserDataManager.Instance.CurrentChild;
                textName.text = child.name;
                textPoints.text = string.Format("{0}pt", child.point);
                imageCurrentLevel.sprite = spritesLevel[child.level + 1];
                imageNextLevel.sprite = spritesLevel[child.level + 2];
                textAge.text = string.Format("{0} 세", child.age);
                var currentLevelMessage = messages.ToList().Find(x => x.level == child.level);
                textCurrentLevel.text = messages.ToList().Find(x => x.level == child.level).currentMessage;
                textNextLevel.text = messages.ToList().Find(x => x.level == child.level + 1).nextMessage;
            }
        });
    }
}
[System.Serializable]
public class LevelMessageData
{
    public int level;
    public string currentMessage;
    public string nextMessage;
}
