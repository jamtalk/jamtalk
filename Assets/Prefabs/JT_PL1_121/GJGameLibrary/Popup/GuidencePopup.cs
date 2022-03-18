using System;
using UnityEngine;
using UnityEngine.UI;

public class GuidencePopup : BasePopup
{
    public Text textTitle;
    public Text textMessage;
    public GameObject multiMessageObject;
    public Text textMulti;
    public Button buttonOK;
    public Text textOK => buttonOK.GetComponentInChildren<Text>();
    public Button buttonCancel;
    public Text textCancel => buttonCancel.GetComponentInChildren<Text>();

    public void Show(string message, Action onOK = null)
    {
        multiMessageObject.SetActive(false);
        textMessage.gameObject.SetActive(true);
        textMessage.text = message;
        if (onOK == null)
            buttonOK.onClick.AddListener(PopupManager.Instance.Close);
        else
            buttonOK.onClick.AddListener(() => onOK?.Invoke());

        buttonCancel.gameObject.SetActive(false);
    }
    public void Show(string message, Action onOk, Action onCancel = null)
    {
        Show(message, onOk);
        if (onCancel == null)
            buttonCancel.onClick.AddListener(PopupManager.Instance.Close);
        else
            buttonCancel.onClick.AddListener(() => onCancel?.Invoke());

        buttonCancel.gameObject.SetActive(true);
    }
    public void ShowMultiLine(string message, Action onOK = null)
    {
        multiMessageObject.SetActive(true);
        textMessage.gameObject.SetActive(false);

        textMulti.text = message;

        if (onOK == null)
            buttonOK.onClick.AddListener(PopupManager.Instance.Close);
        else
            buttonOK.onClick.AddListener(() => onOK?.Invoke());

        buttonCancel.gameObject.SetActive(false);
    }
    public void ShowMultiLine(string message, Action onOk, Action onCancel = null)
    {
        ShowMultiLine(message, onOk);

        if (onCancel == null)
            buttonCancel.onClick.AddListener(PopupManager.Instance.Close);
        else
            buttonCancel.onClick.AddListener(() => onCancel?.Invoke());

        buttonCancel.gameObject.SetActive(true);
    }
    public void Show(GuidenceData data)
    {
        textTitle.text = data.title;
        textOK.text = data.textOK;
        textCancel.text = data.textCancel;
        if (data.isMultiline)
        {
            if (!data.isCanCancel)
                ShowMultiLine(data.text, data.onOK);
            else
                ShowMultiLine(data.text, data.onOK, data.onCancel);
        }
        else
        {
            if (!data.isCanCancel)
                Show(data.text, data.onOK);
            else
                Show(data.text, data.onOK, data.onCancel);
        }
    }
}
public class GuidenceBuilder
{
    private GuidenceData data;
    public GuidenceBuilder(string text)
    {
        data = new GuidenceData(text);
    }
    public GuidenceBuilder SetTitle(string title)
    {
        data.title = title;
        return this;
    }
    public GuidenceBuilder SetMultiline()
    {
        data.isMultiline = true;
        return this;
    }
    public GuidenceBuilder SetTextOK(string textOK)
    {
        data.textOK = textOK;
        return this;
    }
    public GuidenceBuilder SetTextCancel(string textCancel)
    {
        data.textCancel = textCancel;
        return this;
    }
    public GuidenceBuilder SetOnOK(Action onOK)
    {
        data.onOK = onOK;
        return this;
    }
    public GuidenceBuilder SetOnCancel(Action onCancel)
    {
        data.onCancel = onCancel;
        data.isCanCancel = true;
        return this;
    }
    public void Build() => PopupManager.Instance.ShowGuidance(data);
}
public class GuidenceData
{
    public string title = string.Empty;
    public string textOK = "확인";
    public string textCancel = "취소";
    public string text = string.Empty;
    public Action onOK;
    public Action onCancel=null;
    public bool isMultiline=false;
    public bool isCanCancel = false;
    public GuidenceData(string text)
    {
        this.text = text;
        onOK += PopupManager.Instance.Close;
        onOK += PopupManager.Instance.Close;
    }
}
