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
    protected override void Awake()
    {
        buttonActivity.onClick.AddListener(() =>
        {
            Debug.Log("액티비티 클릭");
            loading.gameObject.SetActive(true);
            GJSceneLoader.Instance.LoadScene(eSceneName.JT_PL1_102);
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
        //TODO.사용자의 레벨에 맞추어 레벨 이미지 세팅 필요
        int level = 0;  //사용자의 레벨 불러와야함
        imageLevel.sprite = levelIcons[level];
        textDay.text = string.Format("Day {0}", UserDataManager.Instance.DashBoard.day.ToString());
        Debug.Log("??");
    }
}
