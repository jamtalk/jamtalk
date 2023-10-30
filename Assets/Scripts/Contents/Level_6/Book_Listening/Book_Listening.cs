using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using DG.Tweening;
using static UnityEngine.Networking.UnityWebRequest;
using UnityEngine.AddressableAssets;

public class Book_Listening : BaseContents<BookContentsSetting>
{
    protected override eContents contents => eContents.Book_Listening;
    protected override bool CheckOver() => GetTotalScore() == GetCorrectScore();
    protected override int GetTotalScore() => data.Length;
    protected override int GetCorrectScore() => index;
    protected override bool isGuidence => false;

    public AudioSinglePlayer player;
    public Image screen;
    public Text caption;
    public Text typingText;
    public BookSentanceData[] data;
    public Button buttonNext;
    public Button buttonPrevious;
    public Button buttonReplay;
    public Button buttonPlay;

    private int index = 0;
    protected override void Awake()
    {
        base.Awake();
        base.OnAwake();
        data = GameManager.Instance.GetCurrentBooks()
            .SelectMany(x => x.sentances)
            .OrderBy(x => x.priority)
            //.Where(x=>!string.IsNullOrEmpty(x.clip))
            .ToArray();

        for (int i = 0; i < data.Length; i++)
        {
            SceneLoadingPopup.SpriteLoader.Add(data[i].spriteAsync);
            if(!string.IsNullOrEmpty(data[i].clip))
                SceneLoadingPopup.SpriteLoader.Add(Addressables.LoadAssetAsync<AudioClip>(data[i].clip));
        }
    }
    protected override void OnAwake()
    {
        buttonNext.onClick.AddListener(() => Show(index + 1));
        buttonPrevious.onClick.AddListener(() => Show(index - 1));
        buttonPlay.onClick.AddListener(() => Show(index));
        buttonReplay.onClick.AddListener(() => Show(0));
        Show(0);
    }

    private void Show(int index)
    {
        if (index < 0)
            index = 0;
        this.index = index;

        if (index >= data.Length)
        {
            ShowResult();
            return;
        }

        var currentData = this.data[index];
        if (string.IsNullOrEmpty(currentData.clip))
        {
            AndroidPluginManager.Instance.PlayTTS(currentData.value);
        }
        else
            player.Play(currentData.clip);
        screen.sprite = currentData.sprite;
        caption.text = currentData.en;
    }

    private string[] richColor = {
        "aqua",
        "brown",
        "green",
        "grey",
        "lightblue",
        "lime",
        "magenta",
        "maroon",
        "navy",
        "olive",
        "orange",
        "purple",
        "red",
        "yellow",
    };
}
