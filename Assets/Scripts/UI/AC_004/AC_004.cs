using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AC_004 : MonoBehaviour
{
    public Book_Listening book_Listening;
    public Image image;
    public Dropdown book;
    public Dropdown bookNumber;
    public Dropdown bookPage;
    public Text textLog;
    private void Awake()
    {
        book.options = GameManager.Instance.GetEnums<eBookType>()
            .OrderBy(x=>(int)x)
            .Select(x =>
            {
                switch (x)
                {
                    case eBookType.JE:
                        return "잼잼 잉글리시";
                    case eBookType.SE:
                        return "스마트 잉글리시";
                    case eBookType.LD:
                        return "리틀 다빈치";
                    case eBookType.SP:
                        return "수 놀이터";
                    default:
                        return string.Empty;
                }
            })
            .Select(x => new Dropdown.OptionData(x))
            .ToList();
        OnBookChanged(0);
        OnBookNumberChanged(0);
        OnBookPageChanged(0);
    }
    private void OnBookChanged(int value)
    {
        GameManager.Instance.currentBook = (eBookType)value;
        GameManager.Instance.currentBookNumber = 1;
        GameManager.Instance.currentPage = 1;
        bookNumber.options = GameManager.Instance.GetBookNumbers(GameManager.Instance.currentBook).Select(x => new Dropdown.OptionData(x + "권")).ToList();
        OnBookNumberChanged(0);
        textLog.text = string.Empty;
    }

    private void OnBookNumberChanged(int value)
    {
        GameManager.Instance.currentBookNumber = value+1;
        GameManager.Instance.currentPage = 1;
        bookPage.options = GameManager.Instance.GetBookNumbers(GameManager.Instance.currentBook, value + 1).Select(x => new Dropdown.OptionData(x + "페이지")).ToList();
        OnBookPageChanged(0);

        textLog.text = string.Empty;
    }

    private void OnBookPageChanged(int value)
    {
        GameManager.Instance.currentPage = value + 1;
        var currentBook = GameManager.Instance.GetCurrentBook().GetURLData();

        var hasAnimation = !string.IsNullOrEmpty(currentBook.animationURL);
        var hasSong = !string.IsNullOrEmpty(currentBook.songURL);

        var list = new List<string>();
        if (!hasAnimation)
            list.Add("애니메이션 URL이 등록되어있지 않습니다");
        if (!hasSong)
            list.Add("송 URL이 등록되어있지 않습니다");

        textLog.text = string.Join("\n", list);
    }
}
