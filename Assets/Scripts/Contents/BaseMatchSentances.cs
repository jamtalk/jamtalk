using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseMatchSentances<TTestSetting,TSentance> : BaseContents<TTestSetting> 
    where TTestSetting : ContentsTestSetting
    where TSentance : BaseSentanceData
{
    protected override eContents contents => throw new System.NotImplementedException();
    protected override bool CheckOver() => index == sources.Length;
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
    protected TSentance[] sources { get; private set; }
    public EventSystem eventSystem;
    protected TSentance currentSentance => sources[index];
    protected virtual int QuestionCount => 4;

    bool isThrow = false;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isThrow) yield return null;
        isThrow = false;

        yield return new WaitForSecondsRealtime(1f);

        for (int j = 0; j < 3; j++)
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

        EndGuidnce();
    }
    protected virtual void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                index += 1;
                if (CheckOver())
                    ShowResult();
                else
                    ShowQuestion();
            });
        }
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();
        index = 0;
        sources = GetSentance();
        ShowQuestion();
    }

    protected override void Awake()
    {
        base.Awake();
        sources = GetSentance();
        if(showQuestionOnAwake)
            ShowQuestion();
    }
    protected virtual void ShowQuestion()
    {
        Debug.LogFormat("{0}/{1} 문제 시작", index+1, sources.Length);
        StartCoroutine(Init(currentSentance));
    }
    protected abstract TSentance[] GetSentance();

    private IEnumerator Init(TSentance data)
    {
        Clear();
        elements.Clear();
        var list = new List<RectTransform>();
        Debug.Log(string.Join(" ", data.words));
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
                        ShowResult();
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
        for (int i = 0; i < throwingParent.childCount; i++)
            targets.Add(throwingParent.GetChild(i).gameObject);

        foreach (var item in targets)
            Destroy(item);

        targets.Clear();
        throwingElements.Clear();
        UnionElements.Clear();
        throwingParent.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }
}
