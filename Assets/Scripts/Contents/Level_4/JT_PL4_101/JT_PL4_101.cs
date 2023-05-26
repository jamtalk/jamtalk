using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL4_101 : BaseContents<DigraphsContentsSetting>
{
    protected override eContents contents => eContents.JT_PL4_101;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private int slideCount = 0;

    public GameObject[] addImages;
    public Animator[] anis;
    public Text[] digraphsTexts;
    public Text[] pairsTexts;
    public Text[] splitTexts;
    public SliderElement[] sliderElements;
    public MonsterElemet[] monsters;
    public Image successImages;

    private DigraphsWordsData data;

    
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        foreach(var item in sliderElements)
        {
            guideFinger.DoMove(item.text.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    ClickMotion(item);
                });
            });

            while (!isNext) yield return null;
            isNext = false;
        }
    }
    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < anis.Length; i++)
            anis[i].SetBool("pacMan", true);

        MakeQuestion();

        foreach (var item in sliderElements)
        {
            item.button.onClick.AddListener(() => ClickMotion(item));
            item.onCollision += () => audioPlayer.PlayIncorrect();
        }
    }

    private void MakeQuestion()
    {
        ResetScene();

        data = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .First();

        var pair = ResourceSchema.GetPair(data.Digraphs).ToString().ToLower();
        var digraph = data.digraphs.ToLower();

        for (int i = 0; i < digraphsTexts.Length; i++)
        {
            digraphsTexts[i].text = digraph;
            pairsTexts[i].text = pair;
        }

        var values = new List<string>();
        var temp = digraph + pair;

        foreach (var item in temp)
            values.Add(item.ToString());

        for (int i = 0; i < values.Count; i++)
            splitTexts[i].text = values[i];
    }

    private void ClickMotion(SliderElement element)
    {
        var alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), element.text.text.ToUpper());
        element.button.interactable = false;

        audioPlayer.Play(GameManager.Instance.schema.GetAlphabetAudio(alphabet).clip, () =>
        {
            element.Move(true, () =>
            {
                element.isCompleted = true;
                isNext = true;
                slideCount += 1;
                element.slider.gameObject.SetActive(false);
                element.addImage.gameObject.SetActive(true);

                audioPlayer.Play(data.audio.phanics, () =>
                {
                    if (slideCount >= 2)
                    {
                        if (!isGuide)
                            ShowResult();
                        else
                        {
                            EndGuidnce();
                            MakeQuestion();
                        }
                    }
                });
            });
        });
    }

    protected override void EndGuidnce()
    {
        audioPlayer.Stop();
        foreach (var item in sliderElements)
            item.Stop();

        base.EndGuidnce();

        foreach (var item in monsters)
            item.StartCoroutine();
        ResetScene();
    }
    private void ResetScene()
    {
        successImages.gameObject.SetActive(true);
        slideCount = 0;
        index += 1;

        foreach(var item in sliderElements)
        {
            item.slider.value = 25f;
            item.addImage.gameObject.SetActive(false);
            item.slider.gameObject.SetActive(true);
            item.button.interactable = true;
            item.isCompleted = false;
        }
        successImages.gameObject.SetActive(false);

        
    }

}
