using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_108 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_108;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;

    private int answerIndex = 0;

    public Sprite textSprite;
    public Image currentImage;
    public Button currentButton;
    public RectTransform potionParent;
    public RectTransform textsParent;
    public SplitElement_408 potionElement;
    public GameObject textElement;

    private DigraphsWordsData current;
    private string[] questionTexts;
    private List<string> answerTexts = new List<string>();
    private List<SplitElement_408> answerElements = new List<SplitElement_408>();
    protected override void Awake()
    {
        base.Awake();
        MakeQuestion();
        currentButton.onClick.AddListener(() => audioPlayer.Play(current.clip));
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
        audioPlayer.Play(current.clip);
        ShowQuestion();
    }

    private void ShowQuestion()
    {
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
                var colorText = ("<color=\"red\">" + answerTexts[i] + "</color>");
                text.Init(colorText, textSprite);
            }
            else
                text.Init(answerTexts[i], textSprite);
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
                answerElements[answerIndex].text.gameObject.SetActive(true);
                answerIndex += 1;
            }

            if(answerIndex == answerElements.Count)
            {
                audioPlayer.Play(current.clip, () =>
                {
                    index += 1;
                    if (CheckOver())
                        ShowResult();
                    else
                        MakeQuestion();
                });
            }
        });
    }

    private void Clear()
    {
        answerIndex = 0;

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
}
