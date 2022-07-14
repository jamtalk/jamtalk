using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_106 : BaseContents
{
    protected override eContents contents => eContents.JT_PL2_106;
    protected override bool CheckOver() => index == QuestionCount;
    protected override int GetTotalScore() => QuestionCount;
    protected  int QuestionCount => 3;
    private int index = 0;

    private DigraphsWordsData currentDigraphs;

    public Thrower306 thrower;
    public Text[] texts;
    public Text currentText;
    public Button currentButton;
    public Image currentImage;
    public Image bagImage;
    [SerializeField]
    private List<DoubleClick306> elements = new List<DoubleClick306>();
    private eDigraphs[] eDig = { eDigraphs.CH, eDigraphs.SH, eDigraphs.TH };

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }

    private void SetCurrentImage()
    {
        currentText.text = currentDigraphs.key;
        currentImage.sprite = currentDigraphs.sprite;
        currentImage.name = currentDigraphs.key;
        currentImage.preserveAspect = true;
        currentImage.gameObject.SetActive(true);

        for (int i = 0; i < elements.Count; i++)
            elements[i].gameObject.SetActive(true);

        thrower.gameObject.SetActive(false);
        bagImage.gameObject.SetActive(false);
    }

    private void SetBagImage()
    {
        bagImage.sprite = currentImage.sprite;
        bagImage.preserveAspect = true;
        bagImage.gameObject.SetActive(true);
    }

    protected void MakeQuestion()
    {
        currentDigraphs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        var value = currentDigraphs.key;
        currentText.text = value.Replace(
            GameManager.Instance.currentDigrpahs.ToString().ToLower(), "__");
        currentImage.sprite = currentDigraphs.sprite;
        currentImage.name = currentDigraphs.key;
        currentImage.preserveAspect = true;

        ShowQuestion();
    }

    protected void ShowQuestion()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].Init(eDig[i]);    
            elements[i].isOn = false;     
            AddDoubleClickListener(elements[i], currentDigraphs); // current 가 아닌 알맞는 digraphs 삽입 
        }
    }

    protected virtual void AddDoubleClickListener(DoubleClick306 element, DigraphsWordsData data)
    {
        element.onClickFirst.RemoveAllListeners();
        element.onClick.RemoveAllListeners();

        var clip = GameManager.Instance.schema.GetDigrpahsAudio(element.eDigraphs);

        element.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(clip.phanics);
        });

        element.onClick.AddListener(() =>
        {
            if (currentDigraphs.key.Contains(element.name))
            {
                index += 1;

                for (int i = 0; i < elements.Count; i++)
                    elements[i].gameObject.SetActive(false);
                currentImage.gameObject.SetActive(false);

                thrower.GetComponent<Image>().sprite = currentImage.sprite;
                thrower.gameObject.SetActive(true);
                thrower.Throw(currentImage, bagImage.GetComponent<RectTransform>(), () =>
                {
                    currentText.text = data.key;
                    SetBagImage();

                    audioPlayer.Play(currentDigraphs.act, () =>
                    {
                        SetCurrentImage();
                        if (CheckOver())
                            ShowResult();
                        else
                            MakeQuestion();
                    });
                });
            }
            else
            {
                audioPlayer.Play(clip.phanics);
            }
        });
    }
}
