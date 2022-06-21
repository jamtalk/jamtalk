using GJGameLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using OptionData = UnityEngine.UI.Dropdown.OptionData;
public class TestScene : MonoBehaviour
{
    public Dropdown drop;
    public GameObject original;
    public Transform parent;
    public eSceneName[] ignore;
    private void Awake()
    {
        //StartCoroutine(Tmp());
        //return;
        AndroidPluginManager.Instance.PlayTTS("Question!!");
        var scenes = Enum.GetNames(typeof(eSceneName))
            .Select(x => (eSceneName)Enum.Parse(typeof(eSceneName), x))
            .Where(x=>!ignore.Contains(x))
            .ToArray();
        for(int i = 0;i < scenes.Length; i++)
        {
            var button = Instantiate(original, parent).GetComponent<Button>();
            AddListener(button, scenes[i]);

        }
        original.gameObject.SetActive(false);

        var options = new List<OptionData>();
        var enums = Enum.GetNames(typeof(eAlphabet))
            .Select(x => (eAlphabet)Enum.Parse(typeof(eAlphabet), x))
            .ToArray();
        for(int i = 0;i < enums.Length/2; i++)
        {
            var first = i * 2;
            var next = first + 1;
            Debug.LogFormat("{0}~{1} / {2}", first, next, enums.Length);
            options.Add(new OptionData(string.Format("{0} ~ {1}", enums[first], enums[next])));
        }
        drop.options = options;
        drop.onValueChanged.AddListener((value) =>
        {
            var alphabet = (eAlphabet)(value * 2);
            GameManager.Instance.currentAlphabet = alphabet;
            Debug.Log(alphabet);
        });
    }
    public void AddListener(Button button, eSceneName scene)
    {
        button.onClick.AddListener(() => GJGameLibrary.GJSceneLoader.Instance.LoadScene(scene));
        button.transform.GetChild(0).GetComponent<Text>().text = scene.ToString();
    }
    //IEnumerator Tmp()
    //{
    //    yield return null;
    //    var root = new DirectoryInfo(@"D:\Project\Jamtalk\Assets\Sprites\DoublePhanics");
    //    var files = root.GetDirectories()
    //        .SelectMany(x => x.GetDirectories())
    //        .SelectMany(x => x.GetFiles())
    //        .Where(x => x.Extension == ".png")
    //        .Select(x => string.Format("{0},{1},{2}", x.Directory.Parent.Name.Replace("level",""), x.Directory.Name, x.Name.Replace(x.Extension, "")));
    //    Debug.Log(string.Join("\n",files));
    //}
}
