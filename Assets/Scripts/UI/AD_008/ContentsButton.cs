using GJGameLibrary;
using UnityEngine;

public class ContentsButton : BaseContentsButton
{
    public eContents contents;
    public void Init(eContents contents, Sprite sumnail)
    {
        this.contents = contents;
        button.image.sprite = sumnail;
        button.image.preserveAspect = true;
    }
    protected override void LoadScene()=> GJSceneLoader.Instance.LoadScene((eSceneName)System.Enum.Parse(typeof(eSceneName), contents.ToString()), true);
}