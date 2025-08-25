using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DGE.Core;
using DGE.Utils;
using DGE.Utils.core;

namespace DGE.UI
{
    public class UI_InputBox : Singleton<UI_InputBox>
    {
        [SerializeField] protected TMP_InputField inputField = null;
        public TMP_InputField InputField => inputField;
        [SerializeField] protected Image inputImage = null;
        [Header("Event")]
        [SerializeField] private UnityEvent onValueChanged = null;

        [Header("Status")]
        [SerializeField] private bool canCheck = true;
        [SerializeField] private bool isAnswered = false;
        [SerializeField] private bool isCorrected = false;
        [Header("Change")]
        [SerializeField] protected ChangeType changeType = ChangeType.TextColor;
        [Header("Color")]
        [SerializeField] protected Color normalColor = Color.white;
        [Header("Image")]
        [SerializeField] protected Sprite normalSprite = null;
        [SerializeField] protected Sprite editSprite = null;
        [SerializeField] protected Sprite wrongSprite = null;
        [SerializeField] protected Sprite correctSprite = null;
        [Header("Font")]
        [SerializeField] protected TMP_FontAsset normalFont = null;
        [SerializeField] protected TMP_FontAsset editFont = null;
        [SerializeField] protected TMP_FontAsset wrongFont = null;
        [SerializeField] protected TMP_FontAsset correctFont = null;

        [Header("Auto Size")]
        [SerializeField] private bool autoSize = false;
        [SerializeField] private LayoutElement layoutElement = null;
        [SerializeField] private float[] autoWidth = null;

        [Space]
        [Header("Input Field Settings")]
        [SerializeField] private bool alwaysInteractable = false;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            inputField.interactable = active;
        }
        private void CheckAnswered()
        {
            if (!canCheck) return;
            inputField.interactable = alwaysInteractable || !isAnswered || !isCorrected;
        }

        public virtual void SetupType2(string text)
        {
            inputField.text = text;
            if (!IsQuestion(text)) SetCorrectAnswer();
        }

        public void InputFieldActive(bool isActive)
        {
            inputField.interactable = isActive;
        }
        public bool GetInputFieldInteractable()
        {
            return inputField.interactable;
        }
        public void SetText(string newText)
        {
            inputField.text = newText;
        }
        public string GetInputText(bool getNull = false, bool getZero = false)
        {
            string input;
            if (getNull)
            {
                input = inputField.text == "" ? "" : inputField.text;
            }
            else if (getZero)
            {
                input = inputField.text == "" ? "0" : inputField.text;
            }
            else
            {
                input = inputField.text == "" ? "0.123456" : inputField.text;
            }
            return input;
        }

        public void SetCorrected(bool correct, bool nullInput = false)
        {
            if (correct)
            {
                SetCorrectAnswer();
            }
            else if (nullInput)
            {
                ResetAnswer();
            }
            else
            {
                SetWrongAnswer();
            }
        }
        public void SetCorrectAnswer()
        {
            OnChangeColor(false, false, true);
            isAnswered = true;
            isCorrected = true;
            CheckAnswered();
        }
        public void SetWrongAnswer()
        {
            isCorrected = false;
            OnChangeColor(false, false, false);
        }

        private void SetNormalAnswer()
        {
            isAnswered = false;
            isCorrected = false;
            CheckAnswered();
            OnChangeColor();
        }

        public void OnSelectInputField()
        {
            EventManager.SelectInputField(inputField);
            EventManager.SelectInputBox(this);
        }
        public virtual void OnEditInputField()
        {
            SetNormalAnswer();
            onValueChanged?.Invoke();
        }

        public void OnEditFieldV2()
        {
            OnChangeColor(false, true, false);
            onValueChanged?.Invoke();
            Debug.Log($"{gameObject.name}: Edit Input Field V2");
        }

        public void OnEndEditFieldV2()
        {
            OnChangeColor();
            Debug.Log($"{gameObject.name}: End Edit Input Field V2");
        }
        public virtual void ResetAnswer()
        {
            isAnswered = false;
            isCorrected = false;
            inputField.text = "";
            SetNormalAnswer();
        }
        private void OnChangeColor(bool normal = true, bool edit = false, bool correct = false)
        {
            Color color = normal ? normalColor : edit ? EditColor : correct ? CorrectColor : WrongColor;
            Sprite sprite = normal ? normalSprite : edit ? editSprite : correct ? correctSprite : wrongSprite;

            switch (changeType)
            {
                case ChangeType.TextColor:
                    inputField.textComponent.color = color;
                    break;
                case ChangeType.ImageColor:
                    inputImage.color = color;
                    break;
                case ChangeType.ImageSprite:
                    inputImage.sprite = sprite;
                    break;
                case ChangeType.ImageTextComponent:
                    var font = normal ? normalFont : edit ? editFont : correct ? correctFont : wrongFont;
                    inputImage.sprite = sprite;
                    inputField.textComponent.font = font;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AutoSize()
        {
            if (!autoSize) return;
            int index = inputField.text.Length;
            if (inputField.text.Length >= autoWidth.Length) index = autoWidth.Length - 1;
            layoutElement.preferredWidth = autoWidth[index];
        }
        private bool IsQuestion(string text)
        {
            return text == "";
        }
        public bool IsCorrected()
        {
            return isCorrected;
        }

        public bool IsHaveValue()
        {
            return inputField.text != "";
        }
        protected enum ChangeType
        {
            TextColor,
            ImageColor,
            ImageSprite,
            ImageTextComponent
        }

        public static Color EditColor = new Color(0.965f, 0.898f, 0.553f);
        public static Color WrongColor = new Color(1, 0.475f, 0.475f);
        public static Color CorrectColor = new Color(0.722f, 0.914f, 0.580f);
   }
}