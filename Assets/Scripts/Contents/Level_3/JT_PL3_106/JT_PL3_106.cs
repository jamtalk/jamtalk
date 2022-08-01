using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_106 : BaseContents
{
    protected override eContents contents => eContents.JT_PL2_106;
    protected override bool CheckOver() => index == QuestionCount;
    protected override int GetTotalScore() => QuestionCount;
    protected  int QuestionCount => 3;
    private int index = 0;

    private DigraphsWordsData currentDigraphs;
    private string digraphs;

    public Thrower306 thrower;
    public Text[] texts;
    public Text currentText;
    public Button currentButton;
    public Image currentImage;
    public Image bagImage;
    [SerializeField]
    private List<DoubleClick306> elements = new List<DoubleClick306>();

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }

    private void SetCurrentImage()
    {
        currentText.text = currentDigraphs.key;
        currentImage.sprite = currentDigraphs.sprite;
        currentImage.name = currentDigraphs.key;
        currentImage.preserveAspect = true;
        currentImage.gameObject.SetActive(true);

        for (int i = 0; i < elements.Count; i++)
            elements[i].gameObject.SetActive(true);

        thrower.gameObject.SetActive(false);
        bagImage.gameObject.SetActive(false);
    }

    private void SetBagImage()
    {
        bagImage.sprite = currentImage.sprite;
        bagImage.preserveAspect = true;
        bagImage.gameObject.SetActive(true);
    }

    protected void MakeQuestion()
    {
        currentDigraphs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
        currentImage.sprite = currentDigraphs.sprite;
        currentImage.name = currentDigraphs.key;
        currentImage.preserveAspect = true;

        if (currentDigraphs.key.IndexOf(currentDigraphs.digraphs.ToLower()) < 0)
            digraphs = currentDigraphs.PairDigrpahs.ToString().ToLower();
        else
            digraphs = currentDigraphs.Digraphs.ToString().ToLower();

        currentText.text = currentDigraphs.key.Replace(digraphs, "__");

        ShowQuestion();
    }

    protected void ShowQuestion()
    {
        var temp = GameManager.Instance.digrpahs
            .Where(x => x != GameManager.Instance.currentDigrpahs)
            .Where(x => (int)x < 400)
            .Select(x => x.ToString())
            .Take(2)
            .ToList();
        temp.Add(digraphs);
        var icorrect = temp.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].Init(icorrect[i]);    
            elements[i].isOn = false;     
            AddDoubleClickListener(elements[i], currentDigraphs); 
        }
    }

    protected virtual void AddDoubleClickListener(DoubleClick306 element, DigraphsWordsData data)
    {
        element.onClickFirst.RemoveAllListeners();
        element.onClick.RemoveAllListeners();
        
        var clip = GameManager.Instance.schema.GetDigrpahsAudio(element.digraphs);

        element.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(clip.phanics);
        });

        element.onClick.AddListener(() =>
        {
            if (currentDigraphs.key.Contains(element.name))
            {
                index += 1;

                for (int i = 0; i < elements.Count; i++)
                    elements[i].gameObject.SetActive(false);
                currentImage.gameObject.SetActive(false);

                thrower.GetComponent<Image>().sprite = currentImage.sprite;
                thrower.gameObject.SetActive(true);
                thrower.Throw(currentImage, bagImage.GetComponent<RectTransform>(), () =>
                {
                    currentText.text = data.key;
                    SetBagImage();

                    audioPlayer.Play(currentDigraphs.act, () =>
                    {
                        SetCurrentImage();
                        if (CheckOver())
                            ShowResult();
                        else
                            MakeQuestion();
                    });
                });
            }
            else
            {
                audioPlayer.Play(clip.phanics);
            }
        });
    }
}
