using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordImage105 : MonoBehaviour
{
    public Image image;
    private void Awake()
    {
        Init("apple");
    }
    public void Init(string word)
    {
        image.sprite = GameManager.Instance.GetSpriteWord(word);
        image.preserveAspect = true;
    }
}
