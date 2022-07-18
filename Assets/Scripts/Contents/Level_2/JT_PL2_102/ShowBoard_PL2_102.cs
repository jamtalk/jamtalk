using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBoard_PL2_102 : MonoBehaviour
{
    public Text textType;
    public Text textValue;
    public AudioSinglePlayer audioPlayer;
    public void Show(VowelWordsData data, System.Action onDone)
    {
        textType.text = data.type;
        textValue.text = data.key;
        gameObject.SetActive(true);
        audioPlayer.Play(data.act, () =>
        {
            gameObject.SetActive(false);
            onDone?.Invoke();
        });
    }
}
