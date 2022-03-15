using System;
using System.IO;
using System.Text;
using UnityEngine;

public class STTParam : Param
{
    private const string id = @"makevofficialb9e11cae5648";
    private const string key = @"3f0b8e06928f41ad92870686059468e3";
    private const string model = @"baseline_eng_8k_default";
    public byte[] file;

    public STTParam(AudioClip clip)
    {
        file = ConvertFile(clip);
    }

    public override WWWForm GetForm()
    {
        var form = new WWWForm();
        form.AddField("apiId",id);
        form.AddField("apiKey", key);
        form.AddField("model", model);
        form.AddBinaryData("file", file, "tmp.wav", "audio/wav");
        return form;
    }

    private byte[] ConvertFile(AudioClip clip)
    {
        var path = Path.Combine(Application.streamingAssetsPath + "/TMP/", "Recording.wav"); ;
        ExportClipData(clip,path);
        var file = File.ReadAllBytes(path);
        Debug.Log(file.Length / 1024+"KB");
        return file;
    }
    private void ExportClipData(AudioClip clip, string path)
    {
        var data = new float[clip.samples * clip.channels];
        clip.GetData(data, 0);
        if (File.Exists(path))
            File.Delete(path);
        using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
        {
            // The following values are based on http://soundfile.sapp.org/doc/WaveFormat/
            var bitsPerSample = (ushort)16;
            var chunkID = "RIFF";
            var format = "WAVE";
            var subChunk1ID = "fmt ";
            var subChunk1Size = (uint)16;
            var audioFormat = (ushort)1;
            var numChannels = (ushort)clip.channels;
            var sampleRate = (uint)clip.frequency;
            var byteRate = (uint)(sampleRate * clip.channels * bitsPerSample / 8);  // SampleRate * NumChannels * BitsPerSample/8
            var blockAlign = (ushort)(numChannels * bitsPerSample / 8); // NumChannels * BitsPerSample/8
            var subChunk2ID = "data";
            var subChunk2Size = (uint)(data.Length * clip.channels * bitsPerSample / 8); // NumSamples * NumChannels * BitsPerSample/8
            var chunkSize = (uint)(36 + subChunk2Size); // 36 + SubChunk2Size
                                                        // Start writing the file.
            WriteString(stream, chunkID);
            WriteInteger(stream, chunkSize);
            WriteString(stream, format);
            WriteString(stream, subChunk1ID);
            WriteInteger(stream, subChunk1Size);
            WriteShort(stream, audioFormat);
            WriteShort(stream, numChannels);
            WriteInteger(stream, sampleRate);
            WriteInteger(stream, byteRate);
            WriteShort(stream, blockAlign);
            WriteShort(stream, bitsPerSample);
            WriteString(stream, subChunk2ID);
            WriteInteger(stream, subChunk2Size);
            foreach (var sample in data)
            {
                // De-normalize the samples to 16 bits.
                var deNormalizedSample = (short)0;
                if (sample > 0)
                {
                    var temp = sample * short.MaxValue;
                    if (temp > short.MaxValue)
                        temp = short.MaxValue;
                    deNormalizedSample = (short)temp;
                }
                if (sample < 0)
                {
                    var temp = sample * (-short.MinValue);
                    if (temp < short.MinValue)
                        temp = short.MinValue;
                    deNormalizedSample = (short)temp;
                }
                WriteShort(stream, (ushort)deNormalizedSample);
            }
        }
    }

    void WriteString(Stream stream, string value)
    {
        foreach (var character in value)
            stream.WriteByte((byte)character);
    }

    void WriteInteger(Stream stream, uint value)
    {
        stream.WriteByte((byte)(value & 0xFF));
        stream.WriteByte((byte)((value >> 8) & 0xFF));
        stream.WriteByte((byte)((value >> 16) & 0xFF));
        stream.WriteByte((byte)((value >> 24) & 0xFF));
    }

    void WriteShort(Stream stream, ushort value)
    {
        stream.WriteByte((byte)(value & 0xFF));
        stream.WriteByte((byte)((value >> 8) & 0xFF));
    }
}
