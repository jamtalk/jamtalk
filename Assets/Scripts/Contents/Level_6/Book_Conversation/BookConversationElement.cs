using UnityEngine;
using UnityEngine.UI;

public class BookConversationElement : MonoBehaviour
{
    [SerializeField] private Sprite speakerA;
    [SerializeField] private Sprite speakerB;
    [SerializeField] private Image imageSpeaker;
    [SerializeField] private Text text;
    public void Init(BookConversationData data)
    {
        imageSpeaker.sprite = data.speaker == "A" ? speakerA : speakerB;
        text.text = data.value;
    }
}
