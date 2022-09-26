using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPopup : BasePopup
{
    public Button buttonPlay;
    public Button buttonRePlay;
    public Button buttonPre;
    public Button buttonNext;
    public VideoPlayer player;
    protected override void Awake()
    {
        base.Awake();
        buttonPlay.onClick.AddListener(player.Play);
        buttonRePlay.onClick.AddListener(() =>
        {
            player.Stop();
            player.Play();
        });
        buttonNext.onClick.AddListener(() =>
        {
            player.time += 10f;
        });
        buttonPre.onClick.AddListener(() =>
        {
            player.time -= 10f;
        });
    }
    public void Init(VideoClip clip)
    {
        player.clip = clip;
        player.Play();
    }
}
