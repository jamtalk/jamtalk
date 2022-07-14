using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SoundTest : MonoBehaviour
{
    private const string target = "PL1-ACT-01-01";
    public AudioSource source;
    public Button buttonLoad;
    public Button[] buttonAtalas;
    public Dropdown drop;
    public ResourceSchema schema;
    void Start()
    {
        Debug.Log("시작");
        GameManager.Instance.Initialize(() =>
        {
            Debug.Log("완료");
            var words = GameManager.Instance.alphabets
                .Take(buttonAtalas.Length)
                .Select(x => GameManager.Instance.GetResources(x).Words.First())
                .ToArray();
            Debug.Log(string.Join("\n", words.Select(x => string.Format("{0} : {1}", x.key, x.sprite))));
            for (int i = 0; i < buttonAtalas.Length; i++)
            {
                var image = buttonAtalas[i].GetComponent<Image>();
                image.sprite = words[i].sprite;
            }
                //AddListener(buttonAtalas[i], words[i]);

        });
    }
    public void AddListener(Button button, AlphabetWordsData word)
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log(word.sprite);
            button.image.sprite = word.sprite;
        });
    }
    IEnumerator load()
    {
        var op = Addressables.LoadAssetAsync<ResourceSchema>("ResourceSchema");
        var used = ToGB(Profiler.GetTotalAllocatedMemoryLong());
        Debug.LogFormat("로딩시작 ({0}GB 사용중)", used.ToString("N2"));
        while (!op.IsDone)
        {
            var _used = ToGB(Profiler.GetTotalAllocatedMemoryLong()); 
            if (used < _used)
            {
                used = _used;
                Debug.LogFormat("로딩중 최대 사용량 갱신({0}GB)", used);
            }
            yield return null;
        }
        Debug.LogFormat("@@@Status@@@\n{0}\n@@@@@@@@@", op.Status);
        Debug.LogFormat("@@@Exception@@@\n{0}\n@@@@@@@@@",op.OperationException);
        Debug.Log("로딩 완료");
        schema = op.Result;
        Debug.Log(schema);
    }
    private float ToGB(float memory)
    {
        return Mathf.Round(memory / 1024f / 1024f / 1024f * 100f) / 100f;
    }
}
