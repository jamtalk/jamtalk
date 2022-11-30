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

    private ChildProfile[] childrens => GetComponentsInChildren<ChildProfile>();
    public Text editText => editButton.GetComponentInChildren<Text>();
    public Text confirmText => confirmButton.GetComponentInChildren<Text>();

    private bool isEdit = false;


    private void Awake()
    {
        confirmText.text = "저장하기";

        editButton.onClick.AddListener(() =>
        {
            isEdit = !isEdit;
            foreach (var item in childrens)
                item.toggle.gameObject.SetActive(isEdit);

            if (isEdit)
            {
                editText.text = "취소";
                confirmText.text = "삭제하기";
            }
            else
            {
                editText.text = "삭제";
                confirmText.text = "저장하기";
            }
        });

        confirmButton.onClick.AddListener(() =>
        {
            if(isEdit)
            {
                var checkList = childrens.Where(x => x.isCheck).ToArray();

                if (checkList.Length == 0)
                    return;

                // 아이 삭제 requset 호출

                foreach (var item in checkList)
                    Destroy(item.gameObject);
            }
            else
            {
                // 아이 정보 수정 requset 호출 
            }

        });
    }
}
