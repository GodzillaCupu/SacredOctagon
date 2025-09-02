using System;
using System.Collections.Generic;
using TMPro;
using DGE.Audio;
using DGE.Audio.Component;
using DGE.Core;
using DGE.Utils;
using DGE.Utils.core;
using DGE.Gameplay.General.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using System.Globalization;
using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager.Components;

namespace DGE.Gameplay.General.Manager
{
    public class CalculatorManager : Singleton<CalculatorManager>
    {
        [SerializeField] double inputValue = 0;
        private string inputString = "0";
        private string _inputString
        {
            get
            {
                return inputString;
            }
            set
            {
                inputString = value;
                onInputStringChanged?.Invoke(value);
            }
        }
        public string GetInputString() => _inputString;
        [SerializeField] private TMP_InputField inputField = null;
        private TMP_InputField _lastInputField = null;
        [SerializeField] private UIAnimator calculatorAnimator = null;
        [SerializeField] private GameObject calculatorPanel = null;
        [SerializeField] private List<CalculatorInput> calculatorNumberInputs = null;

        [Header("Calculator Input")]
        [SerializeField]
        private UIButton deleteButton = null;

        [SerializeField] private UIButton clearButton = null;
        [SerializeField] private UIButton plusMinusButton = null;
        [SerializeField] private UIButton commaButton = null;
        [SerializeField] private UIButton submitButton = null;

        [Space]
        [Header("Input")]
        [SerializeField]
        private List<TMP_InputField> allFields = new List<TMP_InputField>();

        [FormerlySerializedAs("resetType")]
        [Space]
        [Header("Settings")]
        [SerializeField]
        private CalculatorResetType calculatorResetType = CalculatorResetType.Zero;
        public CalculatorResetType CalculatorResetType { get { return calculatorResetType; } }

        [SerializeField] private int digitLimit = 10;
        [SerializeField] private bool replaceValue = true;
        [SerializeField] private bool usingComa = false;
        [SerializeField] private bool usingPlusMinus = false;
        [SerializeField] private bool autoNextWhileEdit = false;
        [SerializeField] private bool canClearSelected = false;
        [SerializeField] private bool autoNextDelete = false;
        [SerializeField] private int nextDeleteTrigger = 2;
        private Action<string> onInputStringChanged = null;
        [SerializeField] private int digitAfterComaLimit = -1;
        public void SetAutoNext(bool active)
        {
            autoNextWhileEdit = active;
            replaceValue = active;
        }
        public void AddInputStringChangedAction(Action<string> callback, bool replace)
        {
            if (replace) onInputStringChanged = callback;
            else onInputStringChanged += callback;
        }
        public void RemoveInputStringChangedAction(Action<string> callback)
        {
            onInputStringChanged -= callback;
        }

        [Tooltip("jika ada multiple input field, mulai dari index paling akhir (mayoritas mulai dari kanan)")]
        [SerializeField]
        private bool reverseInputFieldRead = false;

        [Tooltip("Saat submit, input field yang sedang diassign sekarang diclear")]
        [SerializeField]
        private bool autoClearAfterSubmit = true;

        [Tooltip("Saat panggil EventManager.OnResetField, sekalian ngeclear input buttonnya")]
        [SerializeField]
        private bool clearOnResetField = false;

        [Tooltip("Digunakan di Ular Tangga ABer")]
        [SerializeField]
        private bool detectZeroAsInput = false;

        [Tooltip("Defaultnya next input tidak mendeteksi apakah inputFieldnya null (digunakan di deret Z6)")]
        [SerializeField]
        private bool NextInputSkipNull = false;

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
            EventManager.UnselectInputFieldEvent += UnselectInputBox;
            EventManager.ResetCalculatorEvent += OnResetCalculator;
            EventManager.ResetFieldCalculatorEvent += OnResetField;
            for (int i = 0; i < calculatorNumberInputs.Count; i++)
            {
                var num = i;
                UIButton _tempButton = calculatorNumberInputs[i].GetInputButton();
                calculatorNumberInputs[i].GetInputButton().onPointerDownBehaviour.Event
                    .AddListener(() => OnNumberButton(num));
            }

            if (deleteButton != null) deleteButton.onPointerDownBehaviour.Event.AddListener(OnDeleteButton);
            if (clearButton != null) clearButton.onPointerDownBehaviour.Event.AddListener(OnClearButton);
            if (plusMinusButton != null) plusMinusButton.onPointerDownBehaviour.Event.AddListener(OnPlusMinusButton);
            if (commaButton != null) commaButton.onPointerDownBehaviour.Event.AddListener(OnCommaButton);
            if (submitButton != null) submitButton.onPointerDownBehaviour.Event.AddListener(OnSubmitButton);

            for (int i = 0; i < calculatorNumberInputs.Count; i++)
            {
                var num = i;
                calculatorNumberInputs[i].GetInputButton().onClickBehaviour.Event
                    .AddListener(() => OnNumberButton(num));
            }

            if (deleteButton != null) deleteButton.onClickBehaviour.Event.AddListener(OnDeleteButton);
            if (clearButton != null) clearButton.onClickBehaviour.Event.AddListener(OnClearButton);
            if (plusMinusButton != null) plusMinusButton.onClickBehaviour.Event.AddListener(OnPlusMinusButton);
            if (commaButton != null) commaButton.onClickBehaviour.Event.AddListener(OnCommaButton);
            if (submitButton != null) submitButton.onClickBehaviour.Event.AddListener(OnSubmitButton);
        }

        private void RemoveListener()
        {
            EventManager.SelectInputFieldEvent -= OnSelectInputBox;
            EventManager.UnselectInputFieldEvent -= UnselectInputBox;
            EventManager.ResetCalculatorEvent -= OnResetCalculator;
            EventManager.ResetFieldCalculatorEvent -= OnResetField;
            for (int i = 0; i < calculatorNumberInputs.Count; i++)
            {
                var num = i;
                calculatorNumberInputs[i].GetInputButton().onPointerDownBehaviour.Event.RemoveAllListeners();
            }

            if (deleteButton != null) deleteButton.onPointerDownBehaviour.Event.RemoveAllListeners();
            if (clearButton != null) clearButton.onPointerDownBehaviour.Event.RemoveAllListeners();
            if (plusMinusButton != null) plusMinusButton.onPointerDownBehaviour.Event.RemoveAllListeners();
            if (commaButton != null) commaButton.onPointerDownBehaviour.Event.RemoveAllListeners();
            if (submitButton != null) submitButton.onPointerDownBehaviour.Event.RemoveAllListeners();
            for (int i = 0; i < calculatorNumberInputs.Count; i++)
            {
                var num = i;
                calculatorNumberInputs[i].GetInputButton().onClickBehaviour.Event.RemoveAllListeners();
            }

            if (deleteButton != null) deleteButton.onClickBehaviour.Event.RemoveAllListeners();
            if (clearButton != null) clearButton.onClickBehaviour.Event.RemoveAllListeners();
            if (plusMinusButton != null) plusMinusButton.onClickBehaviour.Event.RemoveAllListeners();
            if (commaButton != null) commaButton.onClickBehaviour.Event.RemoveAllListeners();
            if (submitButton != null) submitButton.onClickBehaviour.Event.RemoveAllListeners();
        }

        private void Start()
        {
            if (inputField != null) inputField.Select();
        }

        public void OnSelectInputBox(TMP_InputField obj)
        {
            //if (inputField != null && inputField == obj) { return; }
            _delClicked = 0;
            if (_lastInputField != null && _lastInputField != obj) _lastInputField.OnDeselect(null);
            inputField = obj;
            _lastInputField = inputField;
            if (inputField != null) inputField.Select();
            //OpenCalculator(true);
            _inputString = inputField.text.Replace(Utils_StringExtensions.NUM_SEPARATOR, "");
            /*if (CheckInput(inputField))
            {
                NextInput();
            }*/
        }
        public void OnDeselectInputBox()
        {
            _delClicked = 0;
            inputField = null;
        }

        private void UnselectInputBox()
        {
            //_delClicked = 0;
            //OpenCalculator(false);
        }
        private double prevInputParsed = 0;
        private void OnNumberButton(int number)
        {
            if (!isActiveState) return;
            if (inputField == null)
                NextInputFieldWithoutNull();
            Debug.Log($"Number {number}");
            if (IsLimit())
            {
                Debug.Log("Limit");
                if (autoNextWhileEdit) NextInput();
                return;
            }

            if (IsZero() && !usingComa)
            {
                inputField.text = number.ToString();
                _inputString = number.ToString();
                if (autoNextWhileEdit) NextInput();
            }
            else
            {
                if (replaceValue)
                {
                    inputField.text = number.ToString();
                    _inputString = number.ToString();
                    if (autoNextWhileEdit) NextInput();
                }
                else
                {
                    if (string.IsNullOrEmpty(inputField.text) && number == 0)
                    {
                        inputField.text = "0";
                        _inputString = "0";
                    }
                    else
                    {
                        inputField.text += number.ToString();
                        _inputString += number.ToString();
                        if (usingComa)
                        {
                            Debug.Log($"[Format] Input string {_inputString}. Input field {inputField.text}");
                            float parseNum = float.Parse(_inputString);
                            if (!_inputString.Contains(".") && ((parseNum != 0 && prevInputParsed != parseNum) || (parseNum == 0))) inputField.text = Utils_StringExtensions.FormatNumber(parseNum);
                            //if (parseNum != 0 && prevInputParsed != parseNum || (number == 0)) inputField.text = Utils_StringExtensions.FormatNumber(parseNum);
                            prevInputParsed = parseNum;
                        }
                        else
                        {
                            inputField.text = Utils_StringExtensions.FormatNumber(long.Parse(_inputString));
                        }
                    }
                }
            }
        }
        private void NextInput()
        {
            if (!reverseInputFieldRead)
            {
                if (IsAllFull() || IsLastFull())
                {
                    Debug.Log("All Full");
                    return;
                }
            }

            int index = allFields.IndexOf(inputField);
            if (reverseInputFieldRead)
            {
                if (index <= 0) return;
                index--;
            }
            else
            {
                if (index >= allFields.Count - 1) return;
                index++;
            }
            SelectInput(index);
            if (IsLimit() || !IsActiveField(inputField))
            {
                NextInput();
            }
        }

        private void PreviousInput()
        {
            int index = allFields.IndexOf(inputField);
            if (reverseInputFieldRead)
            {
                if (index < allFields.Count - 1)
                {
                    index++;
                    SelectInput(index);
                }
            }
            else
            {
                index--;
                index = Mathf.Clamp(index, 0, allFields.Count);
                SelectInput(index);
            }
            if (!CheckActiveInputField(allFields[index]))
            {
                Debug.LogWarning($"Cant select input field {allFields[index].name}, not active or interactable");
                NextInput();
                return;
            }
            if (index > 0)
            {
                index--;
                SelectInput(index);
            }
        }
        public void SelectFirstInputFromAllFields()
        {
            if (allFields.Count == 0) return;
            if (reverseInputFieldRead)
            {
                SelectInput(allFields.Count - 1);
            }
            else
            {
                SelectInput(0);
            }
            //Debug.Log($"SelectInputFirstOnAllFields IsLimit: {IsLimit()} IsActive: {IsActiveField(inputField)}");
            if (IsLimit() || !IsActiveField(inputField))
            {
                NextInput();
            }
        }
        public void SelectInput(int index)
        {
            /*if(!CheckActiveInputField(allFields[index]))
            {
                Debug.LogWarning($"Cant select input field {allFields[index].name}, not active or interactable");
                return;
            }*/
            Debug.Log($"Select Input {index}");
            //if (inputField != null && inputField == allFields[index]) { return; }
            _delClicked = 0;
            if (_lastInputField != null || _lastInputField != null && _lastInputField != allFields[index]) _lastInputField.OnDeselect(null);
            inputField = allFields[index];
            _lastInputField = inputField;
            if (inputField != null && !IsInputFieldNotActive()) inputField.Select();
            _inputString = _lastInputField.text.Replace(Utils_StringExtensions.NUM_SEPARATOR, "");
        }

        private int _delClicked;
        private bool InputFieldContains()
        {
            return inputField != null && allFields.Contains(inputField);
        }
        private void OnDeleteButton()
        {
            if (!isActiveState) return;
            if (autoNextDelete && InputFieldContains())
            {
                _delClicked++;
                if (_delClicked >= nextDeleteTrigger)
                {
                    _delClicked = 0;
                    NextDelete();
                    return;
                }
            }

            Debug.Log("Before Delete field");
            DeleteField();

            if (inputField != null)
            {
                if (string.IsNullOrEmpty(inputField.text) &&
                    calculatorResetType == CalculatorResetType.Zero)
                {
                    inputField.text = "0";
                    _inputString = "0";
                }
                if (_inputString.Contains("."))
                {
                    //inputField.text = Utils_StringExtensions.FormatNumber(num);
                }
                else
                {
                    if (double.TryParse(inputField.text.Replace(Utils_StringExtensions.NUM_SEPARATOR, ""), out double value))
                    {
                        Debug.Log($"Format {value}");
                        inputField.text = Utils_StringExtensions.FormatNumber((long)value);
                        prevInputParsed = value;
                    }
                }
            }
        }

        private int _nextDeleteCounter = 0;
        private void NextDelete()
        {
            if (!reverseInputFieldRead)
            {
                if (IsAllEmpty() || IsFirstEmpty())
                {
                    Debug.Log("All Empty");
                    return;
                }
            }
            Debug.Log("Next Delete");
            if (IsEmpty() || (IsZero() && calculatorResetType == CalculatorResetType.Zero) || IsInputFieldNotActive())
            {
                PreviousInput();
                Debug.Log("Next Delete again");
            }
            else
            {
                _nextDeleteCounter = 0;
            }

            DeleteField();
            inputField.Select();
        }

        private void DeleteField()
        {
            Debug.Log("Delete Field");
            if (inputField == null)
            {
                TMP_InputField inputFieldTemp = !reverseInputFieldRead ? allFields[^1] : allFields[allFields.IndexOf(inputField) + 1];
                OnSelectInputBox(inputFieldTemp);
            }
            if (IsInputFieldNotActive())
            {
                return;
            }
            if (IsEmpty())
            {
                return;
            }
            _inputString = _inputString.Remove(_inputString.Length - 1);
            Debug.Log("new input : " + _inputString);
            if (!string.IsNullOrEmpty(_inputString))
            {
                if(!_inputString.Contains("."))
                {
                    _inputString = double.TryParse(_inputString, out var result) ? result.ToString(CultureInfo.InvariantCulture) : "";
                    inputField.text = _inputString;
                }
                else
                {
                    inputField.text = inputField.text.Remove(inputField.text.Length - 1);
                }
            }
            if (_inputString == "")
            {
                if (calculatorResetType == CalculatorResetType.Zero)
                {
                    _inputString = "0";
                    inputField.text = usingComa ? Utils_StringExtensions.FormatNumber(float.Parse(_inputString)) : Utils_StringExtensions.FormatNumber(long.Parse(_inputString));

                }
                else
                {
                    inputField.text = "";
                }
            }

            //if (IsEmpty() || !IsNumeric() || IsZero()) 
            //{ 
            //    OnResetField();
            //}
        }

        public void OnClearButton()
        {
            if (allFields.Count == 0 || !isActiveState)
            {
                inputField.text = ResetField();
                return;
            }
            foreach (TMP_InputField field in allFields)
            {
                if (field != null)
                {
                    field.text = ResetField();
                    field.OnDeselect(null);
                }
            }

            inputField = null;
            TMP_InputField temp = reverseInputFieldRead ? allFields[^1] : NextInputFieldWithoutNull();
            OnSelectInputBox(temp);
            if (canClearSelected) OnResetField();
        }

        private void OnPlusMinusButton()
        {
            if (!usingPlusMinus || !isActiveState) return;
            if (IsEmpty() || IsZero()) return;
            if (inputField.text.StartsWith("-"))
            {
                inputField.text = inputField.text.Remove(0, 1);
                _inputString = _inputString.Remove(0, 1);
            }
            else
            {
                inputField.text = "-" + inputField.text;
                _inputString = "-" + _inputString;
            }
        }

        private void OnCommaButton()
        {
            if (!usingComa || IsLimit() || IsEmpty() || !isActiveState) return;

            if (!inputField.text.Contains(",") && !_inputString.Contains("."))
            {
                inputField.text += ",";
                _inputString += ".";
            }
        }

        private void OnSubmitButton()
        {
            if (!isActiveState) return;
            Debug.Log("Submit");
            foreach (var field in allFields.Where(field => field != null))
            {
                field.OnDeselect(null);
            }
            if (!detectZeroAsInput)
            {
                if (!IsZero())
                {
                    double.TryParse(_inputString, out inputValue);
                    EventManager.SubmitCalculator(float.TryParse(_inputString, out float result) ? result : 0);
                    EventManager.SubmitCalculator(decimal.TryParse(_inputString, out decimal decimalValue) ? decimalValue : 0);
                }
                else
                {
                    inputValue = 0;
                    if (autoClearAfterSubmit)
                        OnResetField();
                }
            }
            else
            {
                double.TryParse(_inputString, out inputValue);
                EventManager.SubmitCalculator((float)inputValue);
                //EventManager.SubmitCalculator(float.TryParse(_inputString, out float result) ? result : 0);
                EventManager.SubmitCalculator(decimal.TryParse(_inputString, out decimal decimalValue) ? decimalValue : 0);
            }
        }

        private void OnResetCalculator()
        {
            inputField = null;
            //OpenCalculator(false);
        }

        private void OpenCalculator(bool value)
        {
            if (value)
            {
                calculatorPanel.SetActive(true);
                if (calculatorAnimator != null)
                {
                    calculatorAnimator.Play();
                }
            }
            else
            {
                if (calculatorAnimator != null)
                {
                    calculatorAnimator.Play(true);
                }
                else
                {
                    calculatorPanel.SetActive(false);
                }
            }
        }

        public void OnResetField()
        {
            Debug.Log("[Calculator] Reset field");
            _inputString = calculatorResetType switch
            {
                CalculatorResetType.Zero => "0",
                CalculatorResetType.Null => "",
                _ => _inputString
            };
            if (inputField != null) inputField.text = _inputString;
            if (clearOnResetField)
                OnClearButton();
            if (allFields == null) return;
            if (allFields.Count == 0) return;
            //inputField = reverseInputFieldRead ? allFields[^1] : allFields[0];
            inputField = null;
            TMP_InputField temp = reverseInputFieldRead ? allFields[^1] : NextInputFieldWithoutNull();
            OnSelectInputBox(temp);
        }
        public void SetInputString(string value)
        {
            _inputString = value;
            inputField.text = Utils_StringExtensions.FormatNumber(long.Parse(value));
        }
        public void ClearInputFields()
        {
            _inputString = ResetField();
            if (inputField != null)
            {
                inputField.text = _inputString;
            }
            foreach (TMP_InputField input in allFields)
            {
                input.text = _inputString;
            }
        }

        private string ResetField()
        {
            _inputString = "0";
            return calculatorResetType switch
            {
                CalculatorResetType.Zero => "0",
                CalculatorResetType.Null => "",
                _ => inputField.text
            };
        }

        private bool IsNumeric()
        {
            return double.TryParse(_inputString, out inputValue);
        }

        public float GetInputValue()
        {
            if (float.TryParse(_inputString, out float result))
            {
                inputValue = result;
            }

            return (float)inputValue;
        }

        private bool IsEmpty()
        {
            return inputField.text == "";
        }

        private bool IsAllEmpty()
        {
            return allFields.All(field => field.text == "");
        }

        private bool IsFirstEmpty()
        {
            if (reverseInputFieldRead)
                return false;
            else
                return inputField == allFields[0] && IsEmpty();
        }

        private bool IsZero()
        {
            if (inputField == null) return false;
            return inputField.text is "-.";
        }

        private bool CheckLimitAndActiveField(TMP_InputField iF)
        {
            string i = iF.text.Replace(".", "");
            return i.Length >= digitLimit || IsActiveField(iF);
        }
        private bool IsLimit()
        {
            string input = inputField.text.Replace(".", "");
            // Detect if it's a decimal number
            if (usingComa && digitAfterComaLimit > 0 && input.Contains(','))
            {
                string[] data = input.Split(',');
                if (data[0].Length >= digitLimit || data[1].Length >= digitAfterComaLimit) return true;
                else return false;
            }
            // Default checking
            Debug.Log($"{inputField.name} {input.Length} >= {digitLimit} IsLimit{input.Length >= digitLimit}");
            return input.Length >= digitLimit;
        }

        private bool IsAllFull()
        {
            return allFields.All(field => field.text != "");
        }

        private bool IsLastFull()
        {
            return inputField == allFields[^1] && !IsEmpty();
        }

        private bool IsActiveField(TMP_InputField iF)
        {
            if (!iF.gameObject.activeSelf || !iF.interactable)
            {
                return false;
            }
            if (iF.gameObject.activeSelf && iF.interactable && IsLimit())
            {
                Debug.Log($"{iF.name} Active Interactable IsLimit");
                return false;
            }
            return iF.gameObject.activeSelf || iF.interactable || iF.gameObject.activeSelf && !iF.interactable;
        }
        private bool CheckActiveInputField(TMP_InputField iF)
        {
            if (iF == null)
            {
                Debug.LogWarning("Input Field is null");
                return false;
            }
            if (!iF.gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"Input Field {iF.name} is not active in hierarchy");
                //return false;
            }
            if (!iF.interactable)
            {
                Debug.LogWarning($"Input Field {iF.name} is not interactable");
                //return false;
            }
            return iF.interactable || iF.gameObject.activeInHierarchy;
        }
        private bool IsInputFieldNotActive()
        {
            bool notActive = !inputField.interactable || !inputField.gameObject.activeInHierarchy;
            if (notActive)
            {
                Debug.LogWarning($"Input Field {inputField.name} not interactable");
            }

            return notActive;
        }
        //private bool NextWhen
        public List<TMP_InputField> GetInputFields()
        {
            return allFields;
        }

        public TMP_InputField GetInputField()
        {
            return inputField;
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

        public void AddInputField(TMP_InputField field)
        {
            if (!allFields.Contains(field)) allFields.Add(field);
        }

        public void RemoveInputField(TMP_InputField field)
        {
            allFields.Remove(field);
        }

        public void NullInputField(int inputFieldIndex)
        {
            allFields[inputFieldIndex] = null;
        }

        public void ClearAllFields()
        {
            allFields = new List<TMP_InputField>();
        }

        public void ReplaceField(int index, TMP_InputField field)
        {
            allFields[index] = field;
        }
        //gak berani edit yang NextInput() hehe
        public TMP_InputField NextInputFieldWithoutNull()
        {
            if (inputField == null)
            {
                for (int i = 0; i < allFields.Count; i++)
                {
                    if (allFields[i] != null && allFields[i].gameObject.activeSelf)
                    {
                        return allFields[i];
                    }
                }
            }
            else
            {
                int currentIndex = allFields.IndexOf(inputField);
                for (int i = currentIndex + 1; i < allFields.Count; i++)
                {
                    if (allFields[i] != null && allFields[i].gameObject.activeSelf)
                    {
                        return allFields[i];
                    }
                }
                //jika sudah mentok kembali ke awal
                for (int i = 0; i < allFields.Count; i++)
                {
                    if (allFields[i] != null && allFields[i].gameObject.activeSelf)
                    {
                        return allFields[i];
                    }
                }
            }
            return null;
        }

        public List<CalculatorInput> GetCalculatorNumberInputs()
        {
            return calculatorNumberInputs;
        }
        public void SetCalculatorDigitLimit(int digit)
        {
            digitLimit = digit;
        }
        public void SetReplaceValue(bool active)
        {
            replaceValue = active;
        }

        /// <summary>
        /// remove coma to enable input >10 digit
        /// </summary>
        /// <param name="coma"></param>
        public void SetComa(bool coma)
        {
            usingComa = coma;
        }

        private bool isActiveState = true;
        public void SetActiveState(bool active)
        {
            isActiveState = active;
        }
    }
    public enum CalculatorResetType
    {
        Zero,
        Null
    }
}
