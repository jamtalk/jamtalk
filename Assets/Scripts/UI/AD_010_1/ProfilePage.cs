using System.Collections;
using System.Collections.Generic;
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
    public override void Init()
    {
        base.Init();

        GetProfileDetail();
    }

    private void GetProfileDetail()
    {
        var awardParam = new Award_logParam(UserDataManager.Instance.CurrentUser.user_id);
        RequestManager.Instance.Request(awardParam, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            if(result.code != eErrorCode.Success)
            {

            }
            else
            {
                // 수집한 업적 목록에 업적 ( ex 아이콘 ) 배치 
            }
        });

        //var childinfoParam = new ChildInfoParam();
        RequestManager.Instance.Request(awardParam, (res) => // param 변경 예정 
        {
            var result = res.GetResult<ActRequestResult>();

            if (result.code != eErrorCode.Success)
            {

            }
            else
            {
                var user = UserDataManager.Instance.CurrentUser;

                textName.text = user.name;
                //textPoints.text = string.Format("{0}pt", user.point);
                //imageCurrentLevel.sprite = spritesLevel[user.level+1];
                //imageNextLevel.sprite = spritesLevel[user.level+2];
                //textAge.text = string.Format("{0} 세", user.Age);
            }
        });
    }
}
