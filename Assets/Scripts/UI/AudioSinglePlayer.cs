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
        if(overRoutine != null)
        {
            StopCoroutine(overRoutine);
            overRoutine = null;
        }
    }
    public void Play(AudioClip clip = null)
    {
        if (player.isPlaying)
            Stop();
        if (clip != null)
            player.clip = clip;
        player.Play();
    }
    public void Play(AudioClip clip, Action onOver)
    {
        Play(clip);
        StartCoroutine(OnOverRoutine(onOver));
    }
    public void Play(float duration, AudioClip clip = null)
    {
        Play(clip);
        stopRoutine = StartCoroutine(StopRoutine(duration));
    }
    IEnumerator StopRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Stop();
    }
    IEnumerator OnOverRoutine(Action onOver)
    {
        yield return new WaitForSeconds(player.clip.length);
        onOver?.Invoke();
    }
} 
