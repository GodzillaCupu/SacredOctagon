using System;
using UnityEngine;
using DGE.Audio.Component;
using DGE.Utils.core;

namespace DGE.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] public AudioSourceHandler sourceHandler = new AudioSourceHandler();
        [SerializeField] ListAudioClip_SO audioClipsData;

        [SerializeField] ClipData currentBGMClip;
        [SerializeField] ClipData currentSFXClip;
        [SerializeField] ClipData currentVOClip;

        void Start()
        {
            sourceHandler.Initialization();
        }

        public void Play(AudioType_Enum _audioType, string _id,bool _isLooping = false)
        {
            switch (_audioType)
            {
                case AudioType_Enum.BGM:
                    PlayBGM();
                    break;

                case AudioType_Enum.SFX:
                    PlaySFX(_id, _isLooping);
                    break;

                case AudioType_Enum.VO:
                    PlayVO(_id);
                    break;
            }
        }

        public void PlayBGM()
        {
            AudioSource _targetSource = sourceHandler.source.BGM;
            currentBGMClip = audioClipsData.GetCurrentBGMData;

            _targetSource.loop = true;
            _targetSource.playOnAwake = true;
            _targetSource.clip = currentBGMClip.clip;
            _targetSource.Play();
        }

        public void PlaySFX(string _id, bool _isLooping = true)
        {
            AudioSource _targetSource = sourceHandler.source.SFX;
            AudioClip _targetClip = audioClipsData.GetAudioClip(AudioType_Enum.SFX, _id);
            currentSFXClip = audioClipsData.GetCurrentSFXData;

            _targetSource.loop = _isLooping;
            _targetSource.playOnAwake = false;
            _targetSource.clip = _targetClip;
            _targetSource.Play();
        }

        public void PlayVO(string _id)
        {
            AudioSource _targetSource = sourceHandler.source.VO;
            AudioClip _targetClip = audioClipsData.GetAudioClip(AudioType_Enum.VO, _id);
            currentVOClip = audioClipsData.GetCurrentVOData;

            _targetSource.loop = false;
            _targetSource.playOnAwake = false;
            _targetSource.PlayOneShot(_targetClip);
        }

        public void Stop(AudioType_Enum _type)
        {
            switch (_type)
            {
                case AudioType_Enum.BGM:
                    StopBGM();
                    break;
                case AudioType_Enum.SFX:
                    StopSFX(false);
                    break;
            }
        }

        public void StopBGM() => sourceHandler.source.BGM.Stop();
        public void StopSFX() => sourceHandler.source.SFX.Stop();
        public void StopSFX(bool _isLooping = false)
        {
            AudioSource _targetSource = sourceHandler.source.SFX;
            _targetSource.loop = _isLooping;
            _targetSource.Stop();
        }


        public void SetVolumeEffect(float _targetVolume)
        {
            // Convert linear slider value to logarithmic decibel value
            // Ensure sliderValue is not zero to prevent issues with Log10
            float mixerVolume = Mathf.Log10(Mathf.Max(0.0001f, _targetVolume)) * 20;
            sourceHandler.mixerGroup.SFX.audioMixer.SetFloat("Audio-Volume-SFX", mixerVolume);
        }
        public void SetVolumeBGM(float _targetVolume)
        {
            // Convert linear slider value to logarithmic decibel value
            // Ensure sliderValue is not zero to prevent issues with Log10
            float mixerVolume = Mathf.Log10(Mathf.Max(0.0001f, _targetVolume)) * 20;
            sourceHandler.mixerGroup.BGM.audioMixer.SetFloat("Audio-Volume-BGM", mixerVolume);
        }

        public AudioClip GetAudioClip(AudioType_Enum _audioType)
        {
            switch (_audioType)
            {
                case AudioType_Enum.BGM:
                    return currentBGMClip.clip;
                case AudioType_Enum.SFX:
                    return currentSFXClip.clip;
                case AudioType_Enum.VO:
                    return currentVOClip.clip;
                default:
                    return null;
            }
        }
    }
}
