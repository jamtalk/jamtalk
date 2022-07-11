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
        if (clips.ContainsKey(key))
            callback?.Invoke(clips[key]);
    }
    private IEnumerator LoadClip(string key, Action<AudioClip> callback)
    {
        var op = Addressables.LoadAssetAsync<AudioClip>(key);
        while (!op.IsDone) { yield return null; }
        if (op.Result != null)
            clips.Add(key, op.Result);
        callback?.Invoke(op.Result);
    }
}