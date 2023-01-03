using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Bezier = GJGameLibrary.Util.Bezier.DoTween.BezierTween;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class JT_PL5_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_101;
    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;

    public EventSystem eventSystem;
    public Image hole;
    public RectTransform[] holePoint;
    public ImageButton501[] anotherButtons;
    public ImageButton501[] alphabetImages;
    public RectTransform[] leftWaypoint;
    public RectTransform[] leftWayPoitn_2;
    public RectTransform[] rightWaypoint;
    private DigraphsWordsData digraphs;
    private string correctString;

    
    private eAlphabet[] eAlphabets = { eAlphabet.A, eAlphabet.E, eAlphabet.I, eAlphabet.O, eAlphabet.U };
    protected override void Awake()
    {
        base.Awake();
        foreach (var item in anotherButtons)
            item.button.onClick.AddListener(() => AddListener(item));
        foreach (var item in alphabetImages)
            item.button.onClick.AddListener(() => AddListener(item));

        MakeQuestion();
    }

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isNext) yield return null;


        guideFinger.DoMove(alphabetImages[0].transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                StartCoroutine(Wait());
            });
        });

    }

    private void AddListener(ImageButton501 image)
    {
        if(image.value == correctString)
        {
            StartCoroutine(Wait());
        }
        else
        {
            audioPlayer.PlayIncorrect();
        }
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        alphabetImages[0].transform.position = leftWaypoint[0].transform.position;
        alphabetImages[1].transform.position = rightWaypoint[0].transform.position;
        anotherButtons[0].transform.position = leftWayPoitn_2[0].transform.position;
        hole.transform.localScale = new Vector3(1f, 1f, 1f);

        foreach (var item in alphabetImages)
            item.gameObject.SetActive(true);

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
        correctString = temp[0].ToString();

        for(int i = 0; i < temp.Length; i++)
        {
            eAlphabet alphabets = (eAlphabet)Enum.Parse(typeof(eAlphabet), temp[i].ToString());
            
            var alphabet = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, alphabets);
            alphabetImages[i].image.sprite = alphabet;
            alphabetImages[i].image.preserveAspect = true;
            alphabetImages[i].value = temp[i].ToString();
        }
        
        foreach(var item in anotherButtons)
        {
            var alphabets = eAlphabets
                .Where(x => x.ToString() != temp[0].ToString())
                .Where(x => x.ToString() != temp[1].ToString())
                .OrderBy(x => Random.Range(0, 100))
                .First();

            var alphabet = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, alphabets);
            item.image.sprite = alphabet;
            item.image.preserveAspect = true;
            item.value = alphabets.ToString();
        }


        MoveAlphabets(() =>
        {
            audioPlayer.Play(digraphs.audio.phanics, () =>
            {
                isNext = true;
            });
        });
    }

    private void MoveAlphabets(TweenCallback callback)
    {
        List<Vector3> vectorLeft = new List<Vector3>();
        List<Vector3> vectorLeft_2 = new List<Vector3>();
        List<Vector3> vectorRight = new List<Vector3>();
        for (int i = 0; i < leftWaypoint.Length; i++)
        {
            vectorLeft.Add(leftWaypoint[i].position);
            vectorLeft_2.Add(leftWayPoitn_2[i].position);
            vectorRight.Add(rightWaypoint[i].position);
        }
        var vectorLefts = vectorLeft.ToArray();
        var vectorLefts_2 = vectorLeft_2.ToArray();
        var vectorRights = vectorRight.ToArray();

        var leftTween = Bezier.Curve(alphabetImages[0].transform, 2.5f, 10, vectorLefts);
        var leftTween_2 = Bezier.Curve(anotherButtons[0].transform, 2.5f, 10, vectorLefts_2);
        var rightTween = Bezier.Curve(alphabetImages[1].transform, 2.5f, 10, vectorRights);

        Sequence seq = DOTween.Sequence();

        seq.Insert(0, leftTween);
        seq.Insert(0, leftTween_2);
        seq.Insert(0, rightTween);

        seq.onComplete += callback;

        seq.Play();
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();

        audioPlayer.Play(digraphs.audio.phanics);
        eventSystem.enabled = false;
        DoHoleIn(alphabetImages, () =>
        {
            eventSystem.enabled = true;
            for (int i = 0; i < alphabetImages.Length; i++)
                alphabetImages[i].gameObject.SetActive(false);

            if (isGuide)
                EndGuidnce();
            else
                ShowResult();
        });
    }
    private void DoMove(Image image, RectTransform[] wayPoint)
    {
        var seq = DOTween.Sequence();

        for( int i = 0; i < wayPoint.Length; i++)
        {
            var tween = image.transform.DOMove(wayPoint[i].position, 1f);
            tween.SetEase(Ease.Linear);
            seq.Append(tween);
        }

        seq.Play();
    }

    private void DoHoleIn(ImageButton[] images, TweenCallback callback)
    {
        var seq = DOTween.Sequence();

        for(int i = 0; i < images.Length; i++)
        {
            var tween = images[i].transform.DOMove(holePoint[i].position, 1f);
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
