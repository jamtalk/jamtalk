using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Polish : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private RectTransform rt;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float delay;
    private Coroutine polishing;
    private void Awake()
    {
        polishing = StartCoroutine(Polishing());
    }
    private void OnDestroy()
    {
        StopCoroutine(polishing);
    }
    private IEnumerator Polishing()
    {
        yield return new WaitForSecondsRealtime(delay);
        var size = transform.parent.GetComponent<RectTransform>().rect.size;
        rt.sizeDelta = new Vector2(100, GetDistance(size.x, size.y));
        size /= 2f;
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
