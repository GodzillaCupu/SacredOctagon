using UnityEngine;
using UnityEngine.UI;
using DGE.Utils.core;
using DGE.Gameplay.APe.Manager;

namespace CrossingRiver.Scripts.Managers
{
    public class CrossingRiverUIManager : Singleton<CrossingRiverUIManager>
    {
        private CrossingRiverManager _crossingRiverManager;

        [SerializeField] private Button moveBoatButton;
        [SerializeField] private Button resetLevelButton;

        [Header("Canvas")]
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private GameObject popUpCanvas;

        [Space]
        [Header("Advanced Settings")]
        [SerializeField] private bool useCustomBoatButtonAction = false;

        private void Start()
        {
            _crossingRiverManager = CrossingRiverManager.Instance;

            _crossingRiverManager.onGameLost.AddListener(OpenPopUpCanvas);

            if (!useCustomBoatButtonAction) moveBoatButton.onClick.AddListener(_crossingRiverManager.boat.MoveBoat);
            resetLevelButton.onClick.AddListener(ResetLevel);
        }

        private void OpenPopUpCanvas()
        {
            popUpCanvas.SetActive(true);
            mainCanvas.SetActive(false);
        }

        private void ResetLevel()
        {
            popUpCanvas.SetActive(false);
            mainCanvas.SetActive(true);

            _crossingRiverManager.onResetLevel?.Invoke();
        }
    }
}
