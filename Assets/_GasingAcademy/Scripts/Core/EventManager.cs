using System;
using TMPro;
using DGE.Utils.core;
using DGE.UI;

namespace DGE.Core
{
    public class EventManager : SingletonPersistant<EventManager>
    {
        #region Calculator
        public static event Action<TMP_InputField> SelectInputFieldEvent;
        public static void SelectInputField(TMP_InputField input) => SelectInputFieldEvent?.Invoke(input);
        public static event Action UnselectInputFieldEvent;
        public static void UnselectInputField() => UnselectInputFieldEvent?.Invoke();
        public static event Action<UI_InputBox> SelectInputBoxEvent;
        public static void SelectInputBox(UI_InputBox input) => SelectInputBoxEvent?.Invoke(input);
        public static event Action<float> SubmitCalculatorEvent;
        public static void SubmitCalculator(float score) => SubmitCalculatorEvent?.Invoke(score);
        public static event Action<decimal> SubmitCalculatorDecimalEvent;
        public static void SubmitCalculator(decimal score) => SubmitCalculatorDecimalEvent?.Invoke(score);

        public static event Action ResetCalculatorEvent;
        public static void ResetCalculator() => ResetCalculatorEvent?.Invoke();
        public static event Action ResetFieldCalculatorEvent;
        public static void ResetFieldCalculator() => ResetFieldCalculatorEvent?.Invoke();
        #endregion

        #region EndGame
        public static event Action FinishGameEvent;
        public static void FinishGame() => FinishGameEvent?.Invoke();
        public static event Action ForceGameCompleteEvent;
        public static void ForceGameComplete() => ForceGameCompleteEvent?.Invoke();
        public static event Action WinConditionEvent;
        public static void WinCondition() => WinConditionEvent?.Invoke();
        public static event Action LoseConditionEvent;
        public static void LoseCondition() => LoseConditionEvent?.Invoke();
        public static event Action PvpFinishEvent;
        public static void PvpFinish() => PvpFinishEvent?.Invoke();
        #endregion
    }
}
