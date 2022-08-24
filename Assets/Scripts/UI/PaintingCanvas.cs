using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaintingCanvas : MonoBehaviour, IDragHandler, IPointerUpHandler,IPointerDownHandler
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
    public bool[][] brushChecker;
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
            var pix = value.GetRawTextureData();
            var orizin = new Texture2D(value.width, value.height, value.format, false);
            orizin.LoadRawTextureData(pix);
            var texture = new Texture2D(value.width, value.height);
            for (int i = 0; i < texture.width; i++)
                for (int j = 0; j < texture.height; j++)
                    texture.SetPixel(i, j, orizin.GetPixel(i,j));
            texture.Apply();
            canvas.texture = texture;
        }
    }

    public event Action onPainting;
    public event Action onPaintingEnd;

    private void Awake()
    {
        var tmp = new List<bool[]>();
        for(int i = 0;i < brushTexture.width; i++)
        {
            var list = new List<bool>();
            for(int j = 0; j < brushTexture.height; j++)
            {
                list.Add(brushTexture.GetPixel(i, j).a >= 0.1f);
            }
            tmp.Add(list.ToArray());
        }
        brushChecker = tmp.ToArray();

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
        var list = new List<Vector2>();
        if (lastPos != Vector2.zero)
        {
            var mousePos = eventData.position;
            var pixel = brushTexture.width / 3f;
            var direction = (mousePos - lastPos).normalized * pixel;
            var targetPos = lastPos;
            var dis = Vector2.Distance(targetPos, mousePos);
            var count = 0;
            while (dis > pixel)
            {
                targetPos += direction;
                count += 1;
                dis = Vector2.Distance(targetPos, mousePos);
                list.Add(ConvertPixelPosition(targetPos));
            }
        }
        lastPos = eventData.position;
        list.Add(ConvertPixelPosition(eventData.position));
        PaintingPixel(list.ToArray());
        Debug.DrawLine(eventData.position, eventData.position + Vector2.up*100,Color.black);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (audioPlayer != null)
            audioPlayer.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (audioPlayer != null)
            audioPlayer.Stop();
        lastPos = Vector2.zero;
        onPaintingEnd?.Invoke();
    }
    private void PaintingPixel(Vector2[] pixelsPos)
    {
        var width = (int)rectTransfrom.sizeDelta.x;
        var height = (int)rectTransfrom.sizeDelta.y;
        var list = pixelsPos.SelectMany(pixelPos =>
        {
            var tmp = new List<Vector2Int>();
            var x = (int)pixelPos.x;
            var y = (int)pixelPos.y;
            for (int i = 0; i < brushTexture.width; i++)
            {
                var posX = x - brushTexture.width / 2 + i;
                //if (posX < 0 || posX > width)
                //    continue;
                for (int j = 0; j < brushTexture.height; j++)
                {
                    var posY = y - brushTexture.height / 2 + j;
                    //if (posY < 0 || posY > height)
                    //    continue;

                    //if (brushTexture.GetPixel(i, j).a < 0.1f)
                    //    continue;
                    if(brushChecker[i][j])
                        tmp.Add(new Vector2Int(posX, posY));
                }
            }
            return tmp.ToArray();
        })
        .Where(x => x.x >= 0)
        .Where(x => x.x <= width)
        .Where(y => y.y >= 0)
        .Where(y => y.y <= height)
        .Distinct()
        .ToArray();

        if (list.Length>0)
        {
            for (int i = 0; i < list.Length; i++)
                texture.SetPixel(list[i].x, list[i].y, brushColor);
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
