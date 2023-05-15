using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book_Writing_Word : SingleAnswerContents<Book_Writing_WordQuestion,BookWordData>
{
    public override eSceneName NextScene => eSceneName.AC_004;
    protected override eContents contents => eContents.Book_Writing_Word;
    public Transform layout;
    public Button button;
    public UIThrower110 thrower;
    public BookWordCreator creator;
    public EventSystem eventSystem;
    public GameObject empty;
    public AlphabetToggle110[] toggles;
    public GraphicRaycaster caster;
    protected override int QuestionCount => 5;
    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(()=>PlayCorrect());
    }
    private void PlayCorrect(System.Action onOver = null)
    {
        AndroidPluginManager.Instance.PlayTTS(currentQuestion.correct.value, onOver);
    }
    protected override List<Book_Writing_WordQuestion> MakeQuestion()
    {
        return GameManager.Instance.GetCurrentBookWords()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new Book_Writing_WordQuestion(x))
            .ToList();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AddAnswer(currentQuestion.correct);
    }

    protected override void ShowQuestion(Book_Writing_WordQuestion question)
    {
        PlayCorrect();
        button.image.sprite = question.correct.sprite;
        button.image.preserveAspect = true;
        CreateLayout(question.correct.value.Replace(" ","").Length);
        toggles = creator.Create(question.correct.value);
        for (int i = 0; i < toggles.Length; i++)
        {
            AddToggleListner(toggles[i]);
            //AddDragListener(toggles[i].drag);
        }
        thrower.Init(toggles.Select(x => x.throwElement).ToArray());
        eventSystem.enabled = false;
        thrower.Throwing(1f, 1f, false, () =>
        {
            eventSystem.enabled = true;
            for (int i = 0; i < toggles.Length; i++)
                toggles[i].drag.intractable = true;
        });
    }
    private void CreateLayout(int length)
    {
        var childCount = layout.childCount;
        for (int i = 0; i < length - childCount; i++)
            Instantiate(empty, layout);

        var list = new List<RectTransform>();
        for(int i = 0;i < layout.childCount; i++)
        {
            var child = layout.GetChild(i);
            layout.GetChild(i).gameObject.SetActive(i < layout.childCount);
            if (i < length)
                list.Add(child.GetComponent<RectTransform>());
        }
        thrower.paths = list.ToArray();
    }

    private void AddToggleListner(AlphabetToggle110 toggle)
    {
        toggle.onOn += () =>
        {
            if (!toggles.Select(x => x.isOn).Contains(false))
                PlayCorrect(() => AddAnswer(currentQuestion.correct));
            else
                audioPlayer.Play(GameManager.Instance.GetResources(toggle.value).AudioData.clip);
        };
    }
    private void AddDragListener(Dragable110 drag)
    {
        drag.onBegin += (eventData) =>
        {
            audioPlayer.Play(GameManager.Instance.GetResources(drag.value).AudioData.phanics);
        };

        drag.onDrag += (eventData) =>
        {
            var rt = drag.GetComponent<RectTransform>();
            var pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.z = rt.position.z;
            rt.position = pos;
        };

        drag.onDrop += (eventData) =>
        {
            var results = new List<RaycastResult>();
            caster.Raycast(eventData, results);
            var targets = results.Select(x => x.gameObject.GetComponent<Dropable110>()).Where(x => x != null);
            if (targets.Count() > 0)
            {
                var target = targets.First();
                if (target.value == drag.value)
                {
                    target.Drop();
                    drag.gameObject.SetActive(false);
                }
            }

            drag.ResetPosition();
        };
    }
}
public class Book_Writing_WordQuestion : SingleQuestion<BookWordData>
{
    public Book_Writing_WordQuestion(BookWordData correct) : base(correct, new BookWordData[0])
    {
    }
}
