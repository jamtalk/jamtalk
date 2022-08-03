using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DevPage : MonoBehaviour
{
    public Button buttonRecord;
    public Button buttonPlay;
    public Button buttonStop;
    public Button buttonPause;
    public VoiceRecorder recorder;
    public bool recording = false;
    void Start()
    {
        Debug.Log("마이크 목록---\n" + string.Join("\n", Microphone.devices) + "\n-----");
        STTManager.Instance.onEnded += () =>
        {
            recording = false;
            recorder.Stop();
        };
        STTManager.Instance.onError += (error) =>
        {
            Debug.Log(error);
        };
        //STTManager.Instance.onStarted += () => recorder.Record();
        STTManager.Instance.onResult += (value) => Debug.LogFormat("결과 : {0}", value);
        buttonRecord.onClick.AddListener(() =>
        {
            recorder.Record();
            STTManager.Instance.StartSTT("en-US");
            //if (recording)
            //    recorder.Stop();
            //else
            //    recorder.Record();
            //recording = !recording;
        });
        buttonPlay.onClick.AddListener(recorder.source.Play);
        buttonStop.onClick.AddListener(recorder.source.Stop);
        buttonPause.onClick.AddListener(recorder.source.Pause);
    }
    private Sprite[] GetAllSprites()
    {
        var words = GameManager.Instance.alphabets
            .SelectMany(x => GameManager.Instance.GetResources(x).Words)
            .Select(x => x.sprite);
        var vowels = GameManager.Instance.vowels
            .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
            .Select(x => x.sprite);
        var digs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Select(x => x.sprite);
        return words.Union(vowels).Union(digs).ToArray();
    }
    private AudioClip[] GetAllClips()
    {
        var alphabets = GameManager.Instance.alphabets.Select(x => GameManager.Instance.schema.GetAlphabetAudio(x));
        var vowels = GameManager.Instance.vowels.Select(x => GameManager.Instance.schema.GetVowelAudio(x));
        var digs = GameManager.Instance.digrpahs.Select(x => GameManager.Instance.schema.GetDigrpahsAudio(x));

        var nulldigs = GameManager.Instance.digrpahs.Where(x => GameManager.Instance.schema.GetDigrpahsAudio(x) == null);
        Debug.Log("Null Digs\n" + string.Join("\n", nulldigs));

        var w_alphabet = GameManager.Instance.alphabets
            .SelectMany(x => GameManager.Instance.GetResources(x).Words);
        var w_vowels = GameManager.Instance.vowels
            .SelectMany(x => GameManager.Instance.GetResources(x).Vowels);
        var w_digs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x));

        Debug.LogFormat(
            "alphabets : {0}\n" +
            "vowels : {1}\n" +
            "digs : {2}\n"
            ,alphabets.Count(),vowels.Count(),digs.Count());

        var values = alphabets.Select(x => x.act1)
            .Union(alphabets.Select(x => x.act2))
            .Union(alphabets.Select(x => x.clip))
            .Union(alphabets.Select(x => x.phanics))
            .Union(w_alphabet.Select(x => x.act))
            .Union(w_alphabet.Select(x => x.clip))

            .Union(digs.Where(x=>x!=null).Select(x => x.phanics))
            .Union(w_digs.Select(x => x.act))
            .Union(w_digs.Select(x => x.clip))

            .Union(vowels.Select(x => x.clip))
            .Union(vowels.Select(x => x.act_long))
            .Union(vowels.Select(x => x.act_short))
            .Union(vowels.Select(x => x.phanics_long))
            .Union(vowels.Select(x => x.phanics_short))
            .Union(w_vowels.Select(x => x.act))
            .Union(w_vowels.Select(x => x.clip))

            .Distinct()
            .ToArray();
        Debug.LogFormat("총 {0}개 음성", values.Length);
        var nullValues = values.Where(x => Addressables.LoadAssetAsync<AudioClip>(x).WaitForCompletion()==null);
        Debug.LogFormat("{0}개 Null\n{1}", nullValues.Count(), string.Join("\n", nullValues));
        return values.Select(x => Addressables.LoadAssetAsync<AudioClip>(x).WaitForCompletion()).ToArray();
    }
    private void GetSiteWordsClips()
    {
        var sen = GameManager.Instance.alphabets
            .SelectMany(x => GameManager.Instance.GetResources(x).AlphabetSentances);
        var data = sen
            .SelectMany(x => x.words)
            .Select(x => GJGameLibrary.GJStringFormatter.OnlyEnglish(x))
            .Distinct();


        //문장엔 있으나, 음원파일이 없는 경우
        var tmp = data.Where(x => string.IsNullOrEmpty(GameManager.Instance.schema.GetSiteWordsClip(x)));

        //음원 목록엔 있으나, 파일이 없는 경우
        var _tmp = data.Where(x => !string.IsNullOrEmpty(GameManager.Instance.schema.GetSiteWordsClip(x)))
            .Select(x => GameManager.Instance.schema.GetSiteWordsClip(x))
            .Where(x => Addressables.LoadAssetAsync<AudioClip>(x).WaitForCompletion()==null).ToArray();

        //누락된 문장 음원
        var __tmp = sen.Where(x => Addressables.LoadAssetAsync<AudioClip>(x.clip).WaitForCompletion() == null).Select(x=>x.value);

        Debug.LogFormat("tmp {0}개\n{1}",tmp.Count(), string.Join("\n", tmp));
        Debug.LogFormat("_tmp {0}개\n{1}",_tmp.Count(), string.Join("\n", _tmp));
        Debug.LogFormat("__tmp {0}개\n{1}", __tmp.Count(), string.Join("\n", __tmp));

    }
    private void GetAllDigraphsSound()
    {
        var data = GameManager.Instance.schema.data.digraphsAudio.Select(x => x.phanics)
            .Union(GameManager.Instance.schema.data.digraphsWords.Select(x => x.clip))
            .Union(GameManager.Instance.schema.data.digraphsWords.Select(x => x.act))
            .Where(x => Addressables.LoadAssetAsync<AudioClip>(x).WaitForCompletion() == null)
            .OrderBy(x => x);
        Debug.LogFormat("총 {0}개\n{1}", data.Count(), string.Join("\n", data));
    }
}
