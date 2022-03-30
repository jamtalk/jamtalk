using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class Charactor113 : MonoBehaviour
{
    public EventSystem eventSystem;
    public eCharactorDirection direction;
    public Button button => GetComponent<Button>();
    private IStopalbeAnimation anim => rt.GetComponent<IStopalbeAnimation>();
    public RectTransform rt;
    public float startPosition => (Screen.width - rt.sizeDelta.x / 2f) * (direction == eCharactorDirection.ToLeft ? 1f : -1f);
    public float middlePosition => (Screen.width/2f) * (direction == eCharactorDirection.ToLeft ? 1f : -1f);
    public float endPosition => (Screen.width) * (direction == eCharactorDirection.ToLeft ? -1f : 1f);
    public eAlphabet value { get; private set; }
    public AudioSinglePlayer movePlayer;
    public AudioSinglePlayer clapPlayer;
    public AudioClip clipClap;
    public event System.Action onAway;
    public Text textAlphabet;
    public Image product;
    private void Awake()
    {
        button.onClick.AddListener(() => clapPlayer.Play(GameManager.Instance.GetClipPhanics(value)));
    }
    public void Init(eAlphabet value)
    {
        this.value = value;
    }
    public void SetProduct(Sprite sprite,string value)
    {
        product.sprite = sprite;
        textAlphabet.text = value;
        product.gameObject.SetActive(true);
        clapPlayer.Play(clipClap);
        Away();
    }
    public void SetStartPosition()
    {
        var pos = rt.anchoredPosition;
        pos.x = startPosition;
        rt.anchoredPosition = pos;
    }
    public void Call()
    {
        product.gameObject.SetActive(false);
        eventSystem.enabled = false;
        SetStartPosition();
        var tween = rt.DOAnchorPosX(middlePosition, 1f);
        tween.SetEase(Ease.OutCubic);
        tween.onComplete += () =>
        {
            anim.Stop();
            movePlayer.Stop();
            clapPlayer.Play(GameManager.Instance.GetClipPhanics(value));
            eventSystem.enabled = true;
        };
        movePlayer.Play();
        tween.Play();
    }
    public void Away()
    {
        eventSystem.enabled = false;
        var tween = rt.DOAnchorPosX(endPosition, 4f);
        tween.onComplete += () =>
        {
            anim.Stop();
            movePlayer.Stop();
            eventSystem.enabled = true;
            onAway?.Invoke();
        };
        tween.SetEase(Ease.InCubic);
        movePlayer.Play();
        tween.Play();
    }
}
