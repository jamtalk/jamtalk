using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ChildProfileEdit : MonoBehaviour
{
    public ChildProfile childProfileOrigin;
    public Button exitButton;
    public Button editButton;
    public Button confirmButton;
    public Button addButton;
    public Text titleText;

    public RectTransform topRt;
    public RectTransform bottomRt;

    private List<ChildProfile> childrens = new List<ChildProfile>();
    private Text editText => editButton.GetComponentInChildren<Text>();
    private Text confirmText => confirmButton.GetComponentInChildren<Text>();

    private bool isEdit = false;
    private bool isAdd = false;
    private bool isOver => childrens.Count < 2;
    private ChildProfile addProfile;

    public ChildProfile testProfile;
    private void Awake()
    {
        exitButton.onClick.AddListener(ExitAction);
        editButton.onClick.AddListener(EditAction);
        addButton.onClick.AddListener(AddAction);
        //confirmButton.onClick.AddListener(() => ConfirmAction());


        Init();
    }

    private void OnEnable()
    {
        addButton.gameObject.SetActive(isOver);
    }

    /// <summary>
    /// 아이 데이터 목록 받아와서 생성 및 출력
    /// </summary>
    private void Init()
    {
        childrens.Add(testProfile);

        addButton.gameObject.SetActive(isOver);
    }

    private void ExitAction()
    {
        addButton.gameObject.SetActive(isOver);

        if (isAdd)
        {
            isAdd = false;

            addProfile.gameObject.SetActive(false);
            editButton.gameObject.SetActive(true);
            foreach (var item in childrens)
                item.gameObject.SetActive(true);

            titleText.text = "아이정보관리";
            confirmText.text = "저장하기";
        }
        else if (!isEdit)
            gameObject.SetActive(false);
        else
            EditAction();
    }

    private void AddAction()
    {
        isAdd = true;

        confirmText.text = "추가하기";
        titleText.text = "아이추가등록";

        editButton.gameObject.SetActive(false);
        addButton.gameObject.SetActive(false);

        foreach (var item in childrens)
            item.gameObject.SetActive(false);

        if (addProfile == null)
            addProfile = Instantiate(childProfileOrigin, topRt);
        else
        {
            addProfile.gameObject.SetActive(true);
            addProfile.inputName.text = string.Empty;
            addProfile.inputBirth.text = string.Empty;
        }
    }

    private void EditAction()
    {
        isEdit = !isEdit;
        foreach (var item in childrens)
            item.SetEdit(isEdit);

        if (isEdit)
        {
            addButton.gameObject.SetActive(false);
            editText.text = "취소";
            confirmText.text = "삭제하기";
            titleText.text = "정보삭제";
        }
        else
        {
            editText.text = "삭제";
            confirmText.text = "저장하기";
            titleText.text = "아이정보관리";
        }
    }

    private void ConfirmAction()
    {
        if (!isEdit)
        {
            if (isAdd)
            { // 아이 정보 추가
                if (string.IsNullOrEmpty(addProfile.textName.text) || string.IsNullOrEmpty(addProfile.textBirth.text))
                    return;
                else
                {
                    var param = new ChildParam(addProfile.textBirth.text, addProfile.textBirth.text);

                    RequestManager.Instance.Request(param, (res) =>
                    {
                        var result = res.GetResult<ActRequestResult>();

                        if(result.code != eErrorCode.Success)
                        {
                            Debug.Log(result.code);
                            AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
                        }
                        else
                        {
                            // 추가 된 아이 정보 표시 *_*
                        }
                    });
                }
            }
            else
            { // 아이 정보 삭제 
                var checkList = childrens.Where(x => x.isCheck).ToArray();

                if (checkList.Length == 0)
                    return;

                // 아이 삭제 requset 호출 *_*

                foreach (var item in checkList)
                    Destroy(item.gameObject);

                childrens.Clear();
            }
        }
        else
        {
            // 아이 정보 수정 requset 호출 *_*
        }
    }
}
