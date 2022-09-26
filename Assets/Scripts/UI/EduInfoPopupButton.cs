public class EduInfoPopupButton : PopupButton<PopupProfile>
{

    protected override PopupProfile PopUp()
    {
        var popup = base.PopUp();
        popup.ShowEduInfo();
        return popup;
    }
}