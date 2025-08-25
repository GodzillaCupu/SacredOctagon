namespace DGE.Gameplay.APe.Utils
{
    using UnityEngine;
    public class CrossingRiverRedMom : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);

            startingIslandPosition = CrossingRiverManager.startingIsland.GetRedMomPosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetRedMomPosition();

            transform.position = startingIslandPosition.position;
        }

        protected override void CheckLoseCondition()
        {
            var yellowMom = CrossingRiverManager.Resolve<CrossingRiverYellowMom>();
            var yellowKidOne = CrossingRiverManager.Resolve<CrossingRiverYellowKidOne>();
            var yellowKidTwo = CrossingRiverManager.Resolve<CrossingRiverYellowKidTwo>();

            if (yellowMom.isAcrossTheRiver != isAcrossTheRiver)
            {
                if (yellowKidOne.isAcrossTheRiver == isAcrossTheRiver || yellowKidTwo.isAcrossTheRiver == isAcrossTheRiver)
                {
                    CrossingRiverManager.onGameLost?.Invoke();
                }
            }
        }
    }
}
