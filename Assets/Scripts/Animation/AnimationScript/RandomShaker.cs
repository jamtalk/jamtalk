using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RandomShaker : AnimationScript
{
    [SerializeField]
    [Range(1f, 10f)]
    private float randomRange;
    [SerializeField]
    [Range(1f, 90f)]
    private float angle;
    Coroutine shaking;
    private Quaternion current;
    protected override void Awake()
    {
        base.Awake();
        current = transform.rotation;
    }
    public override void Play()
    {
        StartCoroutine(Shaking());
    }

    public override void Stop()
    {
        if (shaking != null)
            StopCoroutine(shaking);
    }
    IEnumerator Shaking()
    {
        while (true)
        {
            var delay = Random.Range(0f, randomRange);
            yield return new WaitForSecondsRealtime(delay);
            if (!gameObject.activeSelf)
                continue;
            var seq = DOTween.Sequence();
            var startTween = transform.DORotate(new Vector3(0, 0, angle),duration/4);
            startTween.SetEase(Ease.Linear);
            startTween.SetLoops(2, LoopType.Yoyo);

            var endTween = transform.DORotate(new Vector3(0, 0, -angle), duration / 4);
            endTween.SetEase(Ease.Linear);
            endTween.SetLoops(2, LoopType.Yoyo);
            seq.Append(endTween);

            bool isSeq = false;
            seq.onComplete += () => isSeq = true;
            seq.onKill += () => transform.rotation = current;
            seq.Play();
            while (!isSeq) { yield return null; }

        }
    }
}
