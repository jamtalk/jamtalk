using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirePopup : MonoBehaviour
{
    public Button cancleButton;
    public Button confirmButton;

    private void Awake()
    {
        cancleButton.onClick.AddListener(() => gameObject.SetActive(false));
        confirmButton.onClick.AddListener(FireMember);
    }

    private void FireMember()
    {
        var param = new MemberOutParam();

        RequestManager.Instance.RequestAct(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            Debug.Log(result.code);
            if (result.code != eErrorCode.Success)
            {
                Debug.Log(result.code);
                AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
            }
            else
            {
                AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);

                if (PlayerPrefs.HasKey("ID")) PlayerPrefs.DeleteKey("ID");
                if (PlayerPrefs.HasKey("UID")) PlayerPrefs.DeleteKey("UID");
                if (PlayerPrefs.HasKey("PROVIDER")) PlayerPrefs.DeleteKey("PROVIDER");
                PlayerPrefs.Save();

                GameManager.Instance.SignOut();
            }
        });
    }
}
