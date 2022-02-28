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
    public AudioSinglePlayer audioPlayer;
    private string word;
    protected override eContents contents => eContents.JT_PL1_110;

    protected override bool CheckOver() => !toggles.Select(x => x.isOn).Contains(false);
    private void Awake()
    {
        //word = "aasdsadasdsad";
        word = GameManager.Instance.GetWords(GameManager.Instance.currentAlphabet)
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
                audioPlayer.Play(GameManager.Instance.GetClipWord(word), ShowResult);
            else
                audioPlayer.Play(GameManager.Instance.GetClipAlphbet(toggle.value));
        };
    }
    private void AddDragListener(Dragable110 drag)
    {
        drag.onBegin += (eventData) =>
        {
            audioPlayer.Play(GameManager.Instance.GetClipPhanics(drag.value));
        };

        drag.onDrag += (eventData) =>
        {
            var rt = drag.GetComponent<RectTransform>();
            rt.position = eventData.position;
        };

        drag.onDrop += (eventData) =>
        {
            Debug.Log(drag.name + "ตๅทำ");
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
