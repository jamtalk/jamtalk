using GJGameLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public string urlSong;
    public string urlAnimation;

    public ChildSetting childSettingOrizin;
    private ChildSetting childSetting;

    //�׽�Ʈ��
    public Dropdown drop;
    protected override void Awake()
    {
        drop.options = GameManager.Instance.alphabets.Select(x => new Dropdown.OptionData(x.ToString())).ToList();
        drop.onValueChanged.AddListener(value => GameManager.Instance.currentAlphabet = (eAlphabet)value);
        buttonActivity.onClick.AddListener(() =>
        {
            loading.gameObject.SetActive(true);
            GJSceneLoader.Instance.LoadScene(eSceneName.JT_PL1_102, true);
        });
        buttonSong.onClick.AddListener(() =>
        {
            PopupManager.Instance.Popup<VideoPopup>(videoPopup).Play(GameManager.Instance.GetResources().AudioData.songURL);
        });
        buttonVideo.onClick.AddListener(() =>
        {
            PopupManager.Instance.Popup<VideoPopup>(videoPopup).Play(GameManager.Instance.GetResources().AudioData.songURL);
        });
        base.Awake();
    }
    public override void Init()
    {
        base.Init();
        int level = 0;
        imageLevel.sprite = levelIcons[level];
        textDay.text = string.Format("Day {0}", UserDataManager.Instance.DashBoard.day.ToString());

        if (UserDataManager.Instance.children == null || UserDataManager.Instance.children.Length == 0)
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
