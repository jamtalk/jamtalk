using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoBoard : MonoBehaviour
{
    public BingoButton[] buttons;
    public event Action<eAlphabet> onClick;
    public int size => (int)Mathf.Sqrt(buttons.Length);

    public void Init(eAlphabet[] alphabets)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick += onClick;
            buttons[i].Init(alphabets[i]);
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
        //���μ��� ī����
        for(int i = 0;i < size; i++)
        {
            //����
            if (tmp.Where(x => x.x == i).Count() == size)
                count += 1;
            //����
            if (tmp.Where(x => x.y == i).Count() == size)
                count += 1;
        }
        //�밢��
        var diagonalCount = tmp.Where(x => x.x == x.y).Count();
        if (diagonalCount > 0 && diagonalCount % size == 0)
            count += diagonalCount / size;
        return count;
    }
}
