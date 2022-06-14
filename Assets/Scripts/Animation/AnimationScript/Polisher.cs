using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Polisher : AnimationScript
{
    [SerializeField]
    [Range(1f,10f)]
    private float delay;
    private Coroutine polishing;
    [SerializeField]
    [Range(1f, 100f)]
    private float width =100;
    [SerializeField]
    private Image image => GetComponent<Image>();
    private RectTransform rt => GetComponent<RectTransform>();
    public override void Play()
    {
        polishing = StartCoroutine(Polishing());
    }
    public override void Stop()
    {
        if (polishing != null)
            StopCoroutine(polishing);
    }
    private IEnumerator Polishing()
    {
        yield return new WaitForSecondsRealtime(delay);
        transform.eulerAngles = new Vector3(0, 0, -45f);
        var size = transform.parent.GetComponent<RectTransform>().rect.size;
        rt.sizeDelta = new Vector2(width, GetDistance(size.x, size.y));
        size /= 2f;
        size += Vector2.one * width;
        var startPos = new Vector2(size.x,-size.y);
        var endPos = new Vector2(-size.x, size.y); 
        rt.localPosition = startPos;
        while (true)
        {
            var tween = rt.DOAnchorPos(endPos, duration);
            var isCompleted = false;
            tween.onComplete += () => rt.localPosition = startPos;
            tween.onKill += () => isCompleted = true;
            tween.Play();
            while (!isCompleted) { yield return null; }
            yield return new WaitForSecondsRealtime(delay);
        }
    }
    private float GetDistance(float width, float height) => Mathf.Sqrt(Mathf.Pow(width, 2f) + Mathf.Pow(height, 2f));
}
