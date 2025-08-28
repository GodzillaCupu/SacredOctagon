using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DGE.Audio;
using DGE.Gameplay.General;
using DGE.Gameplay.General.Manager;

namespace Berpetualang.CrossingRiver.Z2L3B7Petualang
{
    public class Z2L3B7_Quest : MonoBehaviour
    {
        [Header("Quest")]
        [SerializeField] private int minNum1 = 1;
        [SerializeField] private int maxNum1 = 10;
        [SerializeField] private int minNum2 = 1;
        [SerializeField] private int maxNum2 = 10;
        [SerializeField] private int minResult = 1;
        [SerializeField] private int maxResult = 100;

        [Space]
        [Header("Components")]
        [SerializeField] private GameObject questView = null;
        [SerializeField] private CalculatorManager calculator = null;
        [SerializeField] private GameObject calculatorView = null;

        [Space]
        [Header("Text")]
        [SerializeField] private TMP_Text questText = null;

        [Space]
        [Header("Mutant")]
        [SerializeField] private Animator mutantAnim = null;

        [Space]
        [Header("Events")]
        [SerializeField] private UnityEvent onCorrect = new UnityEvent();
        [SerializeField] private UnityEvent onWrong = new UnityEvent();

        private QuestData currentQuest = null;
        private bool isOnQuest = false;

        private void Start()
        {
            onCorrect.AddListener(OnCorrect);
            onWrong.AddListener(OnWrong);
        }

        public void StartQuest()
        {
            calculator.ClearInputFields();
            currentQuest = new QuestData(minNum1, maxNum1, minNum2, maxNum2, minResult, maxResult);
            questText.text = $"{currentQuest.Num1} + {currentQuest.Num2} = ";
            questView.SetActive(true);
            calculatorView.SetActive(true);
            isOnQuest = true;
            //mutantAnim.Play("Swimming Idle");
        }

        public void Submit()
        {
            if (!isOnQuest) return;
            int answerValue = int.Parse(calculator.GetInputString());
            if (answerValue == currentQuest.Result)
            {
                PlayAnswerSfx(true);
                onCorrect.Invoke();
            }
            else
            {
                PlayAnswerSfx(false);
                onWrong.Invoke();
            }
        }

        private void OnCorrect()
        {
            isOnQuest = false;
            questView.SetActive(false);
            calculatorView.SetActive(false);
        }
        private void OnWrong()
        {
            calculator.ClearInputFields();
        }
        private void PlaySfx(string sfxName)
        {
            AudioManager.Instance.Play(DGE.Audio.Component.AudioType_Enum.SFX,"CorrectAttempt");
        }

        private void PlayAnswerSfx(bool correct)
        {
            string answerSFX = correct ? "CorrectAttempt" : "FalseAttempt";
            AudioManager.Instance.Play(DGE.Audio.Component.AudioType_Enum.SFX,answerSFX);
        }
        private class QuestData
        {
            public int Num1 = 0;
            public int Num2 = 0;
            public int Result = 0;

            public QuestData(int minNum1, int maxNum1, int minNum2, int maxNum2, int minResult, int maxResult)
            {
                RandomNums();
                int loopCount = 100;
                while (Result < minResult || Result > maxResult)
                {
                    RandomNums();
                    loopCount--;
                    if (loopCount == 0) break;
                }
                void RandomNums()
                {
                    Num1 = Random.Range(minNum1, maxNum1 + 1);
                    Num2 = Random.Range(minNum2, maxNum2 + 1);
                    Result = Num1 + Num2;
                }
            }
        }
    }
}