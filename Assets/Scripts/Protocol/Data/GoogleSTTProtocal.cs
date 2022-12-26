using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

public class GoogleSTTProtocal
{ 
    public GoogleSTTConfig config;
    public GoogleSTTAudio audio;
    public GoogleSTTProtocal(AudioClip clip)
    {
        config = new GoogleSTTConfig();
        var path = Path.Combine(Application.persistentDataPath + "/", "Recording.wav");
        Debug.Log(path);
        var file = WavUtility.FromAudioClip(clip, out path, saveAsFile: false);
        this.audio = new GoogleSTTAudio(file);
    }


    public byte[] ToJson()
    {
        var json = JsonConvert.SerializeObject(this);
        var bytes = Encoding.UTF8.GetBytes(json);
        var base64String = Convert.ToBase64String(bytes);
        Debug.Log(json);
        return Convert.FromBase64String(base64String);
    }
}

public class GoogleSTTConfig
{
    //public string encoding => "FLAC";
    //public int sampleRateHertz => 16000;
    public string languageCode;
    //public bool enableWordTimeOffsets => false;
}

public class GoogleSTTAudio
{
    public string content;
    public GoogleSTTAudio(byte[] bytes)
    {
        content = Convert.ToBase64String(bytes);
    }
}
