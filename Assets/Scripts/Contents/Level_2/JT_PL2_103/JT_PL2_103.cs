using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL2_103 : BaseContents
{
    protected override eContents contents => eContents.JT_PL2_103;
    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;

    private int index = 0;
    private int questionCount => 1;
    private int wordsCount = 5;


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
    public Button buttonCurrent;
    public Image popupImage;
    public Sprite longImage;
    public Button shortButton;
    public Button longButton;

    public AudioClip startClip;
    public AudioClip currentClip;
    public AudioClip tabClip;
    public EventSystem eventSystem;

    private VowelWordsData[] questions;
    private List<WordElement203> elements = new List<WordElement203>();
    private List<DragWordElement203> throwingElements = new List<DragWordElement203>();

    bool isNext = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        foreach(var item in questions)
        {
            var dragTarget = throwingElements.Where(x => x.gameObject.activeSelf)
                .OrderBy(x => Random.Range(0, 100)).First();
            var dropTarget = elements.Where(x => x.value == dragTarget.value).First();

            while (!isNext) yield return null;
            isNext = true;

            guideFinger.DoMove(dragTarget.transform.position, () =>
            {
                guideFinger.DoPress(() =>
                {
                    guideFinger.DoMove(dropTarget.transform.position, () =>
                    {

                    });
                });
            });

            while (true) yield return null;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        buttonCurrent.onClick.AddListener(OnClickCurrent);
        shortButton.onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.schema.GetVowelAudio(questions[index].Vowel).phanics_short));
        longButton.onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.schema.GetVowelAudio(questions[index].Vowel).phanics_long));
        for (int i = 0; i < throwingElements.Count; i++)
        {
            throwingElements[i].onDrag += OnDrag;
        }

        StartContents();
    }

    private void StartContents()
    {
        audioPlayer.Play(startClip);

        questions = MakeQuestion();
        StartCoroutine(ShowQuestion());
    }

    private VowelWordsData[] MakeQuestion()
    {
        var shorts = GameManager.Instance.GetResources().Vowels
            .Where(x => x.VowelType == eVowelType.Short)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(wordsCount)
            .ToArray();

        var longs = GameManager.Instance.GetResources().Vowels
            .Where(x => x.VowelType == eVowelType.Long)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(wordsCount)
            .ToArray();

        var vowels = shorts.Union(longs).ToArray();
        return vowels;
    }

    private IEnumerator ShowQuestion()
    {
        var list = new List<RectTransform>();

        for (int i = 0; i < questions.Length; i++)
        {
            var type = questions[i].VowelType;
            var parent = type == eVowelType.Short ? wordShortParent : wordLongParent;
            var dragParent = type == eVowelType.Short ? shotThrowParent : longThrowParent;
            // element 생성 
            WordElement203 element;
            element = Instantiate(prefabWordElement, parent).GetComponent<WordElement203>();
            if (type == eVowelType.Long)
                element.GetComponent<Image>().sprite = longImage;
            
            element.Init(questions[i]);
            element.visible = false;
            elements.Add(element);

            // drag elemet 생성 
            var dragElement = Instantiate(prefabDragWordElement, dragParent).GetComponent<DragWordElement203>();
            dragElement.Init(elements[i].value);
            if(type == eVowelType.Long)
                dragElement.GetComponent<Image>().sprite = longImage;

            dragElement.visible = true;
            throwingElements.Add(dragElement);
            list.Add(dragElement.GetComponent<RectTransform>());
            dragElement.onDrop += OnDrop;

            var size = throwingElements[i].GetComponent<RectTransform>().sizeDelta;
            size.y = elements[i].GetComponent<RectTransform>().sizeDelta.y;
            throwingElements[i].GetComponent<RectTransform>().sizeDelta = size;
            throwingElements[i].transform.position = elements[i].transform.position;
        }
        yield return new WaitForEndOfFrame();
        var unionElements = elements.Union(throwingElements).ToArray();
        foreach (var item in unionElements)
            item.SetSize();

        yield return new WaitForEndOfFrame();
        shotThrowParent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        longThrowParent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        eventSystem.enabled = false;

        thrower.Init(list.ToArray());
        thrower.Throwing(delay: 3f, rotating: false, onTrowed: () =>
        {
            eventSystem.enabled = true;
            for (int i = 0; i < throwingElements.Count; i++)
            {
                throwingElements[i].SetDefaultPosition();
            }
            isNext = true;
        });
    }

    private void OnDrag(DragWordElement203 target)
    {
        audioPlayer.Play(1f, tabClip);
    }

    private void OnDrop(WordElement203 target)
    {
        index++;
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i].key.Contains(target.textValue.text))
                popupImage.sprite = questions[i].sprite;
        }
        popupImage.preserveAspect = true;
        popupCureent.GetComponentInChildren<Text>().text = target.textValue.text;
        popupCureent.gameObject.SetActive(true);

        audioPlayer.Play(target.data.clip);
        StartCoroutine(WaitSeconds());

        target.visible = true;
    }
    private IEnumerator WaitSeconds()
    {
        eventSystem.enabled = false;
        yield return new WaitForSecondsRealtime(1);
        audioPlayer.Play(1f, currentClip, () =>
        {
            eventSystem.enabled = true;
            popupCureent.gameObject.SetActive(false);
            if (CheckOver())
                if(!isGuide)
                    ShowResult();
                else
                {
                    index = 0;
                    elements.Clear();
                    throwingElements.Clear();

                    StartContents();
                }
        });
    }
    private void OnClickCurrent()
    {
        popupCureent.gameObject.SetActive(false);
        if (!elements.Select(x => x.visible).Contains(false))
        {
            ShowResult();
        }
    }
}
