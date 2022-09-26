using UnityEngine;
using UnityEngine.UI;

public class CaptionVideoPopup : BasePopup
{
    public AudioSinglePlayer player;
    public Image screen;
    public Text caption;
    public Button buttonPlay;
    public Button buttonRePlay;
    public Button buttonNext;
    public Button buttonPrevious;
    public VideoData[] data;
    private int index = 0;
    protected override void Awake()
    {
        base.Awake();
        index = 0;
        buttonRePlay.onClick.AddListener(() => ShowPage(index));
        buttonPlay.onClick.AddListener(() => ShowPage(0));
        buttonNext.onClick.AddListener(() =>
        {
            if (index + 1 < data.Length)
                ShowPage(index + 1);
        });
        buttonPrevious.onClick.AddListener(() =>
        {
            if (index - 1 > 0)
                ShowPage(index - 1);
        });
        ShowPage(0);
    }

    public void ShowPage(int index)
    {
        this.index = index;
        Show(data[index]);
    }

    private void Show(VideoData data)
    {
        player.Play(data.clip);
        screen.sprite = data.sprite;
        caption.text = data.value;
    }


    [System.Serializable]
    public class VideoData
    {
        public Sprite sprite;
        public AudioClip clip;
        public string value;
    }
}