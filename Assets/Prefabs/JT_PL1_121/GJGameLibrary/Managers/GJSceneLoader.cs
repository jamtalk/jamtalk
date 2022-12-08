using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GJGameLibrary.DesignPattern;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GJGameLibrary
{
    public class GJSceneLoader : MonoSingleton<GJSceneLoader>
    {
        public eSceneName currentScene { get; private set; }
        private Dictionary<eSceneName, AudioClip> _bgm = null;
        private Dictionary<eSceneName, AudioClip> bgm
        {
            get
            {
                if (_bgm == null)
                {
                    _bgm = Enum.GetNames(typeof(eSceneName))
                        .Select(x => (eSceneName)Enum.Parse(typeof(eSceneName), x))
                        .ToDictionary(key => key, value => Resources.Load<AudioClip>("Audio/BGM/" + value.ToString()));
                }
                return _bgm;
            }
        }
        public void LoadScene(eSceneName nextScene, bool withLoading = false) => StartCoroutine(LoadSceneAsyc(nextScene, withLoading));
        IEnumerator LoadSceneAsyc(eSceneName scene, bool withLoading = false)
        {
            currentScene = scene;
            var op = SceneManager.LoadSceneAsync(scene.ToString());
            LoadingPopup loading = null;
            if (withLoading)
            {
                loading = PopupManager.Instance.Popup<LoadingPopup>
                    (Resources.Load<GameObject>(PopupManager.PopupLoadingRecourcePath));
            }
            while (!op.isDone)
            {
                var progress = op.progress * 100f;
                if (loading != null)
                    loading.progressbarCharging(op.progress);
                Debug.LogFormat("{0} 씬 로딩중.. ({1}%)", scene.ToString(), progress.ToString("N2"));
                yield return null;
            }
            Debug.Log("씬 로딩 완료");
            PopupManager.Instance.Clear();
            op.allowSceneActivation = true;
            yield break;
        }
    }
}
