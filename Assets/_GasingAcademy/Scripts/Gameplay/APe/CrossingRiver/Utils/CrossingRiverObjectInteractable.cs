namespace DGE.Gameplay.APe.Utils
{
    using System;
    using DGE.Utils;
    using DGE.Gameplay.APe;
    using DGE.Gameplay.APe.Manager;
    using UnityEngine;
    public class CrossingRiverObjectInteractable : MonoBehaviour
    {
        private CrossingRiverManager _crossingRiverManager;
        private IInteractable _interactable;

        private void Start()
        {
            _crossingRiverManager = CrossingRiverManager.Instance;
            _interactable = GetComponent<IInteractable>();
        }

        public void OnClick()
        {
            if (_crossingRiverManager.SelectedObject != null && _crossingRiverManager.SelectedObject == _interactable) return;

            _interactable.Interact();
        }
    }
}
