using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BingoScoreBoard : MonoBehaviour
{
    [SerializeField]
    private Text textValue;
    [SerializeField]
    private Toggle[] toggles;
    public int score { get; private set; }
    public int incorrectCount
    {
        get => toggles.Where(x => x.isOn).Count();
        set
        {
            for (int i = 0; i < toggles.Length; i++)
                toggles[i].isOn = i < value;
        }
    }
    public event Action onFailed;
    public void Init()
    {
        score = 0;
        incorrectCount = 0;
        textValue.text = "0";
    }
    public void AddScore(int score)
    {
        this.score += score;
        textValue.text = score.ToString();
    }
    public void IncreaseIncorrect()
    {
        incorrectCount += 1;
        if (incorrectCount == toggles.Length)
            onFailed?.Invoke();
    }
}
