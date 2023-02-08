using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildSetting : MonoBehaviour
{
    public SelectChild selectChild;
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
        selectChild.addAction += () => addChild.gameObject.SetActive(true);
        exitButton.onClick.AddListener(ExitAction);

        addChild.onAdd += () =>
        {
            if (isAdd)
            {
                gameObject.SetActive(false);
                UserDataManager.Instance.LoadChildList();
            }
            else
                addChild.gameObject.SetActive(false);
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
        else
            selectChild.Init();
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
