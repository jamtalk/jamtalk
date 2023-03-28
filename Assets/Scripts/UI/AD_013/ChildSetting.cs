using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildSetting : MonoBehaviour
{
    public ChildSelecterUI selectChild;
    public AddChild addChild;
    public Button exitButton;

    private bool isAdd = false;

    private void OnEnable()
    {
        selectChild.gameObject.SetActive(true);
        addChild.gameObject.SetActive(false);
    }

    private void Awake()
    {
        selectChild.onClickAdd += () => addChild.gameObject.SetActive(true);
        selectChild.onSelect += () => gameObject.SetActive(false);
        exitButton.onClick.AddListener(ExitAction);

        addChild.onAdd += () =>
        {
            Debug.Log("Ãß°¡µÊ");
            if (isAdd)
                UserDataManager.Instance.UpdateChildren(()=> gameObject.SetActive(false));
            else
                addChild.gameObject.SetActive(false);
            selectChild.Init();
        };
    }

    public void Init(bool isAdd = false)
    {
        this.isAdd = isAdd;

        if (isAdd)
        {
            exitButton.gameObject.SetActive(false);
            addChild.gameObject.SetActive(true);
        }
    }

    private void ExitAction()
    {
        if (isAdd)
            gameObject.SetActive(false);
        else
        {
            if (addChild.gameObject.activeSelf)
            {
                if (addChild.terms.activeSelf)
                    addChild.terms.gameObject.SetActive(false);
                else
                    addChild.ExitAction();
            }
            else
                gameObject.SetActive(false);
        }
    }
}
