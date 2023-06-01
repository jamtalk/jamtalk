using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Parent_TimeLabel : MonoBehaviour
{
    public Parent_TimeLabel_Card cardTotal;
    public Parent_TimeLabel_Card cardMonthly;
    public Text textTime;

    private void Awake()
    {
        var child = UserDataManager.Instance.CurrentChild;
    }
}

[System.Serializable]
public class Parent_TimeLabel_Card
{
    public Text textPeriod;
    public Text textTime;
    public Text textBooks;
}
