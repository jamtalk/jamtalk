using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ImageButton : MonoBehaviour
{
    public Button button => GetComponent<Button>();
    public Image image;
    public Sprite sprite
    {
        get => image.sprite;
        set
        {
            image.sprite = value;
            image.preserveAspect = true;
            image.name = value.name;
        }
    }
}
