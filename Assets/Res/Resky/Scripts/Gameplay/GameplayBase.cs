using System.Collections;
using System.Collections.Generic;
using DGE.Audio;
using UnityEngine;
using UnityEngine.Events;

public class GameplayBase : MonoBehaviour
{
    [Header("Data")]
    public int spawnIndex = 0;

    [Space]
    [Header("Events")]
    public UnityEvent onGameplayCompleted = new UnityEvent();
    [HideInInspector] public UnityEvent onGameplayFailed = new UnityEvent();
    public UnityEvent<float> onAddScoreEvent = new UnityEvent<float>();
    [SerializeField] GameObject _blockPanel;

    public virtual void OnGameplayCompleted()
    {
        StopSfxLoop();
        onGameplayCompleted.Invoke();
    }

    public virtual void OnGameplayFailed()
    {
        StopSfxLoop();
        OpenBlockPanel(true);
        onGameplayFailed.Invoke();
    }

    private void OpenBlockPanel(bool _isOpen) => _blockPanel.SetActive(_isOpen);

    public virtual void OnAddScoreEvent(float score) => onAddScoreEvent.Invoke(score);

    public void PlaySfx(string sfxName)
    {
        AudioManager.Instance.Play(
            DGE.Audio.Component.AudioType_Enum.SFX,
            sfxName);
    }

    public void PlaySfxDirect(string sfxName) => PlaySfx(sfxName);

    public void PlaySfxWithScale(string sfxName, float sfxScale) => PlaySfx(sfxName);

    public void PlaySfxLoop(string sfxName)
    {
        AudioManager.Instance.Play(
            DGE.Audio.Component.AudioType_Enum.SFX,
            sfxName, true);
    }

    public void StopSfx()
    {
        AudioManager.Instance.StopSFX();
    }
    public void StopSfxLoop()
    {
        AudioManager.Instance.StopSFX(false);
    }
    public void PlayAnswerSfx(bool isCorrect)
    {
        AudioManager.Instance.Play(
            DGE.Audio.Component.AudioType_Enum.SFX,
            isCorrect ? "CorrectAnswer" : "WrongAnswer");
    }

    public void PlayAnswerSFXDirect(bool isCorrect)
    {
        PlayAnswerSfx(isCorrect);
    }

    public float GetSfxLength()
    {
        AudioClip _tempClip = AudioManager.Instance.GetAudioClip(DGE.Audio.Component.AudioType_Enum.SFX);
        float _tempLength = _tempClip.length;
        return _tempLength;
    }
    public float GetSfxLengthAnswer(bool isCorrect)
    {
        return GetSfxLength() / 3;
    }
}
