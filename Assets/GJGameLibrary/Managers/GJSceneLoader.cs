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
        private Dictionary<eSceneName, AudioClip> _bgm = null;
        private Dictionary<eSceneName,AudioClip> bgm
        {
            get
            {
                if (_bgm==null)
                {
                    _bgm = Enum.GetNames(typeof(eSceneName))
                        .Select(x => (eSceneName)Enum.Parse(typeof(eSceneName), x))
                        .ToDictionary(key => key, value => Resources.Load<AudioClip>("Audio/BGM/" + value.ToString()));
                }
                return _bgm;
            }
        }
        public void LoadScene(eSceneName nextScene) => StartCoroutine(LoadSceneAsyc(nextScene));
        IEnumerator LoadSceneAsyc(eSceneName scene)
        {
            PopupManager.Instance.ShowLoading();
            AsyncOperation op = SceneManager.LoadSceneAsync(scene.ToString());
            op.allowSceneActivation = false;
            op.completed += Op_completed;
            while (!op.isDone) {
                if (op.progress < .9f)
                {
                    yield return null;
                }
                else
                {
                    op.allowSceneActivation = true;
                    Debug.Log(bgm[scene]);
                    SoundManager.Instance.SetBGMClip(bgm[scene]);
                    yield break;
                }
            }
        }

        private void Op_completed(AsyncOperation obj)
        {
            PopupManager.Instance.Clear();
        }
    }
}
