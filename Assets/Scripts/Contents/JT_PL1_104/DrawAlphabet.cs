using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawAlphabet : MonoBehaviour
{
    public Transform pathParent;
    [SerializeField]
    private AlphabetPaths pathData;
    public RectTransform arrow;
    public AlphabetPath[] Path { get; private set; }
    public Image image;
    public PaintingCanvas paintingCanvas;
    public event Action onCompleted;
    public int stroke { get; private set; }
    public int index { get; private set; }
    private bool isCompletedStroke = false;
    public Vector2 currentPos => Path[stroke].path[index];
    private void Awake()
    {
        paintingCanvas.onPainting += OnPainting;
        paintingCanvas.onPaintingEnd += OnEndPainting;
    }
    private void OnDisable()
    {
        paintingCanvas.onPainting -= OnPainting;
        paintingCanvas.onPaintingEnd -= OnEndPainting;
    }

    public void Init(eAlphabet alphabet, eAlphabetType type)
    {
        isCompletedStroke = false;
        index = 0;
        stroke = 0;
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, type, alphabet);
        image.preserveAspect = true;
        paintingCanvas.texture = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Gray, type, alphabet).texture;
        Path = pathData.GetPath(alphabet, type);
        SetArrow();
    }
    private void OnPainting()
    {
        var mousePos = Input.mousePosition;
        mousePos.x += Screen.width / -2;
        mousePos.y += Screen.height / -2;
        SetArrow();
        if (index == Path[stroke].path.Length)
        {
            //paintingCanvas.intractable = false;
            arrow.gameObject.SetActive(false);
        }
        else
        {
            var dis = Vector2.Distance(mousePos, Path[stroke].path[index]);
            if (dis < 75f)
            {
                index += 1;
                OnPainting();
            }
        }
    }
    private void OnEndPainting()
    {
        index = 0;
        if (isCompletedStroke)
        {
            stroke += 1;
            Debug.LogFormat("{0}/{1}", stroke, Path.Length);
            if (stroke == Path.Length)
                onCompleted?.Invoke();
            else
            {
                SetArrow();
                arrow.gameObject.SetActive(true);
                //paintingCanvas.intractable = true;
            }
        }
        else
        {
            SetArrow();
        }
        isCompletedStroke = false;
    }
    private void SetArrow()
    {
        if (stroke == Path.Length)
            return;
        else if (index == Path[stroke].path.Length)
        {
            arrow.gameObject.SetActive(false);
            isCompletedStroke = true;
            return;
        }

        Vector2 currentPos;
        Vector2 targetPos;
        if (index == 0)
        {
            //First
            currentPos = arrow.anchoredPosition = this.currentPos;
            targetPos = Path[stroke].path[index + 1];
        }
        else if (index + 1 == Path[stroke].path.Length)
        {
            //Last
            currentPos = Path[stroke].path[index - 1];
            targetPos = Path[stroke].path[index];
            arrow.anchoredPosition = targetPos;
        }
        else
        {
            currentPos = this.currentPos;
            targetPos = Path[stroke].path[index + 1];
            arrow.anchoredPosition = currentPos;
        }
        var v1 = currentPos - targetPos;
        var angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg - 180f;
        arrow.rotation = Quaternion.Euler(0, 0, angle);

    }
}
