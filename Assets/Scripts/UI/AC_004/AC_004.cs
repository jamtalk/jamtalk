using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AC_004 : MonoBehaviour
{
    public Book_Listening book_Listening;
    public Image image;
    private void Awake()
    {
        GameManager.Instance.LoadAddressables((progress) => Debug.LogFormat("{0}% ÁøÇàÁß..", progress), () =>
        {
            image.sprite = GameManager.Instance.GetCurrentBook()
                .OrderBy(x => x.type)
                .OrderBy(x => x.bookNumber)
                .OrderBy(x => x.page)
                .First()
                .GetSprite();
        });
    }
}
