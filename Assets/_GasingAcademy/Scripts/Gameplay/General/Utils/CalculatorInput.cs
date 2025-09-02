using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;

namespace DGE.Gameplay.General.Utils
{
    public class CalculatorInput : MonoBehaviour
    {
        [SerializeField] UIButton inputButton = null;
        [SerializeField] TMP_Text buttonNameText = null;
        [SerializeField] TMP_Text inputText = null;

        public void SetButtonName(string name)
        {
            buttonNameText.text = name;
        }
        public void SetButtonInteractable(bool interactable)
        {
            inputButton.interactable = interactable;
        }
        public UIButton GetInputButton()
        {
            return inputButton;
        }

        public string GetText()
        {
            return buttonNameText.text;
        }
    }
}
