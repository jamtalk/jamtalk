using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaintingCanvas : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool resetOnAwake;

    [Header("Canvas")]
    public RawImage canvas;
    public Color canvasColor;

    [Header("Brush")]
    [Range(1,512)]
    public int brushSize;
    public Color brushColor;

    public RectTransform rectTransfrom => GetComponent<RectTransform>();
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        PaintingPixel(ConvertPixelPosition(eventData.position));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onPaintingEnd?.Invoke();
    }

    private void PaintingPixel(Vector2 pixcelPos)
    {
        var x = (int)pixcelPos.x;
        var y = (int)pixcelPos.y;
        var width = (int)rectTransfrom.sizeDelta.x;
        var hegiht = (int)rectTransfrom.sizeDelta.y;
        bool isPainted = false;
        for (int i = x - brushSize; i<=x+brushSize; i++)
        {
            if (i < 0 || i > width)
                continue;
            for(int j=  y-brushSize; j <= y+brushSize; j++)
            {
                if (j < 0 || j > hegiht)
                    continue;

                texture.SetPixel(i, j, brushColor);
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
}
