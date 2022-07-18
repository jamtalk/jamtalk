using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_110 : SingleAnswerContents<Question_PL1_110,AlphabetWordsData>
{
    public GraphicRaycaster caster;
    private AlphabetToggle110[] toggles;
    public WordCreator110 creator;
    public UIThrower110 thrower;
    public UIMover[] mover;
    public AudioClip startClip;

    protected override int QuestionCount => 2;
    protected override eGameResult GetResult() => eGameResult.Perfect;
    protected override eContents contents => eContents.JT_PL1_110;
    //protected override bool CheckOver() => !toggles.Select(x => x.isOn).Contains(false);

    protected override List<Question_PL1_110> MakeQuestion()
    {
        return new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 }
            .SelectMany(x => GameManager.Instance.GetResources(x).Words.OrderBy(x => Random.Range(0f, 100f)).Take(QuestionCount/2))
            .Select(x => new Question_PL1_110(x))
            .OrderBy(x=>Random.Range(0f,100f))
            .ToList();
    }

    protected override void ShowQuestion(Question_PL1_110 question)
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
            mover[i].Move(4f, 3f);
    }

    private void AddToggleListner(AlphabetToggle110 toggle)
    {
        toggle.onOn += () =>
        {
            if (!toggles.Select(x=>x.isOn).Contains(false))
                audioPlayer.Play(currentQuestion.correct.clip, ()=>AddAnswer(currentQuestion.correct));
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
            rt.position = eventData.position;
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
public class Question_PL1_110 : SingleQuestion<AlphabetWordsData>
{
    public Question_PL1_110(AlphabetWordsData correct) : base(correct, new AlphabetWordsData[] { })
    {
    }
}