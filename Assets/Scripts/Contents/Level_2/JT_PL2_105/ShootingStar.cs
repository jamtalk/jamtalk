using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    public float minTurm = 5;
    public float maxTurm = 10;
    public RectTransform start;
    public RectTransform end;
    public RectTransform target;
    public Sequence seq;
    private void Awake()
    {
        target.position = start.position;
        target.gameObject.SetActive(true);
        seq = DOTween.Sequence();
        var tween = target.DOMove(end.position, 1f);
        tween.SetDelay(Random.Range(minTurm, maxTurm));
        seq.Append(tween);
        seq.SetLoops(-1);
    }
    private void OnDestroy()
    {
        if(seq != null)
            seq.Kill();

        seq = null;
    }
}
