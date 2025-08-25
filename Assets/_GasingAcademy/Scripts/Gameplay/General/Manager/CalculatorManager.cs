using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DGE.Audio;
using DGE.Audio.Component;
using DGE.Gameplay.General.Utils;
using DGE.Core;
using DGE.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace DGE.Gameplay.General.Manager
{
    public class CalculatorManager : MonoBehaviour
    {
        [SerializeField] private CalculatorResetType calculatorResetType = CalculatorResetType.Zero;
        [SerializeField] float inputValue = 0;
        private string _inputString = "0";
        public string GetInputString() => _inputString;
        [SerializeField] private TMP_InputField inputField = null;
        [SerializeField] private GameObject calculatorPanel = null;
        [SerializeField] private List<CalculatorInput> calculatorNumberInputs = null;

        [Header("Calculator Input")]
        [SerializeField] private Button deleteButton = null;
        [SerializeField] private Button clearButton = null;
        [SerializeField] private Button plusMinusButton = null;
        [SerializeField] private Button commaButton = null;
        [SerializeField] private Button submitButton = null;

        [Space]
        [Header("Advanced Settings")]
        [SerializeField] private bool usingLimit = false;
        [SerializeField] private int limit = 10;

        private void OnEnable()
        {
            InitListener();
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void InitListener()
        {
            EventManager.SelectInputFieldEvent += OnSelectInputBox;
            EventManager.ResetCalculatorEvent += OnResetCalculator;
            EventManager.ResetFieldCalculatorEvent += OnResetField;

            InitListenerButtonNumber();
            InitListenerButtonOpration();
        }

        private void InitListenerButtonNumber()
        {
            for (int i = 0; i < calculatorNumberInputs.Count; i++)
            {
                AddListener(
                    calculatorNumberInputs[0].GetInputButton(),
                    () => OnNumberButton(i)
                );
            }
        }

        private void InitListenerButtonOpration()
        {
            if (deleteButton != null)
                AddListener(deleteButton, OnDeleteButton);

            if (clearButton != null)
                AddListener(clearButton, OnClearButton);

            if (plusMinusButton != null)
                AddListener(plusMinusButton, OnPlusMinusButton);

            if (commaButton != null)
                AddListener(commaButton, OnCommaButton);

            if (submitButton != null)
                AddListener(submitButton, OnSubmitButton);
        }

        Button AddListener(Button _targetButton, Action _action)
        {
            Button _tempButton = _targetButton;
            _tempButton.onClick?.AddListener(
                () => _action?.Invoke()
            );
            return _tempButton;
        }

        private void RemoveListenerButtonOprations()
        {
            if (deleteButton != null)
                RemoveListener(deleteButton);

            if (clearButton != null)
                RemoveListener(clearButton);

            if (plusMinusButton != null)
                RemoveListener(plusMinusButton);

            if (commaButton != null)
                RemoveListener(commaButton);

            if (submitButton != null)
                RemoveListener(submitButton);
        }

        private void RemoveListenerButtonNumber()
        {
            for (int i = 0; i < calculatorNumberInputs.Count; i++)
                RemoveListener(calculatorNumberInputs[i].GetInputButton());
        }

        private void RemoveListener()
        {
            EventManager.SelectInputFieldEvent -= OnSelectInputBox;
            EventManager.ResetCalculatorEvent -= OnResetCalculator;
            EventManager.ResetFieldCalculatorEvent -= OnResetField;

            RemoveListenerButtonNumber();
            RemoveListenerButtonOprations();
        }

        Button RemoveListener(Button _targetButton)
        {
            Button _tempButton = _targetButton;
            _tempButton.onClick?.RemoveAllListeners();
            return _tempButton;
        }

        public void OnSelectInputBox(TMP_InputField obj)
        {
            inputField = obj;
            calculatorPanel.SetActive(true);
            _inputString = inputField.text;

        }
        private void OnNumberButton(int number)
        {
            if (IsNull()) return;
            if (IsZero())
            {
                _inputString = number.ToString();
                inputField.text = Utils_StringExtensions.FormatNumber(int.Parse(_inputString));
            }
            else
            {
                string tempInputString = _inputString + number.ToString();
#if RESKY_MOD
            int tempInput = int.Parse(tempInputString);
            int tempInputLength = Mathf.Abs(tempInput).ToString().Length;
            if (tempInputLength > limit) return;
#endif
                _inputString += number.ToString();
                inputField.text = Utils_StringExtensions.FormatNumber(int.Parse(_inputString));
            }

        }
        private void OnDeleteButton()
        {
            if (IsNull()) return;
            _inputString = _inputString.Remove(_inputString.Length - 1);
            if (_inputString == "") _inputString = "0";
            if (IsEmpty() || !IsNumeric() || IsZero()) OnResetField();
            inputField.text = Utils_StringExtensions.FormatNumber(int.Parse(_inputString));
        }
        private void OnClearButton()
        {
            if (IsNull()) return;
            OnResetField();
        }
        private void OnPlusMinusButton()
        {
            if (IsNull()) return;
            if (IsEmpty() || IsZero()) return;
            if (inputField.text.StartsWith("-"))
            {
                inputField.text = inputField.text.Remove(0, 1);
            }
            else
            {
                inputField.text = "-" + inputField.text;
            }
        }
        private void OnCommaButton()
        {
            if (IsNull()) return;
            if (!inputField.text.Contains("."))
            {
                inputField.text += ".";
            }
        }
        private void OnSubmitButton()
        {
            Debug.Log("Submit");
            if (!IsZero())
            {
                float.TryParse(_inputString, out inputValue);
                decimal.TryParse(_inputString, out decimal decimalValue);
                //inputValue = float.Parse(inputField.text);
                EventManager.SubmitCalculator(inputValue);
                EventManager.SubmitCalculator(decimalValue);
            }
            else
            {
                // AudioManager.main.PlaySfx(SFXName.Click);
                AudioManager.Instance.Play(AudioType_Enum.SFX, "Click");
                inputValue = 0;
                OnResetField();
            }
        }
        private void OnResetCalculator()
        {
            inputField = null;
            calculatorPanel.SetActive(false);
        }
        private void OnResetField()
        {
            _inputString = calculatorResetType switch
            {
                CalculatorResetType.Zero => "0",
                CalculatorResetType.Null => "",
                _ => _inputString
            };
            if (inputField != null) inputField.text = _inputString;
        }
        private bool IsNumeric()
        {
            return float.TryParse(inputField.text, out inputValue);
        }
        private bool IsEmpty()
        {
            return _inputString == "";
        }
        private bool IsZero()
        {
            return _inputString is "0" or "-0." or "-0" or "-.";
        }

        private bool IsNull()
        {
            bool isNull = inputField == null;
            if (isNull)
            {
                Debug.Log("Input Field is Null");
            }
            return isNull;
        }
        [ContextMenu("Rename")]
        private void Rename()
        {
            for (int i = 0; i < calculatorNumberInputs.Count; i++)
            {
                calculatorNumberInputs[i].name = $"Calculator Input Button ({i})";
                calculatorNumberInputs[i].SetButtonName(i.ToString());
            }
            deleteButton.name = $"Calculator Input Button (Delete)";
            clearButton.name = $"Calculator Input Button (Clear)";
            plusMinusButton.name = $"Calculator Input Button (Plus Minus)";
            commaButton.name = $"Calculator Input Button (Comma)";
            submitButton.name = $"Calculator Input Button (Submit)";
        }
        public enum CalculatorResetType
        {
            Zero,
            Null
        }
    }
}