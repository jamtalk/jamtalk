using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSinglePlayer : MonoBehaviour
{
    private AudioSource player => GetComponent<AudioSource>();
    private Coroutine stopRoutine = null;
    public void Stop()
    {
        player.Stop();
        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
            stopRoutine = null;
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
    public void Play(float duration, AudioClip clip = null)
    {
        Play(clip);
    }
    IEnumerator StopRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Stop();
    }
} 
