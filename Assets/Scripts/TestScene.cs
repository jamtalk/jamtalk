using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class TestScene : MonoBehaviour
{
    public GameObject original;
    public Transform parent;
    public eSceneName[] ignore;
    private void Awake()
    {
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
    }
    public void AddListener(Button button, eSceneName scene)
    {
        button.onClick.AddListener(() => GJGameLibrary.GJSceneLoader.Instance.LoadScene(scene));
        button.transform.GetChild(0).GetComponent<Text>().text = scene.ToString();
    }
}
