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
    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;

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
    protected T[] words;
    public EventSystem eventSystem;
    private T currentSentance => words[index];
    protected int questionCount => 6;

    protected override void Awake()
    {
        base.Awake();
        GetSentance();
        StartCoroutine(Init(currentSentance));
    }
    protected abstract void GetSentance();

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
        });
    }

    private void OnDrop(WordElement121 target)
    {
        target.visible = true;
        audioPlayer.Play(1f, stampClip);
        if (!elements.Select(x => x.visible).Contains(false))
        {
            audioPlayer.Play(currentSentance.clip, () =>
            {
                audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
                {
                    index += 1;
                    if (CheckOver())
                        ShowResult();
                    else
                        StartCoroutine(Init(currentSentance));
                });
            });
        }
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
