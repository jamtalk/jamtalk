using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GJGameLibrary.DesignPattern;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

namespace GJGameLibrary
{
    public class GJSceneLoader : MonoSingleton<GJSceneLoader>
    {
        private bool isAddressabelsInitialized = false;
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
        public void LoadScene(eSceneName nextScene, bool withLoading = false) => StartCoroutine(LoadSceneAsync(nextScene, withLoading));
        IEnumerator LoadSceneAsync(eSceneName scene, bool withLoading = false)
        {
            currentScene = scene;
            var op = SceneManager.LoadSceneAsync(scene.ToString());
            SceneLoadingPopup loading = null;
            if (withLoading)
            {
                loading = Instantiate(Resources.Load<SceneLoadingPopup>(PopupManager.PopupSceneLoadingRecourcePath));
                DontDestroyOnLoad(loading.gameObject);
                yield return new WaitForEndOfFrame();
                foreach (var item in loading.charactors)
                    item.LoadingRoutine();
            }
            StartCoroutine(WaitAddressables(() => isAddressabelsInitialized = true));
            loading.progressbarCharging(0f);
            var time = 0f;
            var maxTime = 1.5f;
            var targetTime = withLoading ? maxTime / 2f : maxTime;
            while (!op.isDone || time< targetTime  || !isAddressabelsInitialized)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
                if (time > targetTime)
                    time = targetTime;
                if(withLoading)
                    loading.progressbarCharging(time/maxTime);
            }
            op.allowSceneActivation = true;
            if (withLoading)
            {
                while (time < maxTime)
                {
                    yield return new WaitForFixedUpdate();
                    if (SceneLoadingPopup.SpriteLoader != null && SceneLoadingPopup.SpriteLoader.Count > 0)
                    {
                        var loaders = SceneLoadingPopup.SpriteLoader;
                        for (int i = 0; i < loaders.Count; i++)
                        {
                            Debug.LogFormat("{0}/{1} 이미지 로딩중 (초)",i+1,loaders.Count);
                            var now = DateTime.Now;
                            yield return loaders[i];
                            Debug.LogFormat("{0}/{1} 이미지 로딩완료({2}초)", i + 1, loaders.Count, (DateTime.Now - now).TotalSeconds);
                            time += (targetTime / (float)loaders.Count) * (i + 1);
                            if (time > maxTime)
                                time = maxTime;
                            loading.progressbarCharging(time / maxTime);
                        }
                        SceneLoadingPopup.SpriteLoader.Clear();
                    }
                    else
                    {
                        time += Time.fixedDeltaTime;
                        if (time > maxTime)
                            time = maxTime;
                    }

                    loading.progressbarCharging(time / maxTime);
                }
            }

            SceneLoadingPopup.onLoaded?.Invoke();
            SceneLoadingPopup.onLoaded = null;

            if (withLoading)
                Destroy(loading.gameObject);
            Debug.LogFormat("[::{0}::] 씬 로딩 완료\n로딩화면 여부 : {1}", SceneManager.GetActiveScene().name, withLoading);
            PopupManager.Instance.Clear();
            yield break;
        }

        IEnumerator WaitAddressables(Action callback)
        {
            if (!isAddressabelsInitialized)
            {
                yield return Addressables.InitializeAsync();
                yield return new AlphabetData(eAlphabet.A).Words.First().SpriteAsync;
                isAddressabelsInitialized = true;
            }
            callback?.Invoke();
        }
    }
}
