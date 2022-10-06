using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GuideBase<T> : MonoBehaviour
    where T : BaseContents
{
    public GuideFingerAnimation finger;

    public void Init()
    {
        finger = Instantiate(finger, transform);
    }
    public void GetGuide(T contents)
    {
        
    }
}
