using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public Image image => GetComponent<Image>();
    public bool playOnAwake=true;
    public float duration = 1f;
    private Coroutine anim = null;
    private void Awake()
    {
        if (playOnAwake)
            Play();
    }
    public void Play()
    {
        Stop();
        anim = StartCoroutine(animRoutine());
    }
    public void Stop()
    {
        if (anim != null)
        {
            StopCoroutine(anim);
            anim = null;
        }
    }
    IEnumerator animRoutine()
    {
        int index = 0;
        while (true)
        {
            image.sprite = sprites[index];
            yield return new WaitForSeconds(duration/sprites.Length);
            index += 1;
            if (index >= sprites.Length)
                index = 0;
        }
    }
}
