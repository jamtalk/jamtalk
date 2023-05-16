using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookWrittingContents_Words : BaseBookContentsRunner<BookWordData>
{
    public UIThrower110 thrower;
    public BookWordCreator creator;
    private void Awake()
    {
        var targetWord = GameManager.Instance.GetCurrentBookWords().OrderBy(x => x.value).First();
        Debug.LogFormat("선택한 단어 : ({0})", targetWord.value);
        ShowQuestions(targetWord);
    }
    public override void ShowQuestions(BookWordData data)
    {
        creator.Clear();
        var elements = creator.Create(data.value);
        thrower.Init(elements.Select(x => x.transform as RectTransform).ToArray());
        thrower.Throwing();
    }
}
