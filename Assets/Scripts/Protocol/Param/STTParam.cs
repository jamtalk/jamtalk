﻿using System;
using System.Collections.Generic;
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
        form.AddBinaryData("file", file, "tmp.wav");
        return form;
    }

    private byte[] ConvertFile(AudioClip clip)
    {

		var path = Path.Combine(Application.streamingAssetsPath + "/TMP/", "Recording.wav");
		var file = WavUtility.FromAudioClip(clip,out path,saveAsFile:false);
		//Save(path, clip);
		//ExportClipData(clip,path);
        //var file = File.ReadAllBytes(path);
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
	const int HEADER_SIZE = 44;

	private bool Save(string filename, AudioClip clip)
	{
		if (!filename.ToLower().EndsWith(".wav"))
		{
			filename += ".wav";
		}

		var filepath = Path.Combine(Application.persistentDataPath, filename);

		Debug.Log(filepath);

		// Make sure directory exists if user is saving to sub dir.
		Directory.CreateDirectory(Path.GetDirectoryName(filepath));

		using (var fileStream = CreateEmpty(filepath))
		{

			ConvertAndWrite(fileStream, clip);

			WriteHeader(fileStream, clip);
		}

		return true; // TODO: return false if there's a failure saving the file
	}

	private AudioClip TrimSilence(AudioClip clip, float min)
	{
		var samples = new float[clip.samples];

		clip.GetData(samples, 0);

		return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency);
	}

	private AudioClip TrimSilence(List<float> samples, float min, int channels, int hz)
	{
		return TrimSilence(samples, min, channels, hz, false, false);
	}

	private AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool _3D, bool stream)
	{
		int i;

		for (i = 0; i < samples.Count; i++)
		{
			if (Mathf.Abs(samples[i]) > min)
			{
				break;
			}
		}

		samples.RemoveRange(0, i);

		for (i = samples.Count - 1; i > 0; i--)
		{
			if (Mathf.Abs(samples[i]) > min)
			{
				break;
			}
		}

		samples.RemoveRange(i, samples.Count - i);

		var clip = AudioClip.Create("TempClip", samples.Count, channels, hz, _3D, stream);

		clip.SetData(samples.ToArray(), 0);

		return clip;
	}

	private FileStream CreateEmpty(string filepath)
	{
		var fileStream = new FileStream(filepath, FileMode.Create);
		byte emptyByte = new byte();

		for (int i = 0; i < HEADER_SIZE; i++) //preparing the header
		{
			fileStream.WriteByte(emptyByte);
		}

		return fileStream;
	}

	private void ConvertAndWrite(FileStream fileStream, AudioClip clip)
	{

		var samples = new float[clip.samples];

		clip.GetData(samples, 0);

		Int16[] intData = new Int16[samples.Length];
		//converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

		Byte[] bytesData = new Byte[samples.Length * 2];
		//bytesData array is twice the size of
		//dataSource array because a float converted in Int16 is 2 bytes.

		int rescaleFactor = 32767; //to convert float to Int16

		for (int i = 0; i < samples.Length; i++)
		{
			intData[i] = (short)(samples[i] * rescaleFactor);
			Byte[] byteArr = new Byte[2];
			byteArr = BitConverter.GetBytes(intData[i]);
			byteArr.CopyTo(bytesData, i * 2);
		}

		fileStream.Write(bytesData, 0, bytesData.Length);
	}

	private void WriteHeader(FileStream fileStream, AudioClip clip)
	{

		var hz = clip.frequency;
		var channels = clip.channels;
		var samples = clip.samples;

		fileStream.Seek(0, SeekOrigin.Begin);

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
		fileStream.Write(riff, 0, 4);

		Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
		fileStream.Write(chunkSize, 0, 4);

		Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
		fileStream.Write(wave, 0, 4);

		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
		fileStream.Write(fmt, 0, 4);

		Byte[] subChunk1 = BitConverter.GetBytes(16);
		fileStream.Write(subChunk1, 0, 4);

		UInt16 two = 2;
		UInt16 one = 1;

		Byte[] audioFormat = BitConverter.GetBytes(one);
		fileStream.Write(audioFormat, 0, 2);

		Byte[] numChannels = BitConverter.GetBytes(channels);
		fileStream.Write(numChannels, 0, 2);

		Byte[] sampleRate = BitConverter.GetBytes(hz);
		fileStream.Write(sampleRate, 0, 4);

		Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
		fileStream.Write(byteRate, 0, 4);

		UInt16 blockAlign = (ushort)(channels * 2);
		fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

		UInt16 bps = 16;
		Byte[] bitsPerSample = BitConverter.GetBytes(bps);
		fileStream.Write(bitsPerSample, 0, 2);

		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
		fileStream.Write(datastring, 0, 4);

		Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
		fileStream.Write(subChunk2, 0, 4);

		//		fileStream.Close();
	}
}

/// <summary>
/// WAV utility for recording and audio playback functions in Unity.
/// Version: 1.0 alpha 1
///
/// - Use "ToAudioClip" method for loading wav file / bytes.
/// Loads .wav (PCM uncompressed) files at 8,16,24 and 32 bits and converts data to Unity's AudioClip.
///
/// - Use "FromAudioClip" method for saving wav file / bytes.
/// Converts an AudioClip's float data into wav byte array at 16 bit.
/// </summary>
/// <remarks>
/// For documentation and usage examples: https://github.com/deadlyfingers/UnityWav
/// </remarks>

public class WavUtility
{
	// Force save as 16-bit .wav
	const int BlockSize_16Bit = 2;

	/// <summary>
	/// Load PCM format *.wav audio file (using Unity's Application data path) and convert to AudioClip.
	/// </summary>
	/// <returns>The AudioClip.</returns>
	/// <param name="filePath">Local file path to .wav file</param>
	public static AudioClip ToAudioClip(string filePath)
	{
		if (!filePath.StartsWith(Application.persistentDataPath) && !filePath.StartsWith(Application.dataPath))
		{
			Debug.LogWarning("This only supports files that are stored using Unity's Application data path. \nTo load bundled resources use 'Resources.Load(\"filename\") typeof(AudioClip)' method. \nhttps://docs.unity3d.com/ScriptReference/Resources.Load.html");
			return null;
		}
		byte[] fileBytes = File.ReadAllBytes(filePath);
		return ToAudioClip(fileBytes, 0);
	}

	public static AudioClip ToAudioClip(byte[] fileBytes, int offsetSamples = 0, string name = "wav")
	{
		//string riff = Encoding.ASCII.GetString (fileBytes, 0, 4);
		//string wave = Encoding.ASCII.GetString (fileBytes, 8, 4);
		int subchunk1 = BitConverter.ToInt32(fileBytes, 16);
		UInt16 audioFormat = BitConverter.ToUInt16(fileBytes, 20);

		// NB: Only uncompressed PCM wav files are supported.
		string formatCode = FormatCode(audioFormat);
		Debug.AssertFormat(audioFormat == 1 || audioFormat == 65534, "Detected format code '{0}' {1}, but only PCM and WaveFormatExtensable uncompressed formats are currently supported.", audioFormat, formatCode);

		UInt16 channels = BitConverter.ToUInt16(fileBytes, 22);
		int sampleRate = BitConverter.ToInt32(fileBytes, 24);
		//int byteRate = BitConverter.ToInt32 (fileBytes, 28);
		//UInt16 blockAlign = BitConverter.ToUInt16 (fileBytes, 32);
		UInt16 bitDepth = BitConverter.ToUInt16(fileBytes, 34);

		int headerOffset = 16 + 4 + subchunk1 + 4;
		int subchunk2 = BitConverter.ToInt32(fileBytes, headerOffset);
		//Debug.LogFormat ("riff={0} wave={1} subchunk1={2} format={3} channels={4} sampleRate={5} byteRate={6} blockAlign={7} bitDepth={8} headerOffset={9} subchunk2={10} filesize={11}", riff, wave, subchunk1, formatCode, channels, sampleRate, byteRate, blockAlign, bitDepth, headerOffset, subchunk2, fileBytes.Length);

		float[] data;
		switch (bitDepth)
		{
			case 8:
				data = Convert8BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
				break;
			case 16:
				data = Convert16BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
				break;
			case 24:
				data = Convert24BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
				break;
			case 32:
				data = Convert32BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
				break;
			default:
				throw new Exception(bitDepth + " bit depth is not supported.");
		}

		AudioClip audioClip = AudioClip.Create(name, data.Length, (int)channels, sampleRate, false);
		audioClip.SetData(data, 0);
		return audioClip;
	}

	#region wav file bytes to Unity AudioClip conversion methods

	private static float[] Convert8BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
	{
		int wavSize = BitConverter.ToInt32(source, headerOffset);
		headerOffset += sizeof(int);
		Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 8-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

		float[] data = new float[wavSize];

		sbyte maxValue = sbyte.MaxValue;

		int i = 0;
		while (i < wavSize)
		{
			data[i] = (float)source[i] / maxValue;
			++i;
		}

		return data;
	}

	private static float[] Convert16BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
	{
		int wavSize = BitConverter.ToInt32(source, headerOffset);
		headerOffset += sizeof(int);
		Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 16-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

		int x = sizeof(Int16); // block size = 2
		int convertedSize = wavSize / x;

		float[] data = new float[convertedSize];

		Int16 maxValue = Int16.MaxValue;

		int offset = 0;
		int i = 0;
		while (i < convertedSize)
		{
			offset = i * x + headerOffset;
			data[i] = (float)BitConverter.ToInt16(source, offset) / maxValue;
			++i;
		}

		Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

		return data;
	}

	private static float[] Convert24BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
	{
		int wavSize = BitConverter.ToInt32(source, headerOffset);
		headerOffset += sizeof(int);
		Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 24-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

		int x = 3; // block size = 3
		int convertedSize = wavSize / x;

		int maxValue = Int32.MaxValue;

		float[] data = new float[convertedSize];

		byte[] block = new byte[sizeof(int)]; // using a 4 byte block for copying 3 bytes, then copy bytes with 1 offset

		int offset = 0;
		int i = 0;
		while (i < convertedSize)
		{
			offset = i * x + headerOffset;
			Buffer.BlockCopy(source, offset, block, 1, x);
			data[i] = (float)BitConverter.ToInt32(block, 0) / maxValue;
			++i;
		}

		Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

		return data;
	}

	private static float[] Convert32BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
	{
		int wavSize = BitConverter.ToInt32(source, headerOffset);
		headerOffset += sizeof(int);
		Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 32-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

		int x = sizeof(float); //  block size = 4
		int convertedSize = wavSize / x;

		Int32 maxValue = Int32.MaxValue;

		float[] data = new float[convertedSize];

		int offset = 0;
		int i = 0;
		while (i < convertedSize)
		{
			offset = i * x + headerOffset;
			data[i] = (float)BitConverter.ToInt32(source, offset) / maxValue;
			++i;
		}

		Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

		return data;
	}

	#endregion

	public static byte[] FromAudioClip(AudioClip audioClip)
	{
		string file;
		return FromAudioClip(audioClip, out file, false);
	}

	public static byte[] FromAudioClip(AudioClip audioClip, out string filepath, bool saveAsFile = true, string dirname = "recordings")
	{
		MemoryStream stream = new MemoryStream();

		const int headerSize = 44;

		// get bit depth
		UInt16 bitDepth = 16; //BitDepth (audioClip);

		// NB: Only supports 16 bit
		//Debug.AssertFormat (bitDepth == 16, "Only converting 16 bit is currently supported. The audio clip data is {0} bit.", bitDepth);

		// total file size = 44 bytes for header format and audioClip.samples * factor due to float to Int16 / sbyte conversion
		int fileSize = audioClip.samples * BlockSize_16Bit + headerSize; // BlockSize (bitDepth)

		// chunk descriptor (riff)
		WriteFileHeader(ref stream, fileSize);
		// file header (fmt)
		WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
		// data chunks (data)
		WriteFileData(ref stream, audioClip, bitDepth);

		byte[] bytes = stream.ToArray();

		// Validate total bytes
		Debug.AssertFormat(bytes.Length == fileSize, "Unexpected AudioClip to wav format byte count: {0} == {1}", bytes.Length, fileSize);

		// Save file to persistant storage location
		if (saveAsFile)
		{
			filepath = string.Format("{0}/{1}/{2}.{3}", Application.persistentDataPath, dirname, DateTime.UtcNow.ToString("yyMMdd-HHmmss-fff"), "wav");
			Directory.CreateDirectory(Path.GetDirectoryName(filepath));
			File.WriteAllBytes(filepath, bytes);
			//Debug.Log ("Auto-saved .wav file: " + filepath);
		}
		else
		{
			filepath = null;
		}

		stream.Dispose();

		return bytes;
	}

	#region write .wav file functions

	private static int WriteFileHeader(ref MemoryStream stream, int fileSize)
	{
		int count = 0;
		int total = 12;

		// riff chunk id
		byte[] riff = Encoding.ASCII.GetBytes("RIFF");
		count += WriteBytesToMemoryStream(ref stream, riff, "ID");

		// riff chunk size
		int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(chunkSize), "CHUNK_SIZE");

		byte[] wave = Encoding.ASCII.GetBytes("WAVE");
		count += WriteBytesToMemoryStream(ref stream, wave, "FORMAT");

		// Validate header
		Debug.AssertFormat(count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);

		return count;
	}

	private static int WriteFileFormat(ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
	{
		int count = 0;
		int total = 24;

		byte[] id = Encoding.ASCII.GetBytes("fmt ");
		count += WriteBytesToMemoryStream(ref stream, id, "FMT_ID");

		int subchunk1Size = 16; // 24 - 8
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk1Size), "SUBCHUNK_SIZE");

		UInt16 audioFormat = 1;
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(audioFormat), "AUDIO_FORMAT");

		UInt16 numChannels = Convert.ToUInt16(channels);
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(numChannels), "CHANNELS");

		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(sampleRate), "SAMPLE_RATE");

		int byteRate = sampleRate * channels * BytesPerSample(bitDepth);
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(byteRate), "BYTE_RATE");

		UInt16 blockAlign = Convert.ToUInt16(channels * BytesPerSample(bitDepth));
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(blockAlign), "BLOCK_ALIGN");

		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(bitDepth), "BITS_PER_SAMPLE");

		// Validate format
		Debug.AssertFormat(count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);

		return count;
	}

	private static int WriteFileData(ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
	{
		int count = 0;
		int total = 8;

		// Copy float[] data from AudioClip
		float[] data = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(data, 0);

		byte[] bytes = ConvertAudioClipDataToInt16ByteArray(data);

		byte[] id = Encoding.ASCII.GetBytes("data");
		count += WriteBytesToMemoryStream(ref stream, id, "DATA_ID");

		int subchunk2Size = Convert.ToInt32(audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk2Size), "SAMPLES");

		// Validate header
		Debug.AssertFormat(count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);

		// Write bytes to stream
		count += WriteBytesToMemoryStream(ref stream, bytes, "DATA");

		// Validate audio data
		Debug.AssertFormat(bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

		return count;
	}

	private static byte[] ConvertAudioClipDataToInt16ByteArray(float[] data)
	{
		MemoryStream dataStream = new MemoryStream();

		int x = sizeof(Int16);

		Int16 maxValue = Int16.MaxValue;

		int i = 0;
		while (i < data.Length)
		{
			dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(data[i] * maxValue)), 0, x);
			++i;
		}
		byte[] bytes = dataStream.ToArray();

		// Validate converted bytes
		Debug.AssertFormat(data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);

		dataStream.Dispose();

		return bytes;
	}

	private static int WriteBytesToMemoryStream(ref MemoryStream stream, byte[] bytes, string tag = "")
	{
		int count = bytes.Length;
		stream.Write(bytes, 0, count);
		//Debug.LogFormat ("WAV:{0} wrote {1} bytes.", tag, count);
		return count;
	}

	#endregion

	/// <summary>
	/// Calculates the bit depth of an AudioClip
	/// </summary>
	/// <returns>The bit depth. Should be 8 or 16 or 32 bit.</returns>
	/// <param name="audioClip">Audio clip.</param>
	public static UInt16 BitDepth(AudioClip audioClip)
	{
		UInt16 bitDepth = Convert.ToUInt16(audioClip.samples * audioClip.channels * audioClip.length / audioClip.frequency);
		Debug.AssertFormat(bitDepth == 8 || bitDepth == 16 || bitDepth == 32, "Unexpected AudioClip bit depth: {0}. Expected 8 or 16 or 32 bit.", bitDepth);
		return bitDepth;
	}

	private static int BytesPerSample(UInt16 bitDepth)
	{
		return bitDepth / 8;
	}

	private static int BlockSize(UInt16 bitDepth)
	{
		switch (bitDepth)
		{
			case 32:
				return sizeof(Int32); // 32-bit -> 4 bytes (Int32)
			case 16:
				return sizeof(Int16); // 16-bit -> 2 bytes (Int16)
			case 8:
				return sizeof(sbyte); // 8-bit -> 1 byte (sbyte)
			default:
				throw new Exception(bitDepth + " bit depth is not supported.");
		}
	}

	private static string FormatCode(UInt16 code)
	{
		switch (code)
		{
			case 1:
				return "PCM";
			case 2:
				return "ADPCM";
			case 3:
				return "IEEE";
			case 7:
				return "μ-law";
			case 65534:
				return "WaveFormatExtensable";
			default:
				Debug.LogWarning("Unknown wav code format:" + code);
				return "";
		}
	}

}