using System.Linq;
using UnityEngine;
using DG.Tweening;
using GJGameLibrary.Util.Bezier;
using GJGameLibrary.Util.Bezier.DoTween;

public class UIMover : MonoBehaviour
{
    public  RectTransform rt => GetComponent<RectTransform>();
    public  RectTransform[] paths;
    public void Move(float duration, float delay = 0,TweenCallback onCompleted=null)
    {
        var seq = BezierTween.Curve(rt, duration, 50, paths.Select(x => x.position).ToArray());
        seq.SetDelay(delay);
        seq.onComplete += onCompleted;
        seq.Play();
    }
}
