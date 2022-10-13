using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class JT_PL3_105 : BaseContents
{
    public EventSystem eventSystem;
    public Button buttonBox;
    protected override eContents contents => eContents.JT_PL3_105;
    protected override bool CheckOver() => index == QuestionCount;
    protected override int GetTotalScore() => QuestionCount;
    protected int QuestionCount => 5;
    private int index = 0;
    
    private DigraphsWordsData currentDigraphs;
    private string digraphs;

    public Text currentText;
    public GameObject hammer;
    public Image effectImage;
    public Image progressBar;
    public MoleElement305[] elements;
    public RectTransform[] layouts;
    public AudioClip hammerClip;
    public AudioClip moleClip;
    public AudioClip effectClip;


    bool isNext = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for(int i = 0; i < QuestionCount; i++)
        {
            var target = elements
                .Where(x => x.text.text == currentDigraphs.digraphs.ToLower()
                || x.text.text == currentDigraphs.PairDigrpahs.ToString().ToLower()).First();

            while (!isNext) yield return null;
            isNext = false;

            guideFinger.DoMoveCorrect(target.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    ClickMotion(target);
                });
            });

            while (!isNext) yield return null;
            isNext = false;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        MakeQuestion();
    }


    protected void MakeQuestion()
    {
        currentDigraphs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .Distinct()
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
        audioPlayer.Play(currentDigraphs.clip, () => isNext = true);
        if (currentDigraphs.key.IndexOf(currentDigraphs.digraphs.ToLower()) < 0)
            digraphs = currentDigraphs.PairDigrpahs.ToString().ToLower();
        else
            digraphs = currentDigraphs.Digraphs.ToString().ToLower();

        string value = currentDigraphs.key;
        currentText.text = value.Replace(digraphs, "__");

        SetMolesPosition();
        buttonBox.onClick.RemoveAllListeners();
        buttonBox.onClick.AddListener(() => audioPlayer.Play(currentDigraphs.clip));
    }

    protected void SetMolesPosition()
    {
        var tempLayouts = layouts
            .OrderBy(x => Random.Range(0f, layouts.Length))
            .Take(3)
            .ToArray();

        var temp = GameManager.Instance.digrpahs
            .Where(x => x != GameManager.Instance.currentDigrpahs)
            .Where(x => (int)x < 400)
            .Select(x => x.ToString())
            .Take(2)
            .ToList();
        temp.Add(digraphs);
        var icorrect = temp.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].transform.position = tempLayouts[i].position;
            elements[i].Init(icorrect[i]);
            elements[i].gameObject.SetActive(true);
            AddListener(elements[i]);
        }
    }

    private void AddListener(MoleElement305 element)
    {
        var button = element.button;
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            ClickMotion(element);
        });
    }

    private void ClickMotion(MoleElement305 element)
    {
        eventSystem.enabled = false;
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

                if (element.text.text.Contains(digraphs))
                {
                    index += 1;
                    currentText.text = currentDigraphs.key;
                    ProgressBarDoMove();
                    audioPlayer.Play(currentDigraphs.clip, () =>
                    {
                        isNext = true;
                        eventSystem.enabled = true;
                        if (CheckOver())
                            if(!isGuide)
                                ShowResult();
                            else
                            {
                                index = 0;
                                isGuide = false;
                                MakeQuestion();
                                progressBar.transform.localScale = new Vector3(0f, 1f, 1f);
                            }
                        else
                            MakeQuestion();
                    });
                }
                else
                {
                    SetMolesPosition();
                    eventSystem.enabled = true;
                }
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