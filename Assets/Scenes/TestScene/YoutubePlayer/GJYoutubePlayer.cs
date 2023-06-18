using LightShaft.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubeLight;

namespace GJGameLibrary.Util.Youtube
{
    public class GJYoutubePlayer : MonoBehaviour
    {
        private Action loading;
        [SerializeField] private RawImage screen;
        public YoutubePlayer player;
        public YoutubeVideoEvents events;
        public VideoPlayer VideoPlayer => player.videoPlayer;

        public void Play(string url)
        {
            screen.color = Color.black;
            player.Play(url);
            loading = PopupManager.Instance.ShowLoading();
            events.OnVideoReadyToStart.AddListener(OnStartVideo);
        }
        private void OnStartVideo()
        {
            screen.color = Color.white;
            loading?.Invoke();
            events.OnVideoReadyToStart.RemoveListener(OnStartVideo);
        }
        public void Stop()
        {
            if (VideoPlayer.isPlaying)
                VideoPlayer.Stop();
        }
        public void Pause()
        {
            if (VideoPlayer.isPlaying)
                VideoPlayer.Pause();
        }
        public void Skip(float time = 5f) => MoveTimeline(time);
        public void Prev(float time = 5f) => MoveTimeline(-time);
        private void MoveTimeline(float time)
        {
            if (VideoPlayer.isPlaying)
            {
                var currentTime = VideoPlayer.time + time;
                if (currentTime > VideoPlayer.length)
                    currentTime = VideoPlayer.length;
                else if (currentTime < 0)
                    currentTime = 0;

                VideoPlayer.time = currentTime;
            }
        }
    }
}
