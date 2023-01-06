using System.Collections;
using System.Collections.Generic;
using Kakaotalk;
using UnityEngine;

public class LoginTest : MonoBehaviour
{
    void Awake()
    {
        KakaoSdk.Initialize(() => {
            KakaoSdk.Login(LoginMethod.Both, (token) => {
                Debug.Log(JsonUtility.ToJson(token));
                KakaoSdk.GetProfile((profile) => {
                    Debug.Log(JsonUtility.ToJson(profile));
                }, e => Debug.Log(e));
            }, e => Debug.Log(e));
        }, e => Debug.Log(e));
    }
}
