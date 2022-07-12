using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL5_107 : JT_PL3_107
{
    protected override eContents contents => eContents.JT_PL5_107;

    protected override void GetWords()
    {
        words = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == eDig[digraphsIndex])
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(drops.Length)
            .ToArray();
        SetElement(words);
    }
}
