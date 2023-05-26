using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BookConversationElementManager : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private BookConversationElement orizinal;
    private List<BookConversationElement> elements = new List<BookConversationElement>();
    public BookConversationElement Create(BookConversationData data)
    {
        var targets = elements.Where(x => !x.gameObject.activeSelf);
        BookConversationElement target;
        if (targets.Count() > 0)
            target = targets.First();
        else
        {
            target = Instantiate(orizinal, parent);
            elements.Add(target);
        }

        target.Init(data);
        return target;
    }
    public void Clear()
    {
        for (int i = 0; i < elements.Count; i++)
            elements[i].gameObject.SetActive(false);
    }
}
