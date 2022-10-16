using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseMatchSentances<T> : BaseContents 
    where T : BaseSentanceData
{
    protected override eContents contents => throw new System.NotImplementedException();
    protected override bool CheckOver() => index == QuestionCount;
    protected override int GetTotalScore() => QuestionCount;

    public AudioClip stampClip;
    public RectTransform sentanceParent;
    public RectTransform throwingParent;
    public GameObject prefabWordElement;
    public GameObject prefabDragWordElement;
    public List<WordElement121> elements = new List<WordElement121>();
    private List<DragWordElement121> throwingElements = new List<DragWordElement121>();
    private List<WordElement121> UnionElements => elements.Union(
        throwingElements.Select(x => x.GetComponent<WordElement121>()))
        .ToList();

    public UIThrower110 thrower;
    protected int index = 0;
    [SerializeField]
    protected T[] words { get; private set; }
    public EventSystem eventSystem;
    protected T currentSentance => words[index];
    protected virtual int QuestionCount => 6;

    bool isThrow = false;
    
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        //for(int i = 0; i < QuestionCount; i++)
        //{
        while (!isThrow) yield return null;
        isThrow = false;

        yield return new WaitForSecondsRealtime(1f);

        for (int j = 0; j < throwingElements.Count; j++)
        {
            var throing = throwingElements
                .Where(x => x.gameObject.activeSelf)
                .OrderBy(x => Random.Range(0, 100)).First();
            var target = elements.Where(x => x.value == throing.value).First();

            guideFinger.DoMove(throing.transform.position, (() =>
            {
                guideFinger.DoPress(() =>
                {
                    guideFinger.DoMove(throing.gameObject, target.transform.position, () =>
                    {
                        guideFinger.gameObject.SetActive(false);
                        throing.gameObject.SetActive(false);
                        guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
                        OnDrop(target);
                    });
                });
            }));


            while (!isNext) yield return null;
            isNext = false;
        }
        //}
    }

    protected override void Awake()
    {
        base.Awake();
        words = GetSentance();
        ShowQuestion();
    }
    protected virtual void ShowQuestion()
    {
        StartCoroutine(Init(currentSentance));
    }
    protected abstract T[] GetSentance();

    private IEnumerator Init(T data)
    {
        Clear();
        elements.Clear();
        var list = new List<RectTransform>();
        for (int i = 0; i < data.words.Length; i++)
        {
            var element = Instantiate(prefabWordElement, sentanceParent).GetComponent<WordElement121>();
            element.Init(data.words[i]);
            element.visible = false;
            elements.Add(element);
        }
        for (int i = 0; i < elements.Count; i++)
        {
            var element = Instantiate(prefabDragWordElement, throwingParent).GetComponent<DragWordElement121>();
            element.Init(elements[i].value);
            element.visible = true;
            throwingElements.Add(element);
            list.Add(element.GetComponent<RectTransform>());
            element.onDrop += OnDrop;
        }
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < UnionElements.Count; i++)
            UnionElements[i].SetSize();
        for (int i = 0; i < elements.Count; i++)
        {
            var size = throwingElements[i].GetComponent<RectTransform>().sizeDelta;
            size.y = elements[i].GetComponent<RectTransform>().sizeDelta.y;
            throwingElements[i].GetComponent<RectTransform>().sizeDelta = size;
            throwingElements[i].transform.position = elements[i].transform.position;
        }
        yield return new WaitForEndOfFrame();
        throwingParent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        thrower.Init(list.ToArray());
        eventSystem.enabled = false;
        thrower.Throwing(delay: 3f, rotating: false, onTrowed: () =>
        {
            eventSystem.enabled = true;
            for (int i = 0; i < throwingElements.Count; i++)
                throwingElements[i].SetDefaultPosition();

            isThrow = true;
        });
    }

    private void OnDrop(WordElement121 target)
    {
        target.visible = true;
        audioPlayer.Play(1f, stampClip, () => isNext = true);
        if (!elements.Select(x => x.visible).Contains(false))
        {
            PlayCurrentSentance(() =>
            {
                audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
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
                });
            });
        }
    }

    protected virtual void PlayCurrentSentance(System.Action onOver)
    {
        audioPlayer.Play(currentSentance.clip, onOver);
    }

    private void Clear()
    {
        var targets = new List<GameObject>();
        for (int i = 0; i < sentanceParent.childCount; i++)
            targets.Add(sentanceParent.GetChild(i).gameObject);
        for (int i = 0; i < targets.Count; i++)
            Destroy(targets[i]);
        targets.Clear();
        throwingParent.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }
}
