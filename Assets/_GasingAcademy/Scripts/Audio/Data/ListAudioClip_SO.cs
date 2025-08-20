namespace DGE.Audio.Component
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ListAudioData", menuName = "CustomData/ListAudioData")]
    public class ListAudioClip_SO : ScriptableObject
    {
        public ClipData BGM_ClipData;
        public List<ClipData> SFX_ClipData;
        public List<AudioVO> VO_ClipData;

        public ClipData GetCurrentSFXData;
        public ClipData GetCurrentBGMData;
        public ClipData GetCurrentVOData;

        public AudioClip GetAudioClip(AudioType_Enum _type, string _clipName = null, string _characterName = null, string _adegan = null)
        {
            switch (_type)
            {
                case AudioType_Enum.BGM:
                    return GetAudioClipBGM();
                case AudioType_Enum.SFX:
                    return GetAudioClipSFX(_clipName);
                case AudioType_Enum.VO:
                    return GetAudioClipVO(_characterName, _adegan, _clipName);
                default:
                    return GetAudioClipData(_clipName, BGM_ClipData);
            }
        }

        public AudioClip GetAudioClipData(string _clipName, ClipData _data) => _data.clip = _clipName == _data.id ? _data.clip : null;
        public AudioClip GetAudioClipBGM()
        {
            ClipData _tempData = new ClipData();
            _tempData = BGM_ClipData;
            GetCurrentBGMData = _tempData;
            return GetAudioClipData(GetCurrentBGMData.id, _tempData);
        }
        public AudioClip GetAudioClipSFX(string _clipName)
        {
            ClipData _tempData = new ClipData();
            _tempData = SFX_ClipData.Find(_targetData => _targetData.id == _clipName);

            GetCurrentSFXData = _tempData;
            return GetAudioClipData(_clipName, _tempData);
        }

        public AudioClip GetAudioClipVO(string _characterName, string _adegan, string _clipName)
        {
            AudioVO _tempVOData = new AudioVO();
            AudioVO.AdeganClip _tempAdeganData = new AudioVO.AdeganClip();
            ClipData _tempClipData = new ClipData();

            _tempVOData = VO_ClipData.Find(_targetData => _targetData.characterName == _characterName);
            _tempAdeganData = _tempVOData.adegan.Find(_targetData => _targetData.id == _adegan);
            _tempClipData = _tempAdeganData.clips.Find(_targetData => _targetData.id == _clipName);

            GetCurrentVOData = _tempClipData;
            return _tempClipData.clip;
        }

        [Serializable]
        public class AudioVO
        {
            public string characterName;
            public List<AdeganClip> adegan;

            [Serializable]
            public class AdeganClip
            {
                public string id;
                public List<ClipData> clips;
            }
        }
    }

    [Serializable]
    public class ClipData
    {
        public string id;
        public AudioClip clip;
    }
}
