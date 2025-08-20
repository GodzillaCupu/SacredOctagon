namespace DGE.Audio.Component
{
    using System;
    using UnityEditor.Rendering;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.PlayerLoop;

    [Serializable]
    public class AudioSourceHandler
    {
        public Source source;
        public OutputAudioMixerGroup mixerGroup;

        public void Initialization()
        {
            CheckAudioSource();
        }

        void CheckAudioSource()
        {
            if (source.SFX != null && source.BGM != null && source.VO != null)
                CheckAudioSourceMixer();
        }

        void CheckAudioSourceMixer()
        {
            if (!CheckAudioMixerDataBGM()) SetAudioMixerDataBGM();
            if (!CheckAudioMixerDataSFX()) SetAudioMixerDataSFX();
            if (!CheckAudioMixerDataVO()) SetAudioMixerDataVO();
        }

        bool CheckAudioMixerData(AudioSource _source, AudioMixerGroup _outputMixerGroup) => _source.outputAudioMixerGroup == _outputMixerGroup ? true : false;

        bool CheckAudioMixerDataSFX() => CheckAudioMixerData(source.SFX, mixerGroup.SFX);
        bool CheckAudioMixerDataBGM() => CheckAudioMixerData(source.BGM, mixerGroup.BGM);
        bool CheckAudioMixerDataVO() => CheckAudioMixerData(source.VO, mixerGroup.VO);
        AudioSource SetAudioMixerData(AudioSource _source, AudioMixerGroup _outputMixerGroup)
        {
            _source.outputAudioMixerGroup = _outputMixerGroup;
            return _source;
        }

        AudioSource SetAudioMixerDataSFX() => SetAudioMixerData(source.SFX, mixerGroup.SFX);
        AudioSource SetAudioMixerDataBGM() => SetAudioMixerData(source.BGM, mixerGroup.BGM);
        AudioSource SetAudioMixerDataVO() => SetAudioMixerData(source.VO, mixerGroup.VO);
    }

    [Serializable]
    public class Source
    {
        public AudioSource SFX;
        public AudioSource BGM;
        public AudioSource VO;
    }

    [Serializable]
    public class OutputAudioMixerGroup
    {
        public AudioMixerGroup SFX;
        public AudioMixerGroup BGM;
        public AudioMixerGroup VO;
    }
}
