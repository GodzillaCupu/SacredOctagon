namespace DGE.Audio
{
    using System;
    using UnityEngine;
    using DGE.Audio.Component;
    using DGE.Utils.core;

    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioSourceHandler sourceHandler = new AudioSourceHandler();
        [SerializeField] ListAudioClip_SO audioClipsData;

        [SerializeField] ClipData currentBGMClip;
        [SerializeField] ClipData currentSFXClip;
        [SerializeField] ClipData currentVOClip;

        void Start()
        {
            sourceHandler.Initialization();
        }

        public void Play(AudioType_Enum _audioType,string _id)
        {
            switch (_audioType)
            {
                case AudioType_Enum.BGM:
                    PlayBGM();
                    break;

                case AudioType_Enum.SFX:
                    PlaySFX(_id);
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

        public void PlaySFX(string _id)
        {
            AudioSource _targetSource = sourceHandler.source.SFX;
            AudioClip _targetClip = audioClipsData.GetAudioClipSFX(_id);
            currentSFXClip = audioClipsData.GetCurrentSFXData;

            _targetSource.loop = false;
            _targetSource.playOnAwake = false;
            _targetSource.PlayOneShot(_targetClip);
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

    }
}
