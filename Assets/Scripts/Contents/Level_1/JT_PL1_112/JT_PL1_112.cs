using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class JT_PL1_112 : BaseContents
{
    public DropToggle[] toggles;
    public DragAlphabet[] drags;
    protected override eContents contents => eContents.JT_PL1_112;

    protected override bool CheckOver() => !toggles.Select(x => x.isOn).Contains(false);
    protected override int GetTotalScore() => toggles.Length;
    eAlphabet[] corrects;

    bool isStop = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        MakeQuestion();

        yield return new WaitForEndOfFrame();


        //for (int i = 0; i < corrects.Length; i++)
        //{
        var i = 0;
        isStop = false;
        var guideCorrect = toggles.Where(x => x.alphabet == corrects[i]).First();
        var drag = drags.Where(x => x.alphabet == guideCorrect.alphabet).First();
        guideFinger.gameObject.SetActive(true);

        guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
        guideFinger.DoMove(drag.transform.position, () =>
        {
            guideFinger.DoPress(() =>
            {
                guideFinger.DoMove(drag.gameObject, guideCorrect.transform.position, () =>
                {
                    guideCorrect.isOn = true;

                    guideFinger.gameObject.SetActive(false);
                    drag.gameObject.SetActive(false);

                    CorrectMotion(guideCorrect);
                });
            });
        });


        while (!isStop) yield return null;
        //}

        EndGuidnce();
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        foreach (var item in toggles)
            item.isOn = false;
        ResetLayout();
        MakeQuestion();
    }

    protected override void Awake()
    {
        base.Awake();
        //MakeQuestion();
    }

    private void MakeQuestion()
    {
        corrects = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .OrderBy(x => x)
            .Take(toggles.Length)
            .ToArray();
        var incorrects = GameManager.Instance.alphabets
            .Where(x => !corrects.Contains(x))
            .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
            .Take(drags.Length - toggles.Length)
            .Union(corrects)
            .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
            .ToArray();
        for (int i = 0; i < toggles.Length; i++)
        {
            AddToggleDropListener(toggles[i]);
            toggles[i].Init(corrects[i], false);
        }
        for (int i = 0; i < drags.Length; i++)
            drags[i].Init(incorrects[i]);
    }

    private void AddToggleDropListener(DropToggle toggle)
    {
        toggle.onDroped += () =>
        {
            CorrectMotion(toggle);
        };
    }

    private void CorrectMotion(DropToggle toggle, Action action = null)
    {
        for (int i = 0; i < drags.Length; i++)
            drags[i].intracable = false;
        audioPlayer.Play(GameManager.Instance.GetResources(toggle.alphabet).AudioData.act2, () =>
        {
            if (CheckOver())
                ShowResult();
            else
            {
                for (int i = 0; i < drags.Length; i++)
                    drags[i].intracable = true;
                isStop = true;
            }
        });
    }

    private void ResetLayout()
    {
        foreach (var item in drags)
        {
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            item.gameObject.SetActive(true);
        }
    }
}
