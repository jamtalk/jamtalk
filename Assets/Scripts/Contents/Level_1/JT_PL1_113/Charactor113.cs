using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class Charactor113 : MonoBehaviour
{
    public GameObject finger;
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
        button.onClick.AddListener(() =>
        {
            finger.gameObject.SetActive(false);
            clapPlayer.Play(GameManager.Instance.GetResources(value).AudioData.phanics);
        });
    }

    public void ResetPosition()
    {
        var rtX = rt.position;
        rtX.x = 2000f;

        rt.position = rtX;
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
    public void Call(TweenCallback callback = null)
    {
        finger.SetActive(false);
        product.gameObject.SetActive(false);
        eventSystem.enabled = false;
        SetStartPosition();
        anim.Play();
        var tween = rt.DOAnchorPosX(middlePosition, 1f);
        tween.SetEase(Ease.OutCubic);
        tween.onComplete += () =>
        {
            anim.Stop();
            movePlayer.Stop();
            clapPlayer.Play(GameManager.Instance.GetResources(value).AudioData.phanics);
            eventSystem.enabled = true;

            finger.SetActive(true);
        };
        tween.onComplete += callback;
        movePlayer.Play();
        tween.Play();
    }
    public void Away()
    {
        finger.SetActive(false);
        eventSystem.enabled = false;
        anim.Play();
        var tween = rt.DOAnchorPosX(endPosition, 4f);
        tween.onComplete += () =>
        {
            Debug.Log("away");
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
