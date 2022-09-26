public class ProfilePopupButton : PopupButton<PopupProfile>
{
    protected override PopupProfile PopUp()
    {
        var popup = base.PopUp();
        popup.ShowProfile();
        return popup;
    }
}
