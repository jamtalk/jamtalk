using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChildViewPage : MonoBehaviour
{
    public Button childEditButton;
    public Button childSettingButton;
    public Text childName;
    public Text childBirth;
    public Text childGender;
    public Text pointText;

    public ChildProfileEdit profileEdit;
    public ChildSetting childSettingOrizin;
    private ChildSetting childSetting;

    private void Awake()
    {
        childEditButton.onClick.AddListener(() => profileEdit.gameObject.SetActive(true));
        childSettingButton.onClick.AddListener(ChildSetting);
    }

    public void Init()
    {
        var data = UserDataManager.Instance.childList.Where(x => x.isDislplay).First();

        childName.text = data.name;
        childBirth.text = data.jumin;
        childGender.text = data.gender == 'F' ? "여자아이" : "남자아이";
        pointText.text = data.point.ToString();
    }

    private void ChildSetting()
    {
        if (childSetting == null)
        {
            childSetting = Instantiate(childSettingOrizin, transform.parent);
            childSetting.Init();
            childSetting.selectChild.selectAction += () => Init();
        }
        else
            childSetting.gameObject.SetActive(true);

    }
}
