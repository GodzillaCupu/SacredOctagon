using UnityEngine;
using DGE.Core;
using DGE.Utils.core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;
using System;
using Doozy.Runtime.UIManager.Containers;
using DGE.Audio;

namespace DGE.Gameplay.General.Manager
{
    public class QuestManager : MonoBehaviour
    {
        [Header("Quest")]
        public QuestionsData data = new QuestionsData();
        private string currentQuesitons;
        private QuestionsType questionsType;

        [Space]
        [Header("UI Components")]
        [SerializeField] private UIView questView = null;
        [SerializeField] private TMP_Text questText = null;

        [Space]
        [SerializeField] private UIView calculatorView = null;
        [SerializeField] private CalculatorManager calculator = null;

        [Space]
        [Header("Text")]

        [Space]
        [Header("Character")]
        [SerializeField] private Animator mutantAnim = null;

        [Space]
        [Header("Events")]
        [SerializeField] private UnityEvent onCorrect = new UnityEvent();
        [SerializeField] private UnityEvent onWrong = new UnityEvent();

        private bool isOnQuest = false;

        private void Start()
        {
            onCorrect?.AddListener(OnCorrect);
            onWrong?.AddListener(OnWrong);
        }

        public void StartQuest()
        {
            calculator.ClearInputFields();
            currentQuesitons = data.GetQuestions(_type: QuestionsType.Subtraction);
            questView.Show();
            calculatorView.Show();
            isOnQuest = true;
            //mutantAnim.Play("Swimming Idle");
        }

        public void Submit()
        {
            if (!isOnQuest) return;
            int answerValue = int.Parse(calculator.GetInputString());
            if (answerValue == data.resultNumber)
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
            questView.Hide();
            calculatorView.Hide();
        }

        private void OnWrong()
        {
            calculator.ClearInputFields();
        }

        private void PlaySfx(string sfxName)
        {
            AudioManager.Instance.Play(Audio.Component.AudioType_Enum.SFX, sfxName);
        }

        private void PlayAnswerSfx(bool correct)
        {
            string answerSFX = correct ? "Correct" : "Wrong";
            AudioManager.Instance.Play(Audio.Component.AudioType_Enum.SFX, answerSFX);

        }
    }

    [Serializable]
    public class QuestionsData : IQuestions
    {
        [Range(1, 100)]
        public int number1;
        [Range(1, 100)]
        public int number2;

        public int resultNumber;

        public int maximumNumber = 100;
        public int minimumNumber = 1;

        public int questionsRange = 100;

        public int GetNumber(int _minimumValue, int _maximumValue)
        {
            int _tempNumber = 0;
            RandomizeNumber(_tempNumber, minimumNumber, maximumNumber);
            return _tempNumber;           
        }

        public string GetQuestions(QuestionsType _type) => GetQuestions(_type, number1, number2, minimumNumber, maximumNumber);

        public string GetQuestions(QuestionsType _type, int _number1, int _number2, int _minimumNumber, int _maximumNumber)
        {
            string _tempQuestions = string.Empty;
            string _tempStringResult = string.Empty;
            string _tempStringNumber1 = string.Empty;
            string _tempStringNumber2 = string.Empty;
            string _tempStringOprations = GetQuestionsType(_type);

            int _tempNumber1 = GetNumber(_minimumNumber, _maximumNumber);
            int _tempNumber2 = GetNumber(minimumNumber, _maximumNumber);

            int _result = GetResultByType(_type, _number1, _number2);

            while (_result < _minimumNumber || _result > _maximumNumber)
            {
                _tempStringNumber1 = _number1.ToString();
                _tempStringNumber2 = _number2.ToString();
                _tempQuestions = $"{_tempStringNumber1} {_tempStringOprations} {_tempStringNumber2} = ";
                if (questionsRange == 0) break;
            }
            return _tempQuestions;
        }

        public string GetQuestionsType(QuestionsType _type)
        {
            switch (_type)
            {
                case QuestionsType.Division:
                    return " / ";

                case QuestionsType.Multiplication:
                    return " x ";

                case QuestionsType.Subtraction:
                    return " - ";

                case QuestionsType.Addition:
                    return " + ";

                default:
                    return string.Empty;
            }
        }

        public int GetResultByType(QuestionsType _type, int _number1 = 1, int _number2 = 100)
        {
            switch (_type)
            {
                case QuestionsType.Division:
                    return number1 / number2;

                case QuestionsType.Multiplication:
                    return number1 * number2;

                case QuestionsType.Subtraction:
                    return number1 - number2;

                case QuestionsType.Addition:
                    return number1 + number2;

                default:
                    throw new NullReferenceException();
            }
        }

        public int RandomizeNumber(int _targetNumber, int _minValue = 1, int _maxValue = 100)
        {
            int temp = _targetNumber;
            temp = UnityEngine.Random.Range(_minValue, _maxValue + 1);
            return temp;
        }
    }

    public interface IQuestions
    {
        public string GetQuestions(QuestionsType _type);
        public int GetNumber(int _minimumValue, int _maximumValue);
        public int GetResultByType(QuestionsType _type, int _number1, int _number2);
        public int RandomizeNumber(int _targetNumber, int _minValue = 1, int _maxValue = 100);
    }

    public enum QuestionsType
    {
        Division,
        Multiplication,
        Subtraction,
        Addition
    }
}
