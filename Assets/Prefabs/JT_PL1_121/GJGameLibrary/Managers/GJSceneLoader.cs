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
            SceneLoadingPopup loading = null;
            if (withLoading)
            {
                loading = PopupManager.Instance.Popup<SceneLoadingPopup>
                    (Resources.Load<GameObject>(PopupManager.PopupSceneLoadingRecourcePath));

                foreach (var item in loading.charactors)
                    item.LoadingRoutine();
            }
            while (!op.isDone)
            {
                var progress = op.progress * 100f;
                if (loading != null)
                    loading.progressbarCharging(op.progress);
                yield return null;
            }
            PopupManager.Instance.Clear();
            op.allowSceneActivation = true;
            yield break;
        }
    }
}
