using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL2_103 : BaseContents
{
    protected override eContents contents => eContents.JT_PL2_103;
    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;

    private int index = 0;
    private int questionCount => 1;
    private int wordsCount = 5;

    private List<WordElement203> shortsElements = new List<WordElement203>();
    private List<WordElement203> longElements = new List<WordElement203>();
    private List<DragWordElement203> throwingElements = new List<DragWordElement203>();
    private List<DragWordElement203> throwingLongElements = new List<DragWordElement203>();
    private List<WordElement203> UnionElements => shortsElements.Union(
        throwingElements.Select(x => x.GetComponent<WordElement203>()))
        .ToList();
    private List<WordElement203> UnionLongElements => longElements.Union(
        throwingLongElements.Select(x => x.GetComponent<WordElement203>()))
        .ToList();

    private WordsData.WordSources[] words;
    private WordsData.WordSources[] tempWords;
    private WordsData.WordSources currentWord => words[index];

    public UIThrower110 thrower;

    [Header("layout")]
    public RectTransform wordShortParent;
    public RectTransform wordLongParent;
    public RectTransform shotThrowParent;
    public RectTransform longThrowParent;

    public GameObject prefabWordElement;
    public GameObject prefabDragWordElement;

    [Header("UI")]
    public GameObject popupCureent;
    public Image popupImage;
    public Sprite longImage;
    public Button shortButton;
    public Button longButton;

    public AudioClip startClip;
    public AudioClip currentClip;
    public AudioClip tabClip;
    public AudioSinglePlayer audioPlayer;
    public EventSystem eventSystem;

    protected override void Awake()
    {
        base.Awake();
        GetWords();
        StartCoroutine(Init(currentWord));

        audioPlayer.Play(startClip);

        for(int i = 0; i < throwingElements.Count; i++)
        {
            throwingElements[i].onDrag += OnDrag;
            throwingLongElements[i].onDrag += OnDrag;
        }

        var alphabet = currentWord.alphabet;
        longButton.onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.GetResources(alphabet).AudioData.phanics));
        shortButton.onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.GetResources(alphabet).AudioData.phanics));
    }

    private void GetWords()
    {
        words = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(wordsCount)
            .ToArray();
        tempWords = GameManager.Instance.alphabets
                .Where(x => x != GameManager.Instance.currentAlphabet)
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(wordsCount)
                .ToArray();
    }

    private IEnumerator Init(WordsData.WordSources data)
    {
        shortsElements.Clear();
        var list = new List<RectTransform>();
        for (int i = 0; i < words.Length; i++)
        {
            var shortElement = Instantiate(prefabWordElement, wordShortParent).GetComponent<WordElement203>();
            shortElement.Init(words[i].value);
            shortElement.visible = false;
            shortsElements.Add(shortElement);

            var longElement = Instantiate(prefabWordElement, wordLongParent).GetComponent<WordElement203>();
            longElement.Init(tempWords[i].value);
            longElement.GetComponent<Image>().sprite = longImage;
            longElement.visible = false;
            longElements.Add(longElement);
        }
        for (int i = 0; i < shortsElements.Count; i++)
        {
            var shortElement = Instantiate(prefabDragWordElement, shotThrowParent).GetComponent<DragWordElement203>();
            shortElement.Init(shortsElements[i].value);
            shortElement.visible = true;
            throwingElements.Add(shortElement);
            list.Add(shortElement.GetComponent<RectTransform>());
            shortElement.onDrop += OnDrop;
            //shortElement.onDrag += OnDrag;

            var longElement = Instantiate(prefabDragWordElement, longThrowParent).GetComponent<DragWordElement203>();
            longElement.Init(longElements[i].value);
            longElement.visible = true;
            longElement.GetComponent<Image>().sprite = longImage;
            throwingLongElements.Add(longElement);
            list.Add(longElement.GetComponent<RectTransform>());
            longElement.onDrop += OnDrop;
            //longElement.onDrag += OnDrag;
        }
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < UnionElements.Count; i++)
        {
            UnionElements[i].SetSize();
            UnionLongElements[i].SetSize();
        }
        for (int i = 0; i < shortsElements.Count; i++)
        {
            var size = throwingElements[i].GetComponent<RectTransform>().sizeDelta;
            size.y = shortsElements[i].GetComponent<RectTransform>().sizeDelta.y;
            throwingElements[i].GetComponent<RectTransform>().sizeDelta = size;
            throwingElements[i].transform.position = shortsElements[i].transform.position;

            var longSize = throwingLongElements[i].GetComponent<RectTransform>().sizeDelta;
            size.y = longElements[i].GetComponent<RectTransform>().sizeDelta.y;
            throwingLongElements[i].GetComponent<RectTransform>().sizeDelta = size;
            throwingLongElements[i].transform.position = longElements[i].transform.position;
        }
        yield return new WaitForEndOfFrame();

        shotThrowParent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        longThrowParent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        thrower.Init(list.ToArray());
        eventSystem.enabled = false;
        thrower.Throwing(delay: 3f, rotating: false, onTrowed: () =>
        {
            eventSystem.enabled = true;
            for (int i = 0; i < throwingElements.Count; i++)
            {
                throwingElements[i].SetDefaultPosition();
                throwingLongElements[i].SetDefaultPosition();
            }
        });
    }

    private void OnDrag(DragWordElement203 target)
    {
        audioPlayer.Play(1f, tabClip);
    }

    private void OnDrop(WordElement203 target)
    {
        for(int i = 0; i < words.Length; i ++)
        {
            if (words[i].value.Contains(target.textValue.text))
                popupImage.sprite = words[i].sprite;
            if(tempWords[i].value.Contains(target.textValue.text))
                popupImage.sprite = tempWords[i].sprite;
        }

        popupImage.preserveAspect = true;
        popupCureent.GetComponentInChildren<Text>().text = target.textValue.text;
        popupCureent.gameObject.SetActive(true);
        
        audioPlayer.Play(1f, currentClip, () => popupCureent.gameObject.SetActive(false));
        target.visible = true;
        if (!shortsElements.Select(x => x.visible).Contains(false)
            && !longElements.Select(x => x.visible).Contains(false))
        {
            index += 1;
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                if (CheckOver())
                    ShowResult();
                else
                    StartCoroutine(Init(currentWord));
            });
        }
    }
}