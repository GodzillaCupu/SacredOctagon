using System;
using System.Collections;
using System.Collections.Generic;
using DGE.Audio;
using DGE.Gameplay.APe.Data;
using DGE.Gameplay.APe.Utils;
using UnityEngine;
using UnityEngine.Events;
using DGE.Utils.core;
using DGE.Core;

namespace DGE.Gameplay.APe.Manager
{
    [Serializable]
    public class BoatContext
    {
        public Transform boatSeatTransform;
        public bool isInUse;
        public bool isSomethingTryToJumping;
        public bool isAdultPassenger;
    }

    public class CrossingRiverManager : Singleton<CrossingRiverManager>
    {
        [HideInInspector] public UnityEvent onCheckLoseCondition = new();
        [HideInInspector] public UnityEvent onResetLevel = new();
        [HideInInspector] public UnityEvent onGameLost = new();
        public UnityEvent onComplete = new();

        public CrossingRiverIsland startingIsland;
        public CrossingRiverIsland targetIsland;

        public CrossingRiverBoat boat;

        public IInteractable SelectedObject { get; set; }

        private readonly Dictionary<Type, CrossingRiverObjectType> _objectTypesUsingType = new();

        private bool _boatIsOnMainIsland;

        [SerializeField] string bgmAmbienceName;
        [SerializeField][Range(0, 1)] float bgmAmbienceVolume = 0.7f;
        [SerializeField][Range(0, 1)] float bgmZoneVolume = .5f;
        private float defaultBGMSourceVolume;
        private float defaultBGMLoopVolume;
        private float defaultBGMAmbienceVolume;
        private bool isFinished = false;

        private IEnumerator Start()
        {
            onGameLost.AddListener(Lost);
            onResetLevel.AddListener(ResetLevel);

            _boatIsOnMainIsland = true;
            SetDefaultVolume();
            AudioManager.Instance.Play(Audio.Component.AudioType_Enum.SFX, "StartGame");
            yield return new WaitUntil(
                () => 
                AudioManager.Instance.sourceHandler.source.SFX.isPlaying == false
            );
            
            AudioManager.Instance.Play(Audio.Component.AudioType_Enum.BGM, bgmAmbienceName);
        }

        //private void PlayBGMAmbience()
        //{
        //    if (string.IsNullOrEmpty(bgmAmbienceName))
        //        return;
        //    AudioManager.main.SetBGMSourceVolume(bgmZoneVolume * defaultBGMSourceVolume);
        //    if (bgmAmbienceName == "Sungai")
        //    {
        //        AudioManager.main.SetBGMLoopVolume(0.1f * defaultBGMLoopVolume);
        //    }
        //    else
        //    {
        //        AudioManager.main.SetBGMLoopVolume(bgmAmbienceVolume * defaultBGMAmbienceVolume);
        //    }
        //    AudioManager.main.PlayBGMOnceLoop(bgmAmbienceName);
        //}
        public bool GetIsMainIsland()
        {
            return _boatIsOnMainIsland;
        }

        public void SetIsMainIsland(bool activeState)
        {
            _boatIsOnMainIsland = activeState;
        }

        public void Register<T>(T objectType) where T : CrossingRiverObjectType
        {
            if (_objectTypesUsingType.ContainsKey(typeof(T)))
            {
                Debug.LogError("You cant add multiple object with same object");
            }
            else
            {
                _objectTypesUsingType.Add(typeof(T), objectType);
            }
        }

        public T Resolve<T>() where T : CrossingRiverObjectType
        {
            if (_objectTypesUsingType.TryGetValue(typeof(T), out var obj))
            {
                return obj as T;
            }
            else
            {
                Debug.LogError($"there's no available {typeof(T)} on the Dictionary. Maybe use another type?. Or you can simply Register needed type");
                return null;
            }
        }

        public Dictionary<Type, CrossingRiverObjectType> GetAllObject()
        {
            return _objectTypesUsingType;
        }

        public void CheckWinCondition()
        {
            if (isFinished) return;
            var robber = Resolve<CrossingRiverRobber>();
            foreach (var objectType in _objectTypesUsingType)
            {
                var objectProperties = objectType.Value.GetComponent<CrossingRiverObjectProperties>();
                if (objectType.Value == robber) continue;
                if (!objectProperties.GetIsOnStartingIsland() && objectProperties.GetIsJumpOff()) continue;

                return;
            }

            Debug.LogError("Selamat yah kamu menang!");
            AudioManager.Instance.StopBGM();
            ResetDefaultVolume();
            //onGameLost.Invoke();
            onComplete?.Invoke();
            isFinished = true;
            EventManager.FinishGame();
        }

        public void CheckLoseCondition()
        {
            onCheckLoseCondition?.Invoke();
        }

        private void ResetLevel()
        {
            foreach (var objectTypeKvp in _objectTypesUsingType)
            {
                objectTypeKvp.Value.ResetLevel();
                objectTypeKvp.Value.GetComponent<CrossingRiverObjectProperties>().ResetLevel();
            }

            boat.ResetLevel();
        }

        private void Lost()
        {
            // Debug.LogError("YOU LOST");
        }
        private void OnDestroy()
        {
            ResetDefaultVolume();
        }

        private void SetDefaultVolume()
        {
            AudioManager.Instance.SetVolumeBGM(bgmAmbienceVolume);
        }

        private void ResetDefaultVolume()
        {
            AudioManager.Instance.SetVolumeBGM(bgmAmbienceVolume);
            Debug.Log($"Default bgm volume: {defaultBGMSourceVolume}. Bgm loop volumse: {defaultBGMLoopVolume}");
        }
    }
}
