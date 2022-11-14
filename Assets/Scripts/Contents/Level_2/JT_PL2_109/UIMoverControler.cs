using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class UIMoverControler : MonoBehaviour
{
    public UIMover[] movers;
    public RectTransform[] firstPaths;
    public RectTransform[] secondPaths;

    private List<RectTransform[]> paths = new List<RectTransform[]>();

    private void Awake()
    {
        paths.Add(firstPaths);
        paths.Add(secondPaths);
    }
    public void Init()
    {
        foreach (var item in movers)
            item.gameObject.SetActive(false);
        var items = movers.OrderBy(x => Random.Range(0, 100)).Take(paths.Count).Take(paths.Count).ToArray();

        for (int i = 0; i < paths.Count; i++)
        {
            items[i].gameObject.SetActive(true);
            items[i].paths = paths[i];
            items[i].Move(4f, 3f);
        }
    }
}
