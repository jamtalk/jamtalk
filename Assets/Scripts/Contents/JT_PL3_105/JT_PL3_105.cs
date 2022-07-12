using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL3_105 : BaseContents
{
    protected override eContents contents => eContents.JT_PL3_105;
    protected override bool CheckOver() => index == QuestionCount;
    protected override int GetTotalScore() => QuestionCount;
    protected int QuestionCount => 5;
    private int index = 0;
    
    private eDigraphs eCurrentDigraphs;
    private DigraphsWordsData currentDigraphs;

    public Text currentText;
    public GameObject hammer;
    public Image effectImage;
    public Image progressBar;
    public MoleElement305[] elements;
    public RectTransform[] layouts;
    public AudioClip hammerClip;
    public AudioClip moleClip;
    public AudioClip effectClip;

    private eDigraphs[] eDig = { eDigraphs.CH, eDigraphs.SH, eDigraphs.TH };

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }


    protected void MakeQuestion()
    {
        eCurrentDigraphs = eDig[Random.Range(0, eDig.Length)];

        currentDigraphs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))  
            .Where(x => x.Digraphs == eCurrentDigraphs)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
        audioPlayer.Play(currentDigraphs.clip);

        string value = currentDigraphs.key;
        currentText.text = value.Replace(eCurrentDigraphs.ToString().ToLower(), "__");

        SetMolesPosition();
    }

    protected void SetMolesPosition()
    {
        var tempLayouts = layouts
            .OrderBy(x => Random.Range(0f, layouts.Length))
            .Take(3)
            .ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].transform.position = tempLayouts[i].position;
            elements[i].gameObject.SetActive(true);
            AddListener(elements[i]);
        }
    }

    private void AddListener(MoleElement305 element)
    {
        var button = element.GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            hammer.transform.position = element.transform.position;
            var hammerRt = hammer.GetComponent<RectTransform>();
            var hammerWidth = hammerRt.rect.width;
            var posion = hammerRt.anchoredPosition;
            posion.x += hammerWidth;

            hammerRt.anchoredPosition = posion;

            HammerDoMove(() =>
            {
                hammer.gameObject.SetActive(false);
                effectImage.transform.position = element.transform.position;
                effectImage.gameObject.SetActive(true);

                audioPlayer.Play(1f, effectClip, () =>
                {
                    effectImage.gameObject.SetActive(false);

                    if (element.text.text.ToUpper().Contains(eCurrentDigraphs.ToString()))
                    {
                        audioPlayer.Play(currentDigraphs.clip, () =>
                        {
                            currentText.text = currentDigraphs.key;
                            ProgressBarDoMove();
                        });
                        index += 1;

                        if (CheckOver())
                            ShowResult();
                        else
                            MakeQuestion();

                    }
                    else
                    {
                        SetMolesPosition();
                    }
                });
                
            });
            
        });
    }

    private void ProgressBarDoMove()
    {
        var vector = new Vector3((0.2f * index), 0f, 0f);
        progressBar.transform.DOScaleX(vector.x, .5f);
    }

    private void HammerDoMove(TweenCallback callback)
    {
        hammer.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        Tween startTween;
        Tween lastTween;
        var startVector = new Vector3(0, 0, -5);
        var lastVector = new Vector3(0, 0, 15);
        startTween = hammer.gameObject.transform.DORotate(startVector, .5f);
        lastTween = hammer.gameObject.transform.DORotate(lastVector, .5f);

        startTween.SetEase(Ease.Linear);
        lastTween.SetEase(Ease.InQuad);
        seq.Append(startTween);
        seq.Append(lastTween);
        seq.onComplete += callback;
        seq.Play();
    }

}