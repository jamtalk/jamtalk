using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL2_102 : STTContents<VowelWordsData, Image>
{
    protected override int QuestionCount => 3;
    protected override eContents contents => eContents.JT_PL2_102;
    protected override bool PlayClipOnShow => false;

    public ShowBoard_PL2_102 board;
    public GameObject QuestionObject;
    protected override void ShowValue(Question_STT<VowelWordsData> question)
    {
        QuestionObject.gameObject.SetActive(false);
        valueViewer.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Lower, question.correct.Vowel);
        board.Show(question.correct, () => QuestionObject.gameObject.SetActive(true));
    }
    protected override List<Question_STT<VowelWordsData>> MakeQuestion()
    {
        return GameManager.Instance.GetResources().Vowels
            .Where(x => x.VowelType == eVowelType.Short)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new Question_STT<VowelWordsData>(x))
            .ToList();
    }
}