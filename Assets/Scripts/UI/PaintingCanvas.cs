using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaintingCanvas : MonoBehaviour, IDragHandler, IEndDragHandler,IBeginDragHandler
{
    public bool resetOnAwake;
    public bool intractable;
    [Header("Canvas")]
    public RawImage canvas;
    public Color canvasColor;
    public AudioSource audioPlayer;

    [Header("Brush")]
    //[Range(1, 512)]
    //public int brushSize;
    public Texture2D brushTexture;
    public Color brushColor;
    public RectTransform rectTransfrom => GetComponent<RectTransform>();
    private Vector2 lastPos = Vector2.zero;
    public Texture2D texture
    {
        get
        {
            if(canvas.texture == null)
            {
                var texture = new Texture2D((int)rectTransfrom.sizeDelta.x, (int)rectTransfrom.sizeDelta.y);
                for (int i = 0; i < texture.width; i++)
                    for (int j = 0; j < texture.height; j++)
                        texture.SetPixel(i, j, canvasColor);
                texture.Apply();

                canvas.texture = texture;
            }

            return (Texture2D)canvas.texture;
        }
        set
        {
            var texture = new Texture2D(value.width, value.height);
            for (int i = 0; i < texture.width; i++)
                for (int j = 0; j < texture.height; j++)
                    texture.SetPixel(i, j, value.GetPixel(i,j));
            texture.Apply();
            canvas.texture = texture;
        }
    }

    public event Action onPainting;
    public event Action onPaintingEnd;

    private void Awake()
    {
        if (resetOnAwake)
            Clear();
    }

    public void Clear()
    {
        for (int i = 0; i < texture.width; i++)
            for (int j = 0; j < texture.height; j++)
                texture.SetPixel(i, j, canvasColor);
        texture.Apply();
        lastPos = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        if (lastPos != Vector2.zero)
        {
            var mousePos = eventData.position;
            var pixel = brushTexture.width/2f;
            var direction = (mousePos - lastPos).normalized * pixel;
            var targetPos = lastPos;
            var dis = Vector2.Distance(targetPos, mousePos);
            var count = 0;
            Debug.Log(direction);
            while (dis > pixel)
            {
                Debug.Log(dis);
                targetPos += direction;
                count += 1;
                dis = Vector2.Distance(targetPos, mousePos);
                PaintingPixel(ConvertPixelPosition(targetPos));
            }

        }
        lastPos = eventData.position;
        PaintingPixel(ConvertPixelPosition(eventData.position));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (audioPlayer != null)
            audioPlayer.Stop();

        onPaintingEnd?.Invoke();
    }

    private void PaintingPixel(Vector2 pixcelPos)
    {
        var x = (int)pixcelPos.x;
        var y = (int)pixcelPos.y;
        var width = (int)rectTransfrom.sizeDelta.x;
        var hegiht = (int)rectTransfrom.sizeDelta.y;
        bool isPainted = false;
        //var brushSize = 100;
        //for (int i = x - brushSize / 2; i <= x + brushSize / 2; i++)
        //{
        //    if (i < 0 || i > width)
        //        continue;
        //    for (int j = y - brushSize / 2; j <= y + brushSize / 2; j++)
        //    {
        //        if (j < 0 || j > hegiht)
        //            continue;

        //        texture.SetPixel(i, j, brushColor);
        //        isPainted = true;
        //    }
        //}
        for (int i = 0; i < brushTexture.width; i++)
        {
            var posX = x - brushTexture.width / 2 + i;
            if (posX < 0 || posX > width)
                continue;
            for (int j = 0; j < brushTexture.height; j++)
            {
                var posY = y - brushTexture.height / 2 + j;
                if (posY < 0 || posY > hegiht)
                    continue;

                if (brushTexture.GetPixel(i, j).a < 0.1f)
                    continue;

                texture.SetPixel(posX, posY, brushColor);
                isPainted = true;
            }
        }

        if (isPainted)
        {
            texture.Apply();
            onPainting?.Invoke();
        }
    }
    public Vector2 ConvertPixelPosition(Vector2 pos)
    {
        Vector2 targetPos = new Vector2((Screen.width - rectTransfrom.sizeDelta.x) / -2, (Screen.height - rectTransfrom.sizeDelta.y) / -2);
        targetPos += pos - rectTransfrom.anchoredPosition;
        return targetPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(audioPlayer!= null)
            audioPlayer.Play();
    }
}
