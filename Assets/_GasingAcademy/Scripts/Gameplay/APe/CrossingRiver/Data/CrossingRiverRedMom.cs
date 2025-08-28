namespace DGE.Gameplay.APe.Utils
{
    using UnityEngine;
    public class CrossingRiverRedMom : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);

            startingIslandPosition = _crossingRiverManager.startingIsland.GetRedMomPosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetRedMomPosition();

            transform.position = startingIslandPosition.position;
        }

        protected override void CheckLoseCondition()
        {
            var yellowMom = _crossingRiverManager.Resolve<CrossingRiverYellowMom>();
            var yellowKidOne = _crossingRiverManager.Resolve<CrossingRiverYellowKidOne>();
            var yellowKidTwo = _crossingRiverManager.Resolve<CrossingRiverYellowKidTwo>();

            if (yellowMom.isAcrossTheRiver != isAcrossTheRiver)
            {
                if (yellowKidOne.isAcrossTheRiver == isAcrossTheRiver || yellowKidTwo.isAcrossTheRiver == isAcrossTheRiver)
                {
                    _crossingRiverManager.onGameLost?.Invoke();
                }
            }
        }
    }
}
