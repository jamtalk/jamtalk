using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.EventSystems;

public class JT_PL2_106 : BaseContents
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL2_106;
    protected override bool CheckOver() => QuestionCount == index;
    protected override int GetTotalScore() => QuestionCount;
    protected int QuestionCount => 5;
    protected virtual int WordsCount => 6;
    private int index = 0;

    public Button longButton;
    public Button shortButton;
    public Button spinButton;
    public Toggle[] currentCount;
    public Image rouletteImage;
    public GameObject parent;
    public GameObject rouletteElement;
    public Image rouletteEffect;

    public AudioClip rouletteClip;
    public AudioClip tabClip;
    public AudioClip currentClip;

    private VowelWordsData[] shortWords;
    private VowelWordsData[] longWords;

    private Sequence seq;
    private List<TextElement206> textList = new List<TextElement206>();
    private int currentIndex;
    private List<VowelWordsData> datas = new List<VowelWordsData>();

    protected override void Awake()
    {          
        base.Awake();
        Init();
        rouletteEffect.gameObject.SetActive(false);
        shortButton.interactable = false;
        longButton.interactable = false;
        spinButton.onClick.AddListener(() => Spin());
        shortButton.onClick.AddListener(() => ButtonListener(shortButton));
        longButton.onClick.AddListener(() => ButtonListener(longButton));
    }

    private void Init()
    {
        shortWords = GameManager.Instance.GetResources().Vowels
            .Where(x => x.VowelType == eVowelType.Short)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(WordsCount)
            .ToArray();

        longWords = GameManager.Instance.GetResources().Vowels
            .Where(x => x.VowelType == eVowelType.Long)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(WordsCount)
            .ToArray();

        for (int i = 0; i < shortWords.Length; i++)
        {
            var angle = 15f + (float)(360 / (WordsCount * 2)) * i;

            var layout = Instantiate(rouletteElement, parent.transform);
            layout.transform.localRotation = Quaternion.Euler(0, 0, angle);
            var text = layout.GetComponentInChildren<TextElement206>();

            text.Init(shortWords[i]);

            textList.Add(text);
            datas.Add(shortWords[i]);
        }

        for (int i = 0; i < longWords.Length; i++)
        {
            var angle = 15f + (float)(360 / (WordsCount * 2)) * (WordsCount + i);

            var layout = Instantiate(rouletteElement, parent.transform);
            layout.transform.localRotation = Quaternion.Euler(0, 0, angle);
            var text = layout.GetComponentInChildren<TextElement206>();

            text.Init(longWords[i]);

            textList.Add(text);
            datas.Add(longWords[i]);
        }
    }

    private void ButtonListener(Button button)
    {
        audioPlayer.Play(tabClip);

        for(int i = 0; i < WordsCount; i++)
        {
            if (button.name == "ShortButton")
            {
                if (textList[currentIndex].text.Contains(shortWords[i].key))
                {
                    currentCount[index].isOn = true;
                    index += 1;
                    audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
                    {
                        if (CheckOver())
                            ShowResult();
                    });
                    shortButton.interactable = false;
                    longButton.interactable = false;
                    break;
                }
            }
            else if (button.name == "LongButton")
            {
                if (textList[currentIndex].text.Contains(longWords[i].key))
                {
                    currentCount[index].isOn = true;
                    index += 1;
                    audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
                    {
                        if (CheckOver())
                            ShowResult();
                    });
                    shortButton.interactable = false;
                    longButton.interactable = false;
                    break;
                }
            }
        }
    }

    private void Spin()
    {
        rouletteEffect.gameObject.SetActive(true);
        shortButton.interactable = true;
        longButton.interactable = true;

        if (seq != null)
        {
            seq.Kill();
            seq = null;
        }
        audioPlayer.Play(6f, rouletteClip);
        eventSystem.enabled = false;

        seq = DOTween.Sequence();
        currentIndex = Random.Range(0, textList.Count);

        var targetAngle = 360 - textList[currentIndex].transform.parent.localRotation.eulerAngles.z;
        var firstTween = rouletteImage.transform.DORotate(new Vector3(0, 0, 360f), 2f, RotateMode.FastBeyond360);
        var secondTween = rouletteImage.transform.DORotate(new Vector3(0, 0, 360f), 2f, RotateMode.FastBeyond360);
        var lastTween = rouletteImage.transform.DORotate(new Vector3(0, 0, targetAngle), 2f,RotateMode.FastBeyond360);

        Debug.LogFormat("text : {0}, rect : {1}" ,textList[currentIndex], targetAngle);
        firstTween.SetEase(Ease.InQuad);
        secondTween.SetEase(Ease.Linear);
        lastTween.SetEase(Ease.OutQuad);
        seq.Append(firstTween);
        seq.Append(secondTween);
        seq.Append(lastTween);

        seq.onComplete += () =>
        {
            eventSystem.enabled = true;
            rouletteEffect.gameObject.SetActive(false);
            audioPlayer.Play(textList[currentIndex].data.clip);
        };
        seq.Play();
    }
}
