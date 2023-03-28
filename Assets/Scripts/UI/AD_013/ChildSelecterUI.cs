using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChildSelecterUI : MonoBehaviour
{
    [SerializeField] private Transform layout;
    [SerializeField] private Button buttonAdd;
    [SerializeField] private ChildSummarayUI orizinal;
    private List<ChildSummarayUI> elements = new List<ChildSummarayUI>();
    public Action onClickAdd;
    public Action onSelect;
    private void Awake()
    {
        buttonAdd.onClick.AddListener(() => onClickAdd?.Invoke());
    }
    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        for (int i = 0; i < elements.Count; i++)
            Destroy(elements[i].gameObject);
        elements.Clear();

        var children = UserDataManager.Instance.children.OrderByDescending(x => x.RegistedDate).ToArray();
        for (int i = 0; i < children.Length; i++)
            Add(children[i]);
    }
    public void Add(ChildInfoData data)
    {
        var child = Instantiate(orizinal, layout);
        child.onClick.AddListener(OnSelect);
        child.Init(data);
        child.transform.SetSiblingIndex(1);
        child.gameObject.SetActive(true);
        elements.Add(child);
    }
    private void OnSelect()
    {
        onSelect?.Invoke();
    }
}
