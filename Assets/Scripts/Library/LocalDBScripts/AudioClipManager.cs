using System.Collections.Generic;
using UnityEngine;
using System;
using GJGameLibrary.DesignPattern;
using UnityEngine.AddressableAssets;
using System.Collections;

public class AudioClipManager : MonoSingleton<AudioClipManager>
{
    private Dictionary<string, AudioClip> clips = null;
    public override void Initialize()
    {
        clips = new Dictionary<string, AudioClip>();
    }
    public void GetClip(string key,Action<AudioClip> callback)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("Audio key is null.");
            callback?.Invoke(null);
        }
        else if (clips.ContainsKey(key))
        {
            var clip = clips[key];
            Debug.LogFormat("{0} : {1}", key, clip);
            callback?.Invoke(clip);
        }
        else
            StartCoroutine(LoadClip(key, callback));

        

    }
    private IEnumerator LoadClip(string key, Action<AudioClip> callback)
    {
        var op = Addressables.LoadAssetAsync<AudioClip>(key);
        while (!op.IsDone) { yield return null; }
        if (op.Status==UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            Debug.LogFormat("{0} : {1} (NEW)", key, op.Result);
        }
        else
        {
            Debug.LogErrorFormat("{0} : NULL (Load failed)", key);
            clips.Add(key, op.Result);
        }
        callback?.Invoke(op.Result);
    }
}