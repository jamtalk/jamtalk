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
        var user = UserDataManager.Instance.CurrentUser;
        textName.text = user.name;
        //textAge.text = string.Format("{0} 세", user.Age);  //현재 생일 입력칸 없어서구현 불가
    }
}
