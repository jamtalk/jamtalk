using System.Collections.Generic;
using UnityEngine;

public class BookWordCreator : MonoBehaviour
{
    public GameObject element;
    public GameObject emptyObject;
    public RectTransform parent => GetComponent<RectTransform>();

    public AlphabetToggle110[] Create(string value)
    {
        var words = value.Contains(" ") ? value.Split(' ') : new string[] { value };
        var elements = new List<AlphabetToggle110>();
        for(int i = 0;i < words.Length; i++)
        {
            var word = words[i];
            Debug.Log(word);
            for (int j = 0; j < word.Length; j++)
            {
                var alphabet = (eAlphabet)System.Enum.Parse(typeof(eAlphabet), word[j].ToString().ToUpper());
                var component = Instantiate(element, parent).GetComponent<AlphabetToggle110>();
                component.Init(alphabet);
                component.gameObject.name = alphabet.ToString();
                elements.Add(component);
            }
            if (i < words.Length - 1)
                Instantiate(emptyObject, parent).name = "empty";
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
