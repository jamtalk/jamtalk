using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChildSummarayUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text textName;
    [SerializeField] private Button button;
    public UnityEvent onClick;
    public ChildInfoData data;
    private void Awake()
    {
        button.onClick.AddListener(OnSelect);
    }
    public void Init(ChildInfoData data)
    {
        this.data = data;
        textName.text = data.name;
        button.interactable = !data.Selected;
    }
    private void OnSelect()
    {
        data.Selected = true;
        onClick?.Invoke();
    }
}