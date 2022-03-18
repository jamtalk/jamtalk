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
    public GameObject baseWord;
    public List<SiteWord121> elements = new List<SiteWord121>();
    public Sprite[] siteWords;
    public UIThrower110 thrower;
    private int index = 0;
    [SerializeField]
    private string[] words;
    public EventSystem eventSystem;
    public AudioSinglePlayer audioPlayer;
    private string currentSentance => words[index];
    private int questionCount => 2;

    protected override eContents contents => eContents.JT_PL1_121;

    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;

    protected override void Awake()
    {
        base.Awake();
        words = GameManager.Instance.GetSentances(GameManager.Instance.currentAlphabet)
            .OrderBy(x=>Random.Range(0f,100f))
            .Take(questionCount)
            .ToArray();
        index = 0;
        Init(currentSentance);
        Debug.Log(GameManager.Instance.GetSentances().Select(x => x.Split(' ').Length).Max());
    }
    private void Init(string sentance)
    {
        Clear();
        elements.Clear();
        var words = sentance.Split(' ');
        var list = new List<RectTransform>();
        for(int i = 0;i < words.Length; i++)
        {
            var sprite = siteWords.ToList().Find(x => x.name.ToLower() == words[i].ToLower());
            var component = Instantiate(baseWord, sentanceParent).GetComponent<SiteWord121>();
            component.onCorrect += OnCorrect;
            component.Init(sprite==null? siteWords[0]:sprite);
            elements.Add(component);
            list.Add(component.throwingElement);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(sentanceParent);
        thrower.Init(list.ToArray());
        eventSystem.enabled = false;
        thrower.Throwing(delay:3f,rotating:false,onTrowed:()=>
        {
            eventSystem.enabled = true;
        });
    }
    private void Clear()
    {
        var targets = new List<GameObject>();
        for (int i = 0; i < sentanceParent.childCount; i++)
            targets.Add(sentanceParent.GetChild(i).gameObject);
        for (int i = 0; i < targets.Count; i++)
            Destroy(targets[i]);
        targets.Clear();
    }
    private void OnCorrect(string word)
    {
        audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
        {
            if (!elements.Select(x => x.isCorrect).Contains(false))
            {
                index += 1;
                if (CheckOver())
                    ShowResult();
                else
                    Init(currentSentance);
            }
        });
    }
}
