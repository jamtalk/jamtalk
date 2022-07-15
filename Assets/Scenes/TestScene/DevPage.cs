using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DevPage : MonoBehaviour
{
    public Image orizinal;
    public AudioSource audios;
    public RectTransform parent;
    void Start()
    {
        var sprites = GetAllSprites();
        for (int i = 0; i < sprites.Length; i++)
            orizinal.sprite = sprites[i];
        //var clips = GetAllClips();
        //for (int i = 0; i < clips.Length; i++)
        //    audios.clip = clips[i];
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
}
