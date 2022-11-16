using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FishingElement : MonoBehaviour
{
    public RectTransform bobberRt;
    public RectTransform bobberBottomRt;
    public RectTransform rodRt;

    public Image imageArm;
    public Image imageBobber;
    public Image imageLine;
    public RectTransform line_rt => imageLine.GetComponent<RectTransform>();

    private float pullTime = 2f;
    private float throwTime = 1f;
    public RectTransform target;
    public void Awake()
    {
        
    }
    private void Update()
    {
        var pos = Camera.main.WorldToScreenPoint(bobberRt.position);
        pos.y += -15f;
        SetLine(pos);
    }

    private void SetLine(Vector2 position)
    {
        line_rt.gameObject.SetActive(true);

        var v1 = position - (Vector2)Camera.main.WorldToScreenPoint(line_rt.position);
        var angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg + 90;//- 270f;
        line_rt.rotation = Quaternion.Euler(0, 0, angle);

        var size = line_rt.sizeDelta;
        size.y = v1.magnitude;
        line_rt.sizeDelta = size;
    }

    public void ThrowBobber(GameObject target, TweenCallback callback = null)
    {
        Sequence seq = DOTween.Sequence();

        Tween tweenPull = imageArm.transform.DORotate(new Vector3(0, 0, 30), pullTime);
        Tween tweenThrow = imageArm.transform.DORotate(new Vector3(0, 0, 0), throwTime);
        Tween tweenBobber = imageBobber.transform.DOMove(target.transform.position, 1f);

        tweenPull.SetEase(Ease.Linear);
        tweenThrow.SetEase(Ease.Linear);

        seq.Append(tweenPull);
        seq.Append(tweenThrow);
        seq.Insert(0, imageBobber.transform.DORotate(new Vector3(0, 0, 180), pullTime));

        seq.Insert(pullTime + throwTime,  imageBobber.transform.DORotate(new Vector3(0, 0, 0), throwTime));
        seq.Insert(pullTime + throwTime, tweenBobber);

        seq.Append(imageBobber.transform.DOMove(rodRt.transform.position, 1f));
        seq.Insert(pullTime + throwTime + 1f,
            target.transform.DOMove(bobberBottomRt.transform.position, 1f));
        seq.Insert(pullTime + throwTime + 1f,
            target.transform.DOScale(new Vector3(0, 0, 0), 1f));

        seq.onComplete += callback;

        seq.Play();
    }

    public void SetLine()
    {

    }
}
