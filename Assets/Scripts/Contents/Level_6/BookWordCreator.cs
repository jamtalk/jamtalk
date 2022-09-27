using System.Collections.Generic;
using UnityEngine;

public class BookWordCreator : MonoBehaviour
{
    public GameObject element;
    public GameObject empty;
    public RectTransform parent => GetComponent<RectTransform>();

    public AlphabetToggle110[] Create(string data)
    {
        Clear();
        var words = data.Contains(" ") ? data.Split(' ') : new string[] { data };
        var elements = new List<AlphabetToggle110>();
        for(int i = 0;i < words.Length; i++)
        {
            if (i > 0)
                Instantiate(empty, parent);

            var word = words[i];
            for (int j = 0; j < word.Length; j++)
            {
                var alphabet = (eAlphabet)System.Enum.Parse(typeof(eAlphabet), word[j].ToString().ToUpper());
                var component = Instantiate(element, parent).GetComponent<AlphabetToggle110>();
                component.Init(alphabet);
                elements.Add(component);
            }
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
