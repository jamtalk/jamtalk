using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ImageButton : MonoBehaviour
{
    public Button button => GetComponent<Button>();
    public Image image;
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
        image.preserveAspect = true;
    }
}
