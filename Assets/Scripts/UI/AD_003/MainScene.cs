using GJGameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainScene : UserInfoScene
{
    public Button buttonProfile;
    public Button buttonVideo;
    public Button buttonSong;
    public Button buttonActivity;
    public Button buttonParent;
    public Button buttonViewAll;
    public Image imageLevel;
    public Sprite[] levelIcons;
    public Text textDay;
    public GameObject loading;
    public GameObject videoPopup;
    public VideoClip clipSong;
    public VideoClip clipVideo;

    public ChildSetting childSettingOrizin;
    private ChildSetting childSetting;

    protected override void Awake()
    {
        buttonActivity.onClick.AddListener(() =>
        {
            loading.gameObject.SetActive(true);
            GJSceneLoader.Instance.LoadScene(eSceneName.JT_PL1_102, true);
        });
        buttonSong.onClick.AddListener(() =>
        {
            PopupManager.Instance.Popup<VideoPopup>(videoPopup).Init(clipSong);
        });
        buttonVideo.onClick.AddListener(() =>
        {
            PopupManager.Instance.Popup<VideoPopup>(videoPopup).Init(clipVideo);
        });
        base.Awake();
    }
    public override void Init()
    {
        base.Init();
        int level = 0;
        imageLevel.sprite = levelIcons[level];
        textDay.text = string.Format("Day {0}", UserDataManager.Instance.DashBoard.day.ToString());

        if (UserDataManager.Instance.childList == null)
            SetChild();
    }

    private void SetChild()
    {
        if (childSetting == null)
            childSetting = Instantiate(childSettingOrizin, transform);
        else
            childSetting.gameObject.SetActive(true);

        childSetting.Init(true);
    }
}
