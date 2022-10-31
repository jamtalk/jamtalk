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
    public CanvasScaler scaler;
    public DropSpaceShip_107[] drops;
    public DragKnob_107[] drags;
    protected T[] words;

    [Header("Guide")]
    public DropSpaceShip_107[] guideDrops;
    public DragKnob_107[] guideDrags;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for(int i = 0; i < drops.Length; i++)
        {
            guideDrops[i].Init(drops[i].data);
            guideDrags[i].Init(drags[i].data);
        }

        var dropTarget = guideDrops.Where(x => !x.isConnected).OrderBy(x => Random.Range(0, 100)).First();
        var dragTarget = guideDrags.Where(x => x.data.key == dropTarget.data.key).First();

        yield return new WaitForEndOfFrame();

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
                    onDrop();
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

    protected abstract void GetWords();


    protected abstract void PlayAudio(ResourceWordsElement word);

    protected override eContents contents => throw new System.NotImplementedException();
    protected override int GetTotalScore() => drops.Length;
    protected override bool CheckOver() => !drops.Select(x => x.isConnected).Contains(false);

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
        if (CheckOver())
        {
           ShowResult();
        }
        else
        {
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                if (isGuide)
                {
                    guidePopup.gameObject.SetActive(false);
                    isGuide = false;

                    foreach (var item in drags)
                    {
                        item.Reset();
                    }

                    foreach (var item in drops)
                    {
                        item.lineImage.fillAmount = 1;
                        item.Reset();
                    }

                    GetWords();
                }

            });
        }
    }

}