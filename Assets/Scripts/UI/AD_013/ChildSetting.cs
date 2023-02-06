using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildSetting : MonoBehaviour
{
    public SelectChild selectChild;
    public AddChild addChild;
    public Button exitButton;

    private void OnEnable()
    {
        selectChild.gameObject.SetActive(true);
        addChild.gameObject.SetActive(false);
    }

    private void Awake()
    {
        selectChild.addAction += () => addChild.gameObject.SetActive(true);
        exitButton.onClick.AddListener(ExitAction);
    }

    private void ExitAction()
    {
        Debug.Log(addChild.gameObject.activeSelf);
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
