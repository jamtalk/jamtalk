using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCreator110 : MonoBehaviour
{
    public GameObject element;
    public RectTransform parent => GetComponent<RectTransform>();

    public AlphabetToggle110[] Create(WordSource data)
    {
        var word = data.value.Replace(" ", "");
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

    public AlphabetToggle110[] Create(DigraphsSource data)
    {
        var word = data.value.Replace(" ", "");
        var elements = new List<AlphabetToggle110>();
        for (int i = 0; i < word.Length; i++)
        {
            var alphabet = (eAlphabet)System.Enum.Parse(typeof(eAlphabet), word[i].ToString().ToUpper());
            var component = Instantiate(element, parent).GetComponent<AlphabetToggle110>();
            component.Init(alphabet);
            elements.Add(component);
        }
        return elements.ToArray();
    }
}
