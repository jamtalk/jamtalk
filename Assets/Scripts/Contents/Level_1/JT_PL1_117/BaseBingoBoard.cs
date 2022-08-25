using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseBingoBoard<TValue, TViewer, TButton> : MonoBehaviour
    where TButton : BaseBingoButton<TValue, TViewer>
    where TViewer : MonoBehaviour
{
    public GameObject cover;
    public TButton[] buttons;
    public Sprite[] stamps;
    public GridLayoutGroup layoutGroup;
    public int size => (int)Mathf.Sqrt(buttons.Length);

    public virtual void Init(TValue[] values, TValue[] corrects, Func<TValue, bool> isCorrect, Action<TValue> onClick)
    {
        Debug.Log(values.Length);
        for (int i = 0; i < buttons.Length; i++)
        {
            var stamp = stamps.OrderBy(x => Random.Range(0, stamps.Length)).First();
            buttons[i].onClick = onClick;
            var value = values[i];
            buttons[i].Init(value, stamp, isCorrect);
        }
    }
    public abstract bool CheckOn(int index);
    public int GetBingoCount()
    {
        int count = 0;
        List<Vector2> tmp = new List<Vector2>();
        int x = 0;
        int y = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (CheckOn(i))
            {
                tmp.Add(new Vector2(x, y));
            }

            x += 1;
            if (x % size == 0)
            {
                x = 0;
                y += 1;
            }
        }
        //???????? ??????
        for (int i = 0; i < size; i++)
        {
            //????
            if (tmp.Where(x => x.x == i).Count() == size)
                count += 1;
            //????
            if (tmp.Where(x => x.y == i).Count() == size)
                count += 1;
        }
        //??????
        var diagonalCount = tmp.Where(x => x.x == x.y).Count();
        if (diagonalCount > 0 && diagonalCount % size == 0)
            count += diagonalCount / size;
        return count;
    }

    public void ResizeBoard(Action callback) => StartCoroutine(Resize(callback));
    private IEnumerator Resize(Action callback)
    {
        var rt = GetComponent<RectTransform>();
        yield return new WaitForEndOfFrame();
        var size = rt.rect.size.y * 1f;
        var sizeDelta = rt.sizeDelta;
        sizeDelta.x = size;
        rt.sizeDelta = sizeDelta;

        rt = layoutGroup.GetComponent<RectTransform>();
        var cellSize = rt.rect.width / this.size;
        layoutGroup.cellSize = new Vector2(cellSize * .9f, cellSize * .9f);
        layoutGroup.spacing = new Vector2(cellSize * .1f, cellSize * .1f);
        cover.gameObject.SetActive(false);
        callback?.Invoke();
    }
}
