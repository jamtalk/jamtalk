using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Parent_ContentsInfo_Element : MonoBehaviour
{
    public Image sumnail;
    public Text comment;
    public void Init(CommentData commentData, int failCount)
    {
        Addressables.LoadAssetAsync<Sprite>(commentData.id.ToString()).Completed += (sprite) =>
        {
            sumnail.sprite = sprite.Result;
            sumnail.preserveAspect = true;
        };
        comment.text = commentData.GetComment(failCount);
    }
}
