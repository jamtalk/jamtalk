using System.Linq;
using UnityEngine;
using DG.Tweening;

public class UIMover : MonoBehaviour
{
    public RectTransform rt => GetComponent<RectTransform>();
    public RectTransform[] paths;
    public void Move(float duration, float delay = 0,TweenCallback onCompleted=null)
    {
        var seq = DOTween.Sequence();
        seq.Insert(delay,rt.DOPath(paths.Select(x => x.position).ToArray(), duration));
        seq.Play();
    }
}
