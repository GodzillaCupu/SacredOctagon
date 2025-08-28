using System.Collections;
using DGE.Audio;
using DGE.Core;
using DGE.Utils.core;
using UnityEngine;
using UnityEngine.Events;

namespace DGE.Gameplay.General.Manager
{
    public class TimerManager : Singleton<TimerManager>
    {
        [Header("Settings")]

        public float GameTime = 5f;
        private float gameTime
        {
            get
            {
                return GameTime * 60f; // convert to seconds
            }
        }
        [SerializeField] private bool updateOnStart = true;
        [SerializeField] private bool finishGameOnTimeOut = true;

        [Space]
        [Header("Events")]
        [SerializeField] private UnityEvent onGameTimeOut = new UnityEvent();

        private delegate void OnState();
        private OnState onState = null;

        public float CurrentTime { get; private set; } = 0;
        public float CurrentStageTime { get; private set; } = 0;
        public bool _isGameStarted = false;
        private bool _isGameFinished = false;
        private void Awake()
        {
            if (finishGameOnTimeOut)
                onGameTimeOut.AddListener(() => ChangeState(0));
        }
        private void OnEnable()
        {
            EventManager.StartGameEvent += OnStartGame;
        }
        private void OnDisable()
        {
            EventManager.StartGameEvent -= OnStartGame;
        }
        private void OnStartGame()
        {
            _isGameStarted = true;
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _isGameStarted);
            if (updateOnStart) onState = OnUpdateTime;
        }
        private void OnDestroy()
        {
            if (_lastTenSeconds)
                AudioManager.Instance.StopBGM();
        }
        private void Update()
        {
            onState?.Invoke();
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Minus))
                CurrentTime = gameTime - 11;
            if (Input.GetKeyDown(KeyCode.Equals))
                CurrentTime -= 10;
#endif
        }

        private bool _lastTenSeconds = false;
        private void OnUpdateTime()
        {
            if (_isGameFinished) return;
            CurrentStageTime += Time.deltaTime;
            CurrentTime += Time.deltaTime;
            if (CurrentTime > gameTime) CurrentTime = gameTime;
            if (gameTime - CurrentTime < 10 && _lastTenSeconds == false)
            {
                AudioManager.Instance.PlaySFX("TimeTickin",true);
                _lastTenSeconds = true;
            }
            if (CurrentTime >= gameTime && !_isGameFinished)
            {
                GameTimeOut();
            }
        }
        private void GameTimeOut()
        {
            if (finishGameOnTimeOut)
            {
                _isGameFinished = true;
            }
            else
            {
                _lastTenSeconds = false;
            }
            Debug.Log("[Bug] Timeout");
            onGameTimeOut.Invoke();
            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX("TimeRunOut");
        }

        private void OnPauseTime()
        {

        }

        public void ChangeState(int id) // 0. Pause, 1. Update.
        {
            switch (id)
            {
                case 0:
                    if (_lastTenSeconds)
                        AudioManager.Instance.StopSFX(); // Suara TimerTickin minta di berentiin pas selesai lawan big mutant!
                    onState = OnPauseTime;
                    break;
                case 1:
                    onState = OnUpdateTime;
                    break;
            }
        }

        public void ResetTime(int id) // 0. Current Time, 1. Stage Time
        {
            switch (id)
            {
                case 0:
                    CurrentTime = 0;
                    if (_lastTenSeconds)
                    {
                        AudioManager.Instance.StopSFX();
                        _lastTenSeconds = false;
                    }
                    break;
                case 1:
                    CurrentStageTime = 0;
                    break;
            }
        }

        public  void FinishGame()
        {
            ChangeState(0);
        }

        public void SetCurrentTime(float time)
        {
            CurrentTime = gameTime - time;
        }

        public void AddTimeoutEvent(UnityAction callback)
        {
            onGameTimeOut.AddListener(callback);
        }
    }
}
