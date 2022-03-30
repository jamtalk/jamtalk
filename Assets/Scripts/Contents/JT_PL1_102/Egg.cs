using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;
using DG.Tweening;

public class Egg : MonoBehaviour
{
    public Image imageEgg;
    public Sprite spriteNormal;
    public Sprite spriteCrack;
    public SerializableDictionaryBase<RectTransform,RectTransform> pices;

    public AudioSource audioPlayer;
    public AudioClip shakeClip;
    public AudioClip crackClip;
    public event Action onBroken;
    public bool isCrakced => imageEgg.sprite == spriteCrack;
    public void SetCrack()
    {
        PlayAudio(crackClip);
        imageEgg.sprite = spriteCrack;
        imageEgg.rectTransform.DOShakePosition(1f,30);
    }
    public void Break()
    {
        imageEgg.gameObject.SetActive(false);
        var seq = DOTween.Sequence();
        foreach(var pice in pices)
        {
            pice.Key.gameObject.SetActive(true);
            var tween = pice.Key.DOMove(pice.Value.position, 1f);
            seq.Insert(0,tween);
        }
        seq.onComplete += () => onBroken?.Invoke();
        seq.Play();
    }
    public void Shake()
    {
        PlayAudio(shakeClip);
        var time = .1f;
        var rt = imageEgg.rectTransform;
        var shakeSeq = DOTween.Sequence();
        
        var leftTween = rt.DORotate(new Vector3(0, 0, -35f), time / 2f);
        leftTween.SetEase(Ease.Linear);
        shakeSeq.Append(leftTween);

        var rightTween = rt.DORotate(new Vector3(0, 0, 35f), time);
        rightTween.SetEase(Ease.Linear);
        shakeSeq.Append(rightTween);

        var resetTween = rt.DORotate(Vector3.zero, time);
        resetTween.SetEase(Ease.Linear);
        shakeSeq.Append(resetTween);

        shakeSeq.Play();
    }
    private void PlayAudio(AudioClip clip)
    {
        if (audioPlayer.isPlaying)
            audioPlayer.Stop();
        audioPlayer.clip = clip;
        audioPlayer.Play();
    }
}
