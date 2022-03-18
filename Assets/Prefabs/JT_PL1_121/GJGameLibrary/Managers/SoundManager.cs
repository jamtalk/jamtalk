using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJGameLibrary.DesignPattern;
using UnityEngine;
namespace GJGameLibrary
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private AudioSource _audioBGM = null;
        private AudioSource audioBGM
        {
            get
            {
                if (_audioBGM == null)
                {
                    var bgm = new GameObject("BGM");
                    bgm.transform.SetParent(transform);
                    _audioBGM = bgm.AddComponent<AudioSource>();
                    _audioBGM.playOnAwake = false;
                    _audioBGM.loop = true;
                    _audioBGM.volume = isOnBGM ? .5f : 0f;
                }

                return _audioBGM;
            }
        }
        private AudioSource _audioClick = null;
        private AudioSource audioClick
        {
            get
            {
                if (_audioClick == null)
                {
                    var ui = new GameObject("UI");
                    ui.transform.SetParent(transform);
                    _audioClick = ui.AddComponent<AudioSource>();
                    _audioClick.playOnAwake = false;
                    _audioClick.loop = false;
                    _audioClick.volume = isOnUI ? .5f : 0f;
                }

                return _audioClick;
            }
        }

        private AudioSource _audioPop = null;
        private AudioSource audioPop
        {
            get
            {
                if(_audioPop == null)
                {
                    var pop = new GameObject("Popup");
                    pop.transform.SetParent(transform);
                    _audioPop = pop.AddComponent<AudioSource>();
                    _audioPop.playOnAwake = false;
                    _audioPop.loop = false;
                    _audioPop.volume = isOnUI ? .7f : 0f;

                }

                return _audioPop;
            }
        }

        private AudioClip clickClip;
        private AudioClip popClip;
        private const string BGM = "BGM";
        private const string UI = "UI";
        private const string TTS = "TTS";

        public bool isOnBGM
        {
            get
            {
                if (!PlayerPrefs.HasKey(BGM))
                {
                    PlayerPrefs.SetString(BGM, true.ToString());
                    PlayerPrefs.Save();
                }
                return bool.Parse(PlayerPrefs.GetString(BGM));
            }
            private set
            {
                PlayerPrefs.SetString(BGM, value.ToString());
                PlayerPrefs.Save();
            }
        }

        public bool isOnUI
        {
            get
            {
                if (!PlayerPrefs.HasKey(UI))
                {
                    PlayerPrefs.SetString(UI, true.ToString());
                    PlayerPrefs.Save();
                }

                return bool.Parse(PlayerPrefs.GetString(UI));
            }
            private set
            {
                PlayerPrefs.SetString(UI, value.ToString());
                PlayerPrefs.Save();
            }
        }

        public bool isOnTTS
        {
            get
            {
                if (!PlayerPrefs.HasKey(TTS))
                {
                    PlayerPrefs.SetString(TTS, true.ToString());
                    PlayerPrefs.Save();
                }

                return bool.Parse(PlayerPrefs.GetString(TTS));
            }
            private set
            {
                PlayerPrefs.SetString(TTS, value.ToString());
                PlayerPrefs.Save();
            }
        }

        public override void Initialize()
        {

            audioPop.clip = Resources.Load<AudioClip>("Audio/Popup");
            Debug.LogFormat("팝업 클립 설정 : {0}", audioPop.clip.name);
            audioClick.clip = Resources.Load<AudioClip>("Audio/Click");
            Debug.LogFormat("클릭 클립 설정 : {0}", audioClick.clip.name);

            base.Initialize();
        }

        public void SetBGMClip(AudioClip clip)
        {
            if (audioBGM.isPlaying)
                audioBGM.Stop();

            audioBGM.clip = clip;

            if (isOnBGM && clip != null)
            {
                audioBGM.Play();
            }
        }

        public void SetBGM(bool isOn)
        {
            isOnBGM = isOn;
            audioBGM.volume = isOn ? .5f : 0f;
            if (isOn && !audioBGM.isPlaying)
            {
                audioBGM.Play();
            }
        }
        public void SetUI(bool isOn)
        {
            isOnUI = isOn;
            audioClick.volume = isOn ? .5f : 0f;
        }
        public void SetTTS(bool isOn)
        {
            isOnTTS = isOn;
        }
        public void PlayPopup()
        {
            if (audioPop.isPlaying)
                audioPop.Stop();

            if(isOnUI)
                audioPop.Play();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && isOnUI)
            {
                if (audioClick.isPlaying)
                    audioClick.Stop();

                if (isOnUI)
                    audioClick.Play();
            }
        }
    }
}
