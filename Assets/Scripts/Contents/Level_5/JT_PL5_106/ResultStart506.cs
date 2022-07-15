using DG.Tweening;
using GJGameLibrary.Util.Bezier;
using GJGameLibrary.Util.Bezier.DoTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResultStart506 : MonoBehaviour
{
    public Text value;
    public RectTransform star;
    public RectTransform tail;
    public RectTransform[] paths;
    private void Awake()
    {
        Show("Test",1f);
    }
    public Sequence Show(string value,float duration)
    {
        this.value.text = value;
        this.value.gameObject.SetActive(false);

        var seq = BezierTween.Curve(star, duration, 10, paths.Select(x=>x.position).ToArray());
        seq.Insert(0, BezierTween.Curve(tail, duration, 10, paths.Select(x => x.position).ToArray()));


        star.rotation = Quaternion.Euler(0, 0, 0);
        var rotTween = star.DORotate(new Vector3(0, 0, 720f), duration, RotateMode.FastBeyond360);
        seq.Insert(0, rotTween);

        star.localScale = Vector3.one * .25f;
        var scaleTween = star.DOScale(1f, duration);
        seq.Insert(0, scaleTween);

        seq.onComplete += () =>
        {
            this.value.gameObject.SetActive(true);
        };

        return seq;
    }
}
