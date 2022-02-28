using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIThrower110 : MonoBehaviour
{
    public RectTransform[] targets;
    public RectTransform[] paths;
    public void Init(RectTransform[] targets)
    {
        this.targets = targets;
    }
    public void Throwing(float duration=1f,float delay = 0, bool rotating=true, TweenCallback onTrowed=null)
    {
        paths = paths.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        var seq = DOTween.Sequence();
        for(int i = 0;i < targets.Length; i++)
        {
            var tween = MakeTween(targets[i], paths[i], duration);
            seq.Insert(delay, tween);
            if (rotating)
            {
                var angle = Random.Range(-45f, 45f);
                var rotateTween = targets[i].DORotate(new Vector3(0, 0, angle), duration);
                rotateTween.SetEase(Ease.Linear);
                seq.Insert(delay, rotateTween);
            }
        }
        seq.onComplete += onTrowed;
        seq.Play();
    }
    private Tween MakeTween(RectTransform target, RectTransform path, float duration)
    {
        var tween = target.DOMove(path.position, duration);
        tween.SetEase(Ease.Linear);
        return tween;
    }
}
