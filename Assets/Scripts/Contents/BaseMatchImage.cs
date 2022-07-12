using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMatchImage<T> : BaseContents
    where T : DataSource
{
    public CanvasScaler scaler;
    public DropSpaceShip_107[] drops;
    public DragKnob_107[] drags;
    protected T[] words;

    protected override void Awake()
    {
        base.Awake();
        GetWords();
    }

    protected abstract void GetWords();


    protected abstract void PlayAudio(DataSource word);

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
            ShowResult();
        else
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
    }

}