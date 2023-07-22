using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public abstract class BaseThrowingAlphabet<TTestSetting,TElement> : SingleAnswerContents<TTestSetting,Question_ThrowerAlphabet<TElement>,TElement> 
    where TTestSetting : ContentsTestSetting
    where TElement : ResourceWordsElement
{
    public GraphicRaycaster caster;
    private AlphabetToggle110[] toggles;
    public WordCreator110 creator;
    public UIMoverControler moverControler;
    public UIThrower110 thrower;
    public AudioClip startClip;
    protected override int QuestionCount => 2;
    protected override eGameResult GetResult() => eGameResult.Success;

    //protected override bool CheckOver() => !toggles.Select(x => x.isOn).Contains(false);
    private bool isMove = false;
    protected override void Awake()
    {
        base.Awake();
        for(int i=0;i < questions.Count; i++)
            SceneLoadingPopup.SpriteLoader.Add(Addressables.LoadAssetAsync<AudioClip>(questions[i].correct.clip));

        var alphabets = questions
            .SelectMany(x => x.correct.key.Replace(" ", "").ToUpper())
            .Select(x => (eAlphabet)System.Enum.Parse(typeof(eAlphabet), x.ToString()))
            .Distinct()
            .Select(x=> GameManager.Instance.GetResources(x).AudioData.phanics)
            .ToArray();

        for (int i = 0; i < alphabets.Length; i++)
            SceneLoadingPopup.SpriteLoader.Add(Addressables.LoadAssetAsync<AudioClip>(alphabets[i]));
    }
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return new WaitForSecondsRealtime(10f);

        var i = 0;
        isNext = false;

        guideFinger.gameObject.SetActive(true);
        guideFinger.DoMove(toggles[i].throwElement.position, () =>
        {
            guideFinger.DoPress(() =>
            {
                audioPlayer.Play(GameManager.Instance.GetResources(toggles[i].value).AudioData.phanics);
                guideFinger.DoMove(toggles[i].throwElement.gameObject, toggles[i].audioPlayer.transform.position, () =>
                {
                    toggles[i].drop.Drop();
                    toggles[i].throwElement.gameObject.SetActive(false);
                    toggles[i].drag.ResetPosition();
                    guideFinger.gameObject.SetActive(false);

                    isNext = true;
                });
            });
        });

        while (!isNext)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1.5f);

        EndGuidnce();
    }

    protected override void EndGuidnce()
    {
        moverControler.Stop();
        audioPlayer.Stop();
        base.EndGuidnce();

        currentQuestionIndex = 0;
        ShowQuestion(questions[currentQuestionIndex]);
    }
    protected override void ShowQuestion(Question_ThrowerAlphabet<TElement> question)
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

        moverControler.Init();
    }

    private void AddToggleListner(BaseDragableToggle<eAlphabet> toggle)
    {
        toggle.onOn += () =>
        {
            if (!toggles.Select(x => x.isOn).Contains(false))
                audioPlayer.Play(currentQuestion.correct.clip, () => AddAnswer(currentQuestion.correct));
            else
                audioPlayer.Play(GameManager.Instance.GetResources(toggle.value).AudioData.clip);
        };
    }
    private void AddDragListener(BaseDragable<eAlphabet> drag)
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
                else
                    audioPlayer.PlayIncorrect();
            }
            else
                audioPlayer.PlayIncorrect();

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