using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ChildModifierUI : MonoBehaviour
{
    [SerializeField] protected Button buttonExit;
    [SerializeField] protected Button buttonDelete;
    [SerializeField] protected Button buttonConfirm;
    [SerializeField] protected Button buttonAdd;
    [SerializeField] protected TMP_InputField inputName;
    [SerializeField] protected TMP_InputField inputBirthday;
    private ChildInfoData child;
    public UnityEvent onClose;

    protected virtual void Awake()
    {
        buttonExit.onClick.AddListener(OnExit);
        buttonDelete.onClick.AddListener(OnDelete);
        buttonConfirm.onClick.AddListener(OnConfirm);
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
                AndroidPluginManager.Instance.Toast("삭제 완료");
            });
        });
    }
    public void Init()
    {
        child = UserDataManager.Instance.CurrentChild;
        inputName.text = child.name;
        inputBirthday.text = child.jumin;
    }
}