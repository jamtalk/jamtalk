using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PotElement : MonoBehaviour
{
    public string[] valueList { get; private set; }
    public string[] valueNumber { get; private set; }
    public string value { get; private set; }
    public int number { get; private set; }
    public void Init(IEnumerable<string> value)
    {
        valueList = value.ToArray();
        valueNumber = value.ToArray();
        GetValue();
    }
    public void GetValue()
    {
        var rand = Random.Range(0, valueList.Length);
        value = valueList[rand];
        valueList = valueList.Where(x => x != value).ToArray();

        for(int i = 0; i < valueNumber.Length; i++)
        {
            if (valueNumber[i].Contains(value))
                number = i;
        }

        Debug.Log("current : " + value);
    }
}