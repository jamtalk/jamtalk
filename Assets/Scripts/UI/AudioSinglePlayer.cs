using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSinglePlayer : MonoBehaviour
{
    private AudioSource player => GetComponent<AudioSource>();
    private Coroutine stopRoutine = null;
    private Coroutine overRoutine = null;
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
        if (clip != null)
            player.clip = clip;
        Invoke("Play", delay);
    }
    public void Play(string clip, float delay)
    {
        AudioClipManager.Instance.GetClip(clip, value =>
        {
            if (value == null)
                AndroidPluginManager.Instance.PlayTTS(clip);
            else
                Play(value, delay);
        });
    }

    public void Play(AudioClip clip = null)
    {
        if (player.isPlaying)
            Stop();
        if (clip != null)
            player.clip = clip;
        player.Play();
    }
    public void Play(string clip)
    {
        AudioClipManager.Instance.GetClip(clip, value =>
        {
            if (value == null)
                AndroidPluginManager.Instance.PlayTTS(clip);
            else
                Play(value);
        });
    }

    public void Play(AudioClip clip, Action onOver)
    {
        Play(clip);
        StartCoroutine(OnOverRoutine(onOver));
    }
    public void Play(string clip, Action onOver)
    {
        AudioClipManager.Instance.GetClip(clip, value =>
        {
            if (value == null)
                AndroidPluginManager.Instance.PlayTTS(clip);
            else
                Play(value, onOver);
        });
    }


    public void Play(float duration, AudioClip clip = null)
    {
        Play(clip);
        stopRoutine = StartCoroutine(StopRoutine(duration));
    }
    public void Play(float duration, string clip)
    {
        AudioClipManager.Instance.GetClip(clip, value =>
        {
            if (value == null)
                AndroidPluginManager.Instance.PlayTTS(clip);
            else
                Play(duration, value);
        });
    }

    public void Play(float duration, float delay, AudioClip clip = null)
    {
        Play(clip, delay);
        stopRoutine = StartCoroutine(StopRoutine(duration));
    }
    public void Play(float duration, float delay, string clip = null)
    {
        AudioClipManager.Instance.GetClip(clip, value =>
        {
            if (value == null)
                AndroidPluginManager.Instance.PlayTTS(clip);
            else
                Play(delay, value);
        });
    }

    public void Play(float duration, AudioClip clip, Action onOver)
    {
        Play(clip);
        stopRoutine = StartCoroutine(OnOverRoutine(duration, onOver));
    }
    public void Play(float duration, string clip, Action onOver)
    {
        AudioClipManager.Instance.GetClip(clip, value =>
        {
            if (value == null)
                AndroidPluginManager.Instance.PlayTTS(clip);
            else
                Play(duration ,value, onOver);
        });
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
} 
