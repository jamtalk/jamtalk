using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JT_PL4_108 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_108;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 6;
    private int index = 0;

    private int answerIndex = 0;

    public Sprite textSprite;
    public Image currentImage;
    public Button currentButton;
    public RectTransform potionParent;
    public RectTransform textsParent;
    public SplitElement_408 potionElement;
    public GameObject textElement;
    private DigraphsWordsData[] questions;
    private DigraphsWordsData current => questions[index];
    private string[] questionTexts;
    private List<string> answerTexts = new List<string>();
    private List<SplitElement_408> answerElements = new List<SplitElement_408>();
    private List<SplitElement_408> optionElements = new List<SplitElement_408>();

    
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for (int j = 0; j < questionCount; j++)
        {
            for (int i = 0; i < optionElements.Count; i++)
            {
                var target = optionElements.Where(x => x.value == answerElements[i].value).First();
                guideFinger.DoMove(target.transform.position, () =>
                {
                    guideFinger.DoClick(() =>
                    {
                        CountMotion();
                        isNext = true;
                    });
                });

                while (!isNext) yield return null;
                isNext = false;
            }

            guideFinger.gameObject.SetActive(false);
            CorrctMotion();
            while (!isNext) yield return null;
            isNext = false;
        }
    }
    protected override void Awake()
    {
        StartCoroutine(WaitFrame(() =>
        {
            currentImage.gameObject.SetActive(true);
            Resize();
            base.Awake();
            questions = GameManager.Instance.GetDigraphs()
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(questionCount)
                .ToArray();

            currentButton.onClick.AddListener(() => audioPlayer.Play(current.clip));
            ShowQuestion();
        }));
    }
    private void Resize()
    {
        Resize(potionElement.GetComponent<RectTransform>(), potionParent);
        Resize(textElement.GetComponent<RectTransform>(), textsParent);
    }
    private void Resize(RectTransform target, RectTransform parent)
    {
        var sprite = target.GetComponent<Image>().sprite;
        var height = parent.rect.height;
        var width = parent.rect.height * sprite.texture.width / sprite.texture.height;
        target.sizeDelta = new Vector2(width, height);
    }

    private void ShowQuestion()
    {
        audioPlayer.Play(current.clip);
        Clear();
        answerTexts.Clear();
        answerElements.Clear();

        var digraphs = string.Empty;
        if(current.key.IndexOf(current.digraphs.ToLower()) < 0)
            digraphs = current.PairDigrpahs.ToString().ToLower();
        else
            digraphs = current.Digraphs.ToString().ToLower();

        currentImage.sprite = current.sprite;
        currentImage.preserveAspect = true;

        var temp = current.key.Replace(digraphs, string.Empty);
        var tempList = new List<string>();
        foreach (var item in temp)
            tempList.Add(item.ToString());
        tempList.Add(digraphs);

        questionTexts = tempList
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();


        var digraphsIndex = current.key.IndexOf(digraphs);

        foreach (var item in temp)
            answerTexts.Add(item.ToString());
        answerTexts.Insert(digraphsIndex, digraphs);

        SetElements(digraphs);
    }

    private void SetElements(string digraphs)
    {
        for (int i = 0; i < questionTexts.Length; i++)
        {
            var potion = Instantiate(potionElement, potionParent.transform).GetComponent<SplitElement_408>();
            potion.Init(questionTexts[i]);
            AddListener(potion);

            var text = Instantiate(textElement, textsParent.transform).GetComponent<SplitElement_408>();
            if( digraphs == answerTexts[i])
            {
                text.Init(answerTexts[i], textSprite, true);
            }
            else
                text.Init(answerTexts[i], textSprite);
            optionElements.Add(potion);
            answerElements.Add(text);
        }
    }

    private void AddListener(SplitElement_408 element)
    {
        var button = element.GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            if (element.text.text.Contains(answerTexts[answerIndex]))
            {
                CountMotion();
            }

            if(answerIndex == answerElements.Count)
            {
                CorrctMotion();   
            }
        });
    }

    private void CountMotion()
    {
        answerElements[answerIndex].text.gameObject.SetActive(true);
        answerIndex += 1;
    }

    private void CorrctMotion()
    {
        audioPlayer.Play(current.clip, () =>
        {
            index += 1;
            if (CheckOver())
                if(!isGuide)
                    ShowResult();
                else
                {
                    isGuide = false;
                    index = 0;
                    ShowQuestion();
                }
            else
                ShowQuestion();

            isNext = true;
        });
    }

    private void Clear()
    {
        answerIndex = 0;
        optionElements.Clear();
        var targets = new List<GameObject>();
        for(int i = 0; i < potionParent.childCount; i++)
        {
            targets.Add(potionParent.GetChild(i).gameObject);
            targets.Add(textsParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < targets.Count; i++)
            Destroy(targets[i]);
        targets.Clear();
    }

    private IEnumerator WaitFrame(Action callback)
    {
        yield return new WaitForEndOfFrame();
        callback?.Invoke();
    }
}
