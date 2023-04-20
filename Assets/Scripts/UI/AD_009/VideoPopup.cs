using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using GJGameLibrary.Util.Youtube;
public class VideoPopup : BasePopup
{
    public Button buttonPlay;
    public Button buttonRePlay;
    public Button buttonPre;
    public Button buttonNext;
    public GJYoutubePlayer youtubePlayer;
    public VideoPlayer player => youtubePlayer.VideoPlayer;
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
            youtubePlayer.Skip(10f);
        });
        buttonPre.onClick.AddListener(() =>
        {
            youtubePlayer.Prev(10f);
        });
    }
    public void Init(VideoClip clip)
    {
        player.clip = clip;
        player.Play();
    }
    public void Play(string url)
    {
        UnityEngine.Debug.Log("플레이 시작");
        if (player.isPlaying)
            player.Stop();
        youtubePlayer.Play(url);
    }
}
