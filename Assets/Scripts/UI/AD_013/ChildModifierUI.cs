using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ChildModifierUI : MonoBehaviour
{
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonDelete;
    [SerializeField] private Button buttonConfirm;
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TMP_InputField inputBirthday;

    [SerializeField] private Button buttonAdd;
    [SerializeField] private Transform parent;
    [SerializeField] private ChildSummarayUI orizinal;
    [SerializeField] private ChildSetting popupOrizinal;
    private ChildSetting popupInstance;
    private List<ChildSummarayUI> elements = new List<ChildSummarayUI>();

    private ChildInfoData child;
    public UnityEvent onClose;

    protected virtual void Awake()
    {
        buttonExit.onClick.AddListener(OnExit);
        buttonDelete.onClick.AddListener(OnDelete);
        buttonConfirm.onClick.AddListener(OnConfirm);
        buttonAdd.onClick.AddListener(OnClickAdd);
    }

    private void OnExit()
    {
        gameObject.SetActive(false);
        onClose?.Invoke();
    }
    private void OnEnable()
    {
        Init();
    }
    private void OnClickAdd()
    {
        if (popupInstance == null)
        {
            popupInstance = Instantiate(popupOrizinal, transform.parent);
            popupInstance.Init();
            popupInstance.selectChild.onSelect += () => Init();
            popupInstance.addChild.onAdd += () =>
            {
                UserDataManager.Instance.UpdateChildren(()=>
                {
                    Init();
                    popupInstance.gameObject.SetActive(false);
                });
            };
        }
        else
            popupInstance.gameObject.SetActive(true);

        popupInstance.Init(true);

    }
    private void OnDelete()
    {
        PopupManager.Instance.ShowGuidance(string.Format("삭제 하시겠습니까?"),DeleteChild,PopupManager.Instance.Close);
    }
    private void OnConfirm()
    {
        if (string.IsNullOrEmpty(inputName.text))
            AndroidPluginManager.Instance.Toast("이름을 입력해주세요");
        else if (string.IsNullOrEmpty(inputBirthday.text))
            AndroidPluginManager.Instance.Toast("생년월일을 입력해주세요");
        else if(inputBirthday.text.Length<8)
            AndroidPluginManager.Instance.Toast("올바른 생년월일을 입력해주세요");
        else
        {
            child.name = inputName.text;
            child.jumin = inputBirthday.text;
            RequestManager.Instance.Request(new ChildOutParam(child), response =>
            {
                AndroidPluginManager.Instance.Toast("수정 완료");
            });
        }
    }
    private void DeleteChild()
    {
        child.display = 0;
        RequestManager.Instance.Request(new ChildOutParam(child), response =>
        {
            UserDataManager.Instance.UpdateChildren(() =>
            {
                UserDataManager.Instance.children.OrderByDescending(x => x.RegistedDate).First().Selected = true;
                Init();
                AndroidPluginManager.Instance.Toast("삭제 완료");
            });
        });
    }
    public void Init()
    {
        child = UserDataManager.Instance.CurrentChild;
        inputName.text = child.name;
        inputBirthday.text = child.jumin;
        CreateChildren();
        buttonDelete.gameObject.SetActive(UserDataManager.Instance.children.Length > 1);
    }

    private void CreateChildren()
    {
        for (int i = 0; i < elements.Count; i++)
            Destroy(elements[i].gameObject);
        elements.Clear();

        var children = UserDataManager.Instance.children.OrderBy(x => x.RegistedDate).ToArray();
        for(int i = 0;i < children.Length; i++)
        {
            CreateChild(children[i]);
        }

    }
    public void CreateChild(ChildInfoData data)
    {
        var child = Instantiate(orizinal, parent);
        child.Init(data);
        child.transform.SetSiblingIndex(1);
    }
}