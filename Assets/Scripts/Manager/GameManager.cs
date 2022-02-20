using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GJGameLibrary.DesignPattern;

public class GameManager : MonoSingleton<GameManager>
{
    public eAlphabet currentAlphabet { get; private set; }
    public eContents currentContents { get; private set; }
    public override void Initialize()
    {
        base.Initialize();
        currentAlphabet = eAlphabet.A;
        currentContents = eContents.JT_PL1_102;
    }
}
