using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagElement : MonoBehaviour
{
    public Animator eyeAni;
    public Animator mouseAni;
    public Image image;
    [HideInInspector]
    public RectTransform imageRt => image.GetComponent<RectTransform>();

    public enum eAnis
    {
        EyeIdle,
        Tracking,
        Speak,
    }

    public void SetBagImage(Sprite sprite)
    {
        image.sprite = sprite;
        image.preserveAspect = true;
        image.gameObject.SetActive(true);
    }

    public void EyeAni(eAnis anis)
    {
        eyeAni.SetBool(anis.ToString(), true);
    }

    public void MouseSpeak()
    {
        mouseAni.SetBool(eAnis.Speak.ToString(), true);
    }
}
