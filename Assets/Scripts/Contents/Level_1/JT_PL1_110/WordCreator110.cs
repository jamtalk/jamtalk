using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCreator110 : MonoBehaviour
{
    public GameObject element;
    public RectTransform parent => GetComponent<RectTransform>();

    public AlphabetToggle110[] Create<T>(T data) where T : ResourceWordsElement
    {
        var word = data.key.Replace(" ", "");
        var elements = new List<AlphabetToggle110>();
        for(int i = 0;i < word.Length; i++)
        {
            var alphabet = (eAlphabet)System.Enum.Parse(typeof(eAlphabet), word[i].ToString().ToUpper());
            var component = Instantiate(element, parent).GetComponent<AlphabetToggle110>();
            component.Init(alphabet);
            elements.Add(component);
        }
        return elements.ToArray();
    }
    public void Clear()
    {
        var targets = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
            targets.Add(parent.GetChild(i).gameObject);
        for (int i = 0; i < targets.Count; i++)
            Destroy(targets[i]);
    }
}
