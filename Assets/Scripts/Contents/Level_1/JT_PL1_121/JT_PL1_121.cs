using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_121 : BaseContents
{
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
    private int index = 0;
    [SerializeField]
    private AlphabetSentanceData[] words;
    public EventSystem eventSystem;
    private AlphabetSentanceData currentSentance => words[index];
    private int questionCount => 2;
    
    protected override eContents contents => eContents.JT_PL1_121;

    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;

    protected override void Awake()
    {
        base.Awake();
        words = GameManager.Instance.GetResources().Sentances
            .OrderBy(x=>Random.Range(0f,100f))
            .Take(questionCount)
            .ToArray();
        index = 0;
        StartCoroutine(Init(currentSentance));
    }
    private IEnumerator Init(AlphabetSentanceData data)
    {
        Clear();
        elements.Clear();
        var list = new List<RectTransform>();
        for(int i = 0;i < data.words.Length; i++)
        {
            var element = Instantiate(prefabWordElement, sentanceParent).GetComponent<WordElement121>();
            element.Init(data.words[i]);
            element.visible = false;
            elements.Add(element);
        }
        for(int i = 0;i < elements.Count; i++)
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
        for(int i = 0;i < elements.Count; i++)
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
        thrower.Throwing(delay:3f,rotating:false,onTrowed:()=>
        {
            eventSystem.enabled = true;
            for (int i = 0; i < throwingElements.Count; i++)
                throwingElements[i].SetDefaultPosition();
        });
    }

    private void OnDrop(WordElement121 target)
    {
        target.visible = true;
        if (!elements.Select(x => x.visible).Contains(false))
        {
            index += 1;
            audioPlayer.Play(1f,GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                if (CheckOver())
                    ShowResult();
                else
                    StartCoroutine(Init(currentSentance));
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
