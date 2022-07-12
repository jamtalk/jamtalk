using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseBingoBoard<TValue, TViewer, TButton> : MonoBehaviour
    where TButton : BaseBingoButton<TValue, TViewer>
    where TViewer : MonoBehaviour
{
    public TButton[] buttons;
    public Sprite[] stamps;
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
}
