using GJGameLibrary.Util.Youtube;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Book_Animation : MonoBehaviour
{
//#if UNITY_EDITOR
    public BookContentsSetting testSetting;
//#endif
    public Button buttonHome;
    public Button buttonPlay;
    public Button buttonRePlay;
    public Button buttonPre;
    public Button buttonNext;
    public GJYoutubePlayer youtubePlayer;
    public VideoPlayer player => youtubePlayer.VideoPlayer;

    protected void Start()
    {
//#if UNITY_EDITOR
        testSetting.Apply();
//#endif
        buttonHome.onClick.AddListener(() => GJGameLibrary.GJSceneLoader.Instance.LoadScene(eSceneName.AC_004));
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

        StartCoroutine(WaitFrame());
    }
    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();

        var url = GameManager.Instance.GetCurrentBook().GetURLData();
        if (string.IsNullOrEmpty(url.animationURL))
            Debug.LogWarning("등록되지 않은 유투브 링크 입니다");
        else
            Play(url.animationURL);
    }
    public void Play(string url)
    {
        Debug.LogFormat("@@@@@@@@@@@@@URL : {0}", url);
        if (player.isPlaying)
            player.Stop();
        StartCoroutine(Wait(() => youtubePlayer.Play(url)));
    }
    IEnumerator Wait(System.Action callback)
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.LogFormat("시작 {0}초전..", 3 - i);
            yield return new WaitForSeconds(1f);
        }
        callback?.Invoke();
    }
}
