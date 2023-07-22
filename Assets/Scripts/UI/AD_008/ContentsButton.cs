using GJGameLibrary;
using UnityEngine;

public class ContentsButton : BaseContentsButton
{
    public eContents contents;
    public GameObject imageLock;

    public void Init(eContents contents, Sprite sumnail)
    {
        this.contents = contents;
        button.image.sprite = sumnail;
        button.image.preserveAspect = true;
        button.interactable = true;
        imageLock.gameObject.SetActive(false);
    }
    public void Disable()
    {
#if DEPLOY
        button.interactable = false;
        imageLock.gameObject.SetActive(true);
#endif
    }
    protected override void LoadScene()=> GJSceneLoader.Instance.LoadScene((eSceneName)System.Enum.Parse(typeof(eSceneName), contents.ToString()), true);
}