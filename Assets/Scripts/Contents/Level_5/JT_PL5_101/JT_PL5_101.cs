using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
//using GJGameLibrary.Util.Bezier.DoTween;

public class JT_PL5_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_101;
    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;

    public Image hole;
    public Image[] alphabetImages;
    public RectTransform[] leftWaypoint;
    public RectTransform[] rightWaypoint;
    private DigraphsWordsData digraphs;
    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }
    private void MakeQuestion()
    {
        digraphs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
            .First();

        var temp = digraphs.Digraphs.ToString().ToCharArray();

        for(int i = 0; i < temp.Length; i++)
        {
            eAlphabet alphabets = (eAlphabet)Enum.Parse(typeof(eAlphabet), temp[i].ToString());
            var alphabet = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, alphabets);
            alphabetImages[i].sprite = alphabet;
            alphabetImages[i].preserveAspect = true;
        }

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        DoMove(leftWaypoint, alphabetImages[0]);
        DoMove(rightWaypoint, alphabetImages[1]);

        audioPlayer.Play(digraphs.act);
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3f);

        DoHoleIn(alphabetImages, () =>
        {
            for (int i = 0; i < alphabetImages.Length; i++)
                alphabetImages[i].gameObject.SetActive(false);

            ShowResult();
        });
    }

    private void DoMove(RectTransform[] waypoint, Image image)
    {
        var seq = DOTween.Sequence();

        for (int i = 0; i < waypoint.Length; i++)
        {
            var tween = image.transform.DOMove(waypoint[i].position, 1f);
            tween.SetEase(Ease.Linear);
            seq.Append(tween);
        }

        seq.Play();
    }

    private void DoHoleIn(Image[] images, TweenCallback callback)
    {
        var seq = DOTween.Sequence();

        for(int i = 0; i < images.Length; i++)
        {
            var tween = images[i].transform.DOMove(hole.rectTransform.position, 1f);
            tween.SetEase(Ease.Linear);
            seq.Insert(0, tween);
        }
        var vector = new Vector3(0f, 0f, 0f);
        var secondTween = hole.transform.DOScale(vector, 1.5f);
        seq.Insert(0, secondTween);

        seq.onComplete += callback;
        seq.Play();
    }

}
