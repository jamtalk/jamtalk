using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class JT_PL1_117 : BingoContents<AlphabetData, BingoButton, Image, BingoBoard>
{
    protected override eContents contents => eContents.JT_PL1_117;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        //for (int i = 0; i < board.size; i++)
        //{
        //    var isNext = false;
        //    var target = board.buttons.Where(x => x.value.Alphabet == currentQuestion.Alphabet).First();

        //    guideFinger.gameObject.SetActive(true);
        //    guideFinger.DoMoveCorrect(target.transform.position, () =>
        //    {
        //        guideFinger.DoClick(() =>
        //        {
        //            isNext = true;
        //            currentIndex += 1;
        //            scoreBoard.AddScore(100);
        //            target.GuideClick();
        //        });
        //    });
        //    while (!isNext) yield return null;
        //    yield return new WaitForSecondsRealtime(1.5f);
        //}
    }
    protected override AlphabetData[] correctsTarget
    {
        get
        {
            if(_correctsTarget == null)
            {
                var target = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
                if (GameManager.Instance.currentAlphabet >= eAlphabet.C)
                {
                    Debug.Log("if");
                    var privious = GameManager.Instance.alphabets.Where(x => x < GameManager.Instance.currentAlphabet)
                        .OrderBy(x => Random.Range(0f, 100f))
                        .Take(board.size - 2);
                    target = target.Union(privious).ToArray();
                }

                _correctsTarget = target
                    .Select(x => GameManager.Instance.GetResources(x))
                    .ToArray();
            }

            return _correctsTarget;
        }
    }

    public override AlphabetData[] GetQuestionType()
    {
        return GameManager.Instance.alphabets
            .Where(x => !correctsTarget.Select(y=>y.Alphabet).Contains(x))
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .Select(x=>GameManager.Instance.GetResources(x))
            .ToArray();
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.AudioData.clip);

    protected override bool IsCurrentAnswer(AlphabetData value) => value.Alphabet == currentQuestion.Alphabet;
}
