using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookWrittingContents_Words : BaseBookContentsRunner<BookWordData>
{
    public UIThrower110 thrower;
    public BookWordCreator creator;
    public RectTransform throwParent;
    public Image questionImage;
    public AudioSinglePlayer audioPlayer;
    public GraphicRaycaster caster;
    AlphabetToggle110[] toggles;
    private void Awake()
    {
        var targetWord = GameManager.Instance.GetCurrentBookWords().OrderBy(x => x.value).First();
        ShowQuestions(targetWord);
    }
    public override void ShowQuestions(BookWordData data)
    {
        questionImage.sprite = data.sprite;
        questionImage.preserveAspect = true;

        creator.Clear();
        thrower.ClearPaths();
        var list = new List<RectTransform>();
        for(int i = 0;i < data.value.Replace(" ","").Length; i++)
        {
            var item = new GameObject("path", typeof(RectTransform)).GetComponent<RectTransform>();
            item.SetParent(throwParent);
            list.Add(item);
        }
        thrower.SetPaths(list.ToArray());

        toggles = creator.Create(data.value);
        for (int i = 0; i < toggles.Length; i++)
        {
            AddToggleListner(toggles[i]);
            AddDragListener(toggles[i].drag);
        }
        thrower.Init(toggles.Select(x => x.throwElement).ToArray());
        thrower.Throwing(onTrowed:()=> 
        {
            for (int i = 0; i < toggles.Length; i++)
                toggles[i].drag.intractable = true;
        });
    }
    private void AddToggleListner(BaseDragableToggle<eAlphabet> toggle)
    {
        toggle.onOn += () =>
        {
            //if (!toggles.Select(x => x.isOn).Contains(false))
            //    Debug.Log("Á¤´ä!");
            //else
            //    audioPlayer.Play(GameManager.Instance.GetResources(toggle.value).AudioData.clip);
        };
    }
    private void AddDragListener(BaseDragable<eAlphabet> drag)
    {
        drag.onBegin += (eventData) =>
        {
            //audioPlayer.Play(GameManager.Instance.GetResources(drag.value).AudioData.phanics);
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
