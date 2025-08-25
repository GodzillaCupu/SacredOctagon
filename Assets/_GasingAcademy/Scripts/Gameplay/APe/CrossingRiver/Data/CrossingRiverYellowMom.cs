namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverYellowMom : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);            
            
            startingIslandPosition = CrossingRiverManager.startingIsland.GetYellowMomPosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetYellowMomPosition();

            transform.position = startingIslandPosition.position;
        }

        protected override void CheckLoseCondition()
        {
            var redMom = CrossingRiverManager.Resolve<CrossingRiverRedMom>();
            var redKidOne = CrossingRiverManager.Resolve<CrossingRiverRedKidOne>();
            var redKidTwo = CrossingRiverManager.Resolve<CrossingRiverRedKidTwo>();

            if (redMom.isAcrossTheRiver != isAcrossTheRiver)
            {
                if (redKidOne.isAcrossTheRiver == isAcrossTheRiver || redKidTwo.isAcrossTheRiver == isAcrossTheRiver)
                {
                    CrossingRiverManager.onGameLost?.Invoke();
                }
            }
        }
    }
}
