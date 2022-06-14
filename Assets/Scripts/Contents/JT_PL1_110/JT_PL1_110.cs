using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_110 : BaseContents
{
    public GraphicRaycaster caster;
    private AlphabetToggle110[] toggles;
    public WordCreator110 creator;
    public UIThrower110 thrower;
    public UIMover[] mover;
    public AudioClip startClip;
    public AudioSinglePlayer audioPlayer;
    private WordsData.WordSources word;

    protected override eContents contents => eContents.JT_PL1_110;
    protected override bool CheckOver() => !toggles.Select(x => x.isOn).Contains(false);
    protected override int GetTotalScore() => 1;
    protected override void Awake()
    {
        base.Awake();
        word = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
        toggles = creator.Create(word);
        for (int i = 0; i < toggles.Length; i++)
            AddToggleListner(toggles[i]);
        var dragables = toggles.Select(x => x.drag).ToArray();

        var throwElements = toggles.Select(x => x.throwElement).ToArray();
        thrower.Init(throwElements);
        
        thrower.Throwing(2f, 3f,onTrowed:()=>
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
            if (CheckOver())
                audioPlayer.Play(word.clip, ShowResult);
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
