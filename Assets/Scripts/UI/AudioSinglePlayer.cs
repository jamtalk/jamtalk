using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSinglePlayer : MonoBehaviour
{
    private string clip;
    private AudioSource player => GetComponent<AudioSource>();
    private Coroutine stopRoutine = null;
    private Coroutine overRoutine = null;
    private Coroutine delayRoutine = null;
    private Coroutine incorrectRoutine = null;

    private void Awake()
    {
        if (player.clip != null && player.playOnAwake)
            Play(player.clip);
    }
    public void Stop()
    {
        player.Stop();
        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
            stopRoutine = null;
        }
        if (overRoutine != null)
        {
            StopCoroutine(overRoutine);
            overRoutine = null;
        }
        if (delayRoutine != null)
        {
            StopCoroutine(delayRoutine);
            delayRoutine = null;
        }
    }

    /// <summary>
    /// invoke로만 delay 호출
    /// </summary>
    private void Play()
    {
        player.Play();
        //AndroidPluginManager.Instance.PlayTTS("sta");
    }

    public void Play(AudioClip clip, float delay)
    {
        if (player.isPlaying)
            Stop();
        if(clip != player.clip)
            player.clip = clip;
        //Invoke("Play", delay);
        delayRoutine =  StartCoroutine(DelayRoutine(delay));
    }

    
    public void Play(string clip, float delay)
    {
        if (clip != this.clip || player.clip ==null)
        {
            AudioClipManager.Instance.GetClip(clip, value =>
            {
                if (value == null)
                {
                    var clips = GameManager.Instance.GetClips();
                    if (clips.ContainsKey(clip))
                        AndroidPluginManager.Instance.PlayTTS(clips[clip]);
                    else
                        AndroidPluginManager.Instance.PlayTTS(clip);
                }
                else
                {
                    this.clip = clip;
                    Play(value, delay);
                }
            });
        }
        else
            player.Play();
    }

    public void Play(AudioClip clip = null)
    {
        if (player.isPlaying)
            Stop();
        if (clip != player.clip)
            player.clip = clip;
        player.Play();
    }
    public void Play(string clip)
    {
        if (this.clip != clip || player.clip == null)
        {
            AudioClipManager.Instance.GetClip(clip, value =>
            {
                if (value == null)
                {
                    var clips = GameManager.Instance.GetClips();
                    if (clips.ContainsKey(clip))
                        AndroidPluginManager.Instance.PlayTTS(clips[clip]);
                    else
                        AndroidPluginManager.Instance.PlayTTS(clip);
                }
                else
                {
                    this.clip = clip;
                    Play(value);
                }
            });
        }
        else
            Play(player.clip);
    }

    public void Play(AudioClip clip, Action onOver)
    {
        Play(clip);
        StartCoroutine(OnOverRoutine(onOver));
    }
    public void Play(string clip, Action onOver)
    {
        if (this.clip != clip || player.clip == null)
        {
            AudioClipManager.Instance.GetClip(clip, value =>
            {
                if (value == null)
                {
                    var clips = GameManager.Instance.GetClips();
                    if (clips.ContainsKey(clip))
                        AndroidPluginManager.Instance.PlayTTS(clips[clip], onOver);
                    else
                        AndroidPluginManager.Instance.PlayTTS(clip, onOver);
                }
                else
                {
                    this.clip = clip;
                    Play(value, onOver);
                }
            });
        }
        else
            Play(player.clip, onOver);
    }


    public void Play(float duration, AudioClip clip = null)
    {
        Play(clip);
        stopRoutine = StartCoroutine(StopRoutine(duration));
    }
    public void Play(float duration, string clip)
    {
        if (this.clip != clip || player.clip == null)
        {
            AudioClipManager.Instance.GetClip(clip, value =>
            {
                if (value == null)
                {
                    var clips = GameManager.Instance.GetClips();
                    if (clips.ContainsKey(clip))
                        AndroidPluginManager.Instance.PlayTTS(clips[clip]);
                    else
                        AndroidPluginManager.Instance.PlayTTS(clip);
                }
                else
                {
                    this.clip = clip;
                    Play(duration, value);
                }
            });
        }
        else
            Play(duration, player.clip);
    }

    public void Play(float duration, float delay, AudioClip clip = null)
    {
        Play(clip, delay);
        stopRoutine = StartCoroutine(StopRoutine(duration));
    }
    public void Play(float duration, float delay, string clip = null)
    {
        if(this.clip != clip || player.clip == null)
        {
            AudioClipManager.Instance.GetClip(clip, value =>
            {
                if (value == null)
                {
                    var clips = GameManager.Instance.GetClips();
                    if (clips.ContainsKey(clip))
                        AndroidPluginManager.Instance.PlayTTS(clips[clip]);
                    else
                        AndroidPluginManager.Instance.PlayTTS(clip);
                }
                else
                {
                    this.clip = clip;
                    Play(delay, value);
                }
            });
        }
    }

    public void Play(float duration, AudioClip clip, Action onOver)
    {
        Play(clip);
        stopRoutine = StartCoroutine(OnOverRoutine(duration, onOver));
    }
    public void Play(float duration, string clip, Action onOver)
    {
        if(this.clip != clip || player.clip == null)
        {
            AudioClipManager.Instance.GetClip(clip, value =>
            {
                if (value == null)
                {
                    var clips = GameManager.Instance.GetClips();
                    if (clips.ContainsKey(clip))
                        AndroidPluginManager.Instance.PlayTTS(clips[clip]);
                    else
                        AndroidPluginManager.Instance.PlayTTS(clip);
                }
                else
                {
                    this.clip = clip;
                    Play(duration, value, onOver);
                }
            });
        }
    }

    public void PlayIncorrect()
    {
        if (incorrectRoutine != null)
        {
            StopCoroutine(incorrectRoutine);
            incorrectRoutine = null;
        }

        incorrectRoutine =  StartCoroutine(PlayIncorrectRoutine());
    }
    public void PlayIncorrect(string beforeClip)
    {
        if (incorrectRoutine != null)
        {
            StopCoroutine(incorrectRoutine);
            incorrectRoutine = null;
        }

        Play(beforeClip, () => incorrectRoutine = StartCoroutine(PlayIncorrectRoutine()));
    }
    private IEnumerator PlayIncorrectRoutine()
    {
        yield return new WaitForEndOfFrame();
        while(player.isPlaying)
            yield return null;
        var clip = ResourceSchema.GetInCorrectClip();
        Play(clip);
    }

    IEnumerator StopRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Stop();
    }
    IEnumerator OnOverRoutine(Action onOver)
    {
        var len = player.clip == null ? 0 : player.clip.length;
        yield return new WaitForSeconds(len);
        onOver?.Invoke();
    }
    IEnumerator OnOverRoutine(float duration,Action onOver)
    {
        yield return new WaitForSeconds(duration);
        Stop();
        onOver?.Invoke();
    }
    IEnumerator DelayRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Play();
    }
} 
