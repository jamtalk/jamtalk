using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BingoBoard : MonoBehaviour
{
    public BingoButton[] buttons;
    public Sprite[] stamps;
    public event Action<eAlphabet> onClick;
    public int size => (int)Mathf.Sqrt(buttons.Length);

    public void Init(eAlphabet[] alphabets, eAlphabet[] correct)
    {
        stamps = stamps.OrderBy(x => Random.Range(0, stamps.Length)).ToArray();
        var correctStamp = stamps.Last();
        var incorrectStamp = stamps.Take(stamps.Length - 1).ToArray();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick += onClick;
            var value = alphabets[i];
            if (correct.Contains(value))
                buttons[i].Init(value, correctStamp);
            else
                buttons[i].Init(value, incorrectStamp[Random.Range(0, incorrectStamp.Length)]);
        }
    }
    public int GetBingoCount()
    {
        int count = 0;
        List<Vector2> tmp = new List<Vector2>();
        int x = 0;
        int y = 0;
        for(int i = 0;i < buttons.Length; i++)
        {
            if (buttons[i].isOn)
            {
                tmp.Add(new Vector2(x, y));
            }

            x += 1;
            if(x%size==0)
            {
                x = 0;
                y += 1;
            }
        }
        //가로세로 카운팅
        for(int i = 0;i < size; i++)
        {
            //가로
            if (tmp.Where(x => x.x == i).Count() == size)
                count += 1;
            //세로
            if (tmp.Where(x => x.y == i).Count() == size)
                count += 1;
        }
        //대각선
        var diagonalCount = tmp.Where(x => x.x == x.y).Count();
        if (diagonalCount > 0 && diagonalCount % size == 0)
            count += diagonalCount / size;
        return count;
    }
}
