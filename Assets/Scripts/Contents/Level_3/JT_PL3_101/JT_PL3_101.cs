using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class JT_PL3_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL3_101;

    protected int index = 0;
    protected virtual int questionCount => 1;
    protected override int GetTotalScore() => index;
    protected override bool CheckOver() => questionCount == index;

    public DragElement301 dragElement;
    public PairColor[] colors;

    public Image[] colorImages;
    public Image resultColorImage;

    public Text[] texts;
    public Text resultText;
    public EventSystem eventSystem;

    private string clip;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        var targets = colorImages.OrderBy(x => Random.Range(0, 100)).ToArray();

        dragElement.brush.transform.position = guideFinger.transform.position;

        guideFinger.DoMove(dragElement.brush.gameObject, resultColorImage.transform.position, () =>
        {
            guideFinger.DoPress(() => isNext = true);
        });

        while (!isNext) yield return null;
        isNext = false;
        dragElement.resultColorImage.DOFade(1, 4f);

        for (int i = 0; i < targets.Length; i++)
        {
            guideFinger.DoMove(dragElement.brush.gameObject,targets[i].transform.position, () =>
            {
                isNext = true;
            });
            while (!isNext) yield return null;
            isNext = false;
        }


        dragElement.isColors = true;
        guideFinger.gameObject.SetActive(false);
        OnDrop(dragElement);
    }
    protected override void Awake()
    {
        base.Awake();
        SetColors(GameManager.Instance.currentDigrpahs);
        dragElement.onDrop += OnDrop;
    }

    private void SetColors(eDigraphs digraphs)
    {
        clip = GameManager.Instance.GetDigraphs(digraphs).First().audio.phanics;
        var targetColor = colors[index % colors.Length];
        var alphabets = digraphs.ToString().Select(x => x.ToString().ToUpper()).ToArray();
        for (int i = 0; i < colorImages.Length; i++)
        {
            colorImages[i].sprite = targetColor.colors[i];
            texts[i].text = alphabets[i];
        }
        resultText.text = digraphs.ToString().ToUpper();
        resultColorImage.sprite = colors[index].result;
    }

    private void OnDrop(DragElement301 target)
    {
        if (dragElement.isColors)
            index += 1;

        resultText.gameObject.SetActive(true);
        audioPlayer.Play(clip, () =>
        {
            if (CheckOver())
            {
                if(!isGuide)
                    ShowResult();
                else
                {
                    isGuide = false;
                    dragElement.isColors = false;
                    index = 0;

                    resultText.gameObject.SetActive(false);
                    Color color = resultColorImage.color;
                    color.a = 0;
                    resultColorImage.color = color;
                }
            }
            else
            {
                //SetColors();
                var color = resultColorImage.color;
                color.a = 0;
                resultColorImage.color = color;
                dragElement.isColors = false;
                resultText.gameObject.SetActive(false);
            }
        });
    }
}

[Serializable]
public class PairColor
{
    public Sprite[] colors;
    public Sprite result;
}