using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class BaseMatchImage<T> : BaseContents
    where T : ResourceWordsElement
{
    protected abstract void GetWords();
    protected abstract void PlayAudio(ResourceWordsElement word);
    protected override eContents contents => throw new System.NotImplementedException();
    protected override int GetTotalScore() => drops.Length;
    protected override bool CheckOver() => questionCounts == index;
    protected virtual bool DropCompleted() => !drops.Select(x => x.isConnected).Contains(false);
    protected virtual int questionCounts => 4;
    protected int index = 0;


    public CanvasScaler scaler;
    public DropSpaceShip_107[] drops;
    public DragKnob_107[] drags;
    protected T[] words;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();


        var dropTarget = drops.Where(x => !x.isConnected).OrderBy(x => Random.Range(0, 100)).First();
        var dragTarget = drags.Where(x => x.data.key == dropTarget.data.key).First();

        guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
        guideFinger.DoMove(dropTarget.pointKnob.transform.position, () =>
        {
            guideFinger.DoPress(() =>
            {
                PlayAudio(dropTarget.data);
                dropTarget.SetGuideLine(1f, dragTarget);

                guideFinger.DoMove(1f, dragTarget.pointKnob.transform.position, () =>
                {
                    dropTarget.SetGuideCover(dragTarget);
                    guideFinger.gameObject.SetActive(false);
                    isNext = true;
                });
            });
        });

        while (!isNext) yield return null;
        isNext = false;
    }
    protected override void Awake()
    {
        base.Awake();
        GetWords();
    }

    protected virtual void SetElement(T[] item)
    {
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);

        drops = drops.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        drags = drags.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < words.Length; i++)
        {
            drops[i].Init(words[i]);
            drags[i].Init(words[i]);

            drops[i].onClick += PlayAudio;
            drags[i].onClick += PlayAudio;

            drags[i].onDrop += () =>
            {
                onDrop();
                Debug.Log(drags[i].name);
            };
            drops[i].onDrop += () =>
            {
                onDrop();
                Debug.Log(drops[i].name);
            };
        }
    }

    protected virtual void onDrop()
    {
        if(DropCompleted())
        {
            index++;

            if (CheckOver())
                ShowResult();
            else
                ResetElement();
        }
        else
        {
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                if (isGuide) EndGuidnce();
            });
        }
    }

    protected virtual void ResetElement()
    {
        foreach (var item in drags)
            item.Reset();

        foreach (var item in drops)
        {
            item.lineImage.fillAmount = 1f;
            item.Reset();
        }

        GetWords();
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        ResetElement();
    }
}