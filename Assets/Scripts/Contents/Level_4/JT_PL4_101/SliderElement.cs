using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderElement : MonoBehaviour
{
    public Slider slider;
    public Button button;
    public Text text;
    public GameObject addImage;

    Sequence seq;
    bool isTrigger = false;
    public bool isCompleted = false;

    public void Move(bool front = true, TweenCallback callback = null)
    {
        var value = front ? 1f : 20f;

        seq = DOTween.Sequence();

        seq.Append(slider.DOValue(value, 1.5f, true));

        seq.onComplete += callback;
        seq.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger || isCompleted) return;
        isTrigger = true;
        seq.Kill();


        button.transform.localEulerAngles = new Vector3(180, 0, 180);

        Move(false, () =>
        {
            Debug.Log("moveEnt");
            isTrigger = false;
            button.interactable = true;
            slider.value = 25f;
            button.transform.localEulerAngles = new Vector3(0, 0, 0);
        });
    }
}
