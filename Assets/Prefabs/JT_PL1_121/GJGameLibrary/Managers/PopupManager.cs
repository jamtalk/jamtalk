using GJGameLibrary;
using GJGameLibrary.DesignPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Android;

[Serializable]
public class PopupManager : MonoSingleton<PopupManager>
{
    public const string PopupGuidenceRecourcePath = "Popup/GuidencePopup";
    public const string PopupSceneLoadingRecourcePath = "Popup/SeceneLoadingPopup";
    public const string PopupLoadingResourcePath = "Popup/LoadingPopup";

    private GameObject guidance;
    private GameObject loading;
    private GameObject sceneLoading;//씬 로딩팝업

    private Stack<GameObject> popupStack;
    public event Action onEmptyPopup = null;
    public AudioSource popAudio;
    public Canvas uiCanvas
    {
        get
        {
            var canvas = FindObjectsOfType<Canvas>();
            if (canvas.Length > 1)
                return canvas.ToList().Find(x => x.name.Contains("UI"));
            else
                return canvas[0];
        }
    }
    //public void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //        pauseCount += 1;
    //}
    //public void OnApplicationFocus(bool focus)
    //{
    //    if(focus)
    //    {
    //        Debug.Log("집중!");
    //        if (pauseCount >= 3)
    //            ShowGuidance("프로그램을 재실행 해주세요", Application.Quit);
    //    }
    //}
    public int PopupCount => popupStack.Count;

    public override void Initialize()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

        popupStack = new Stack<GameObject>();
        guidance = Resources.Load<GameObject>(PopupGuidenceRecourcePath);
        loading = Resources.Load<GameObject>(PopupLoadingResourcePath);
        sceneLoading = Resources.Load<GameObject>(PopupSceneLoadingRecourcePath);
        base.Initialize();
    }
    public void ShowGuidance(string message, Action onOK = null)
    {
        SoundManager.Instance.PlayPopup();
        Popup<GuidencePopup>(guidance).Show(message, onOK);
    }
    public void ShowGuidance(string message, Action onOK, Action onCancel = null)
    {
        SoundManager.Instance.PlayPopup();
        Popup<GuidencePopup>(guidance).Show(message, onOK, onCancel);
    }
    public void ShowGuidanceMultiLine(string message, Action onOk = null)
    {
        SoundManager.Instance.PlayPopup();
        Popup<GuidencePopup>(guidance).ShowMultiLine(message, onOk);
    }
    public void ShowGuidanceMultiLine(string message, Action onOk, Action OnCancel = null)
    {
        SoundManager.Instance.PlayPopup();
        Popup<GuidencePopup>(guidance).ShowMultiLine(message, onOk, OnCancel);
    }
    public void ShowGuidance(GuidenceData data)
    {
        Popup<GuidencePopup>(guidance).Show(data);
    }

    public Action ShowLoading()
    {
        return Popup<LoadingPopup>(loading).ShowLoading();
    }
    public Action ShowSceneLoading()
    {
        return Popup<SceneLoadingPopup>(sceneLoading).ShowLoading();
    }

    public T Popup<T>(GameObject popupObject) where T:BasePopup
    {
        //if(typeof(T) != typeof(LoadingPopup))
        //    SoundManager.Instance.PlayPopup();
        foreach (var item in popupStack)
        {
            if(item == null)
            {
                Clear();
                return Popup<T>(popupObject);
            }
            //item.SetActive(false);
        }
        Transform parent = uiCanvas.transform;
        var popup = Instantiate(popupObject, parent);
        popup.transform.SetAsLastSibling();

        popupStack.Push(popup);
        
        var component = popup.GetComponent<T>();
        component.Open();
        return component;
    }
    public T Peek<T>() where T : BasePopup
    {
        return popupStack.Peek().GetComponent<T>();
    }
    public T Select<T>() where T : BasePopup
    {
        return popupStack.ToList().Find(x => x.GetComponent<T>() != null).GetComponent<T>();
    }

    /// <summary>
    /// Close popup
    /// </summary>
    public void Close()
    {
        if (popupStack.Count ==0)
        {
            onEmptyPopup?.Invoke();
            return;
        }
        else
        {
            var obj = popupStack.Pop();
            obj.GetComponent<BasePopup>().Close();
            Destroy(obj);
            if(popupStack.Count != 0)
            {
                popupStack.Peek().gameObject.SetActive(true);
            }
        }
    }
    public void Close(GameObject obj)
    {
        var stack = new Stack<GameObject>();
        while(popupStack.Count>0)
        {
            var popup = popupStack.Pop();
            if (obj != popup)
            {
                if (obj == null)
                    continue;
                else
                    stack.Push(popup);
            }
            else
            {
                Destroy(popup);
                break;
            }
        }
        while (stack.Count > 0)
        {
            popupStack.Push(stack.Pop());
        }
    }

    /// <summary>
    /// Close all popup
    /// </summary>
    public void Clear()
    {
        while (popupStack.Count != 0)
            Destroy(popupStack.Pop());
    }
}
public abstract class BasePopup : MonoBehaviour
{
    public event Action onClosed;
    public Button buttonExit;
    protected virtual void Awake()
    {
        if (buttonExit != null)
            buttonExit.onClick.AddListener(() => PopupManager.Instance.Close());
    }
    public virtual void Open() { Debug.Log(GetType().Name + " popup opened"); }
    public virtual void Close()
    {
        onClosed?.Invoke();
        Debug.Log(GetType().Name + " popup closed"); 
    }
}
public abstract class BaseFullScreenPopup : BasePopup
{
    public void SetFullScreen()
    {
        var rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
    }
}
