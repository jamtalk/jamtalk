using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GJGameLibrary.Util.Youtube
{
    public class GJYoutubePlayer : MonoBehaviour
    {
        [SerializeField] private RawImage screen;
        public YoutubePlayer.YoutubePlayer player;
        public VideoPlayer VideoPlayer => player.VideoPlayer;

        public async void Play(string url)
        {
            screen.color = Color.black;
            var loading = PopupManager.Instance.ShowLoading();
            await player.PlayVideoAsync(url);
            screen.color = Color.white;
            loading?.Invoke();
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
