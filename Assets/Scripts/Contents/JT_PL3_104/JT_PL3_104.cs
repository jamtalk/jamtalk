using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_104 : JT_PL2_104
{
    protected override eContents contents => eContents.JT_PL3_104;
    public GameObject effect;
    public AudioClip effectClip;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override List<Question2_104> MakeQuestion()
    {
        return base.MakeQuestion();
    }

    protected override void ThrowElement(BubbleElement bubble, VowelData.VowelSource data)
    {
        thrower.Throw(bubble, textPot.GetComponent<RectTransform>(), () =>
        {
            effect.gameObject.SetActive(true);
            audioPlayer.Play(1f, effectClip, () =>
            {
                AddAnswer(data);
                effect.gameObject.SetActive(false);
            });
        });
    }
}
