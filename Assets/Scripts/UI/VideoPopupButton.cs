using UnityEngine.Video;

public class VideoPopupButton : PopupButton<VideoPopup>
{
    public VideoClip clip;
    protected override VideoPopup PopUp()
    {
        var popup = base.PopUp();
        popup.Init(clip);
        return popup;
    }
}
