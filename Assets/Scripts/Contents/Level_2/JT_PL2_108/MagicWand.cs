using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagicWand<T> : MonoBehaviour
    where T : ResourceWordsElement
{
    public ParticleSystem particle;
    public Image potionImage;
    public Text potionValue;

    private void OnEnable()
    {
        particle.Play();
    }

    private void Update()
    {
        if (gameObject.activeSelf && Input.GetMouseButton(0))
        {
            transform.position = GameManager.Instance.GetMousePosition();
        }
        else
            gameObject.SetActive(false);
    }
    public void SetDrag(PotionElement<T> data)
    {
        transform.position = GameManager.Instance.GetMousePosition();
        potionImage.sprite = data.image.sprite;
        potionValue.text = data.textValue.text;
        gameObject.SetActive(true);
    }
}
