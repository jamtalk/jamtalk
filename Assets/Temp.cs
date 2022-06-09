using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class Drawable : MonoBehaviour
{
    public Image image;
    public AlphabetPaths paths;
    public eAlphabet alphabet;
    public eAlphabetType type;
    private void Awake()
    {
    }
    //public void Read()
    //{
    //    var item = paths.GetPath(alphabet, type);
    //    Debug.Log(string.Join("\n------\n",
    //        string.Join("\n",
    //        item.Select(x => string.Format("{0}¹øÂ° È¹\n{1}", x.stroke,string.Join("\n",x.path))))));
    //}
    //public void Clear()
    //{
    //    var tmp = new List<GameObject>();
    //    for (int i = 0; i < transform.childCount; i++)
    //        tmp.Add(transform.GetChild(i).gameObject);

    //    for (int i = 0; i < tmp.Count; i++)
    //        Destroy(tmp[i]);

    //    tmp.Clear();
    //}
    //public void Load()
    //{
    //    image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.FullColor, type, alphabet);
    //    image.preserveAspect = true;
    //}
    //public void Add()
    //{
    //    List<Vector2[]> path = new List<Vector2[]>();
    //    var count = transform.childCount;
    //    for (int i = 0; i < count; i++)
    //    {
    //        var child = transform.GetChild(i);
    //        List<RectTransform> tmp = new List<RectTransform>();
    //        for (int j = 0; j < child.childCount; j++)
    //        {
    //            tmp.Add(child.GetChild(j).gameObject.GetComponent<RectTransform>());
    //        }
    //        path.Add(tmp
    //            .Select(x => x.anchoredPosition)
    //            .ToArray()
    //            );
    //    }

    //    var items = new List<AlphabetPath>();
    //    for(int i = 0;i < path.Count; i++)
    //        items.Add(new AlphabetPath(alphabet, type,i, path[i]));
    //    paths.Add(alphabet, type, items.ToArray());
    //}
}
