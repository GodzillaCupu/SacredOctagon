namespace DGE.Gameplay.APe.Utils
{
    using DGE.Gameplay.APe.Manager;
    using UnityEngine;
    
    public class CrossingRiverObjectType : MonoBehaviour
    {
        protected CrossingRiverManager _crossingRiverManager;

        public Transform startingIslandPosition;
        public Transform targetIslandPosition;

        public bool isAcrossTheRiver;

        protected virtual void Start()
        {
            _crossingRiverManager = CrossingRiverManager.Instance;
            _crossingRiverManager.onCheckLoseCondition.AddListener(CheckLoseCondition);
        }

        protected virtual void CheckLoseCondition()
        {

        }

        public void ResetLevel()
        {
            isAcrossTheRiver = false;
            transform.position = startingIslandPosition.position;
        }

    }
}