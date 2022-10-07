using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseThrowingAlphabet<T> : SingleAnswerContents<Question_ThrowerAlphabet<T>,T> where T : ResourceWordsElement
{
    public GraphicRaycaster caster;
    private AlphabetToggle110[] toggles;
    public WordCreator110 creator;
    public UIThrower110 thrower;
    public UIMover[] mover;
    public AudioClip startClip;
    protected override int QuestionCount => 2;
    protected override eGameResult GetResult() => eGameResult.Perfect;

    //protected override bool CheckOver() => !toggles.Select(x => x.isOn).Contains(false);

    private bool isNext = false;
    private bool isMove = false;
    protected IEnumerator ShowGuidnceRoutine()
    {
        ShowGuidnce();
        guideFinger.gameObject.SetActive(false);
        guideFinger.transform.localScale = new Vector3(.7f, .7f, .7f);

        while (!isMove) { yield return null; }

        for (int i = 0; i < toggles.Length; i++)
        {
            isNext = false;

            guideFinger.transform.position = toggles[i].throwElement.position;
            guideFinger.gameObject.SetActive(true);

            audioPlayer.Play(GameManager.Instance.GetResources(toggles[i].value).AudioData.phanics);
            guideFinger.DoMoveCorrect(toggles[i].throwElement, toggles[i].audioPlayer.transform.position, () =>
            {
                toggles[i].drop.Drop();
                toggles[i].throwElement.gameObject.SetActive(false);
                toggles[i].drag.ResetPosition();
                guideFinger.gameObject.SetActive(false);

                isNext = true;
            });

            while (!isNext)
            {
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1.5f);
        }

        isGuide = false;
        ShowGuidnce();
    }

    protected override void Awake()
    {
        StartCoroutine(ShowGuidnceRoutine());
        base.Awake();
    }
    protected override void ShowQuestion(Question_ThrowerAlphabet<T> question)
    {
        creator.Clear();
        toggles = creator.Create(question.correct);
        for (int i = 0; i < toggles.Length; i++)
            AddToggleListner(toggles[i]);
        var dragables = toggles.Select(x => x.drag).ToArray();

        var throwElements = toggles.Select(x => x.throwElement).ToArray();
        thrower.Init(throwElements);

        thrower.Throwing(2f, 3f, onTrowed: () =>
        {
            for (int i = 0; i < dragables.Length; i++)
                dragables[i].intractable = true;
        });
        audioPlayer.Play(5f, 3.5f, startClip);

        for (int i = 0; i < dragables.Length; i++)
            AddDragListener(dragables[i]);

        for (int i = 0; i < mover.Length; i++)
            mover[i].Move(4f, 3f, () => isMove = true);
    }

    private void AddToggleListner(AlphabetToggle110 toggle)
    {
        toggle.onOn += () =>
        {
            if (!toggles.Select(x => x.isOn).Contains(false))
                audioPlayer.Play(currentQuestion.correct.clip, () => AddAnswer(currentQuestion.correct));
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
public class Question_ThrowerAlphabet<T> : SingleQuestion<T> where T : ResourceElement
{
    public Question_ThrowerAlphabet(T correct) : base(correct, new T[] { })
    {
    }
}