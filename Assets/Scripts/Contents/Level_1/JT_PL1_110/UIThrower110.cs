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
    public void ClearPaths()
    {
        for (int i = 0; i < paths.Length; i++)
            Destroy(paths[i].gameObject);
        paths = new RectTransform[0];
    }
    public void SetPaths(RectTransform[] paths)
    {
        this.paths = paths;
    }
    
    public virtual void Throwing(float duration=1f,float delay = 0, bool rotating=true, TweenCallback onTrowed=null)
    {
        StartCoroutine(Throw(duration, delay, rotating, onTrowed));
    }
    IEnumerator Throw(float duration = 1f, float delay = 0, bool rotating = true, TweenCallback onTrowed = null)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        paths = paths.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        var seq = DOTween.Sequence();
        Debug.Log(paths.Length);
        for (int i = 0; i < targets.Length; i++)
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
