namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverYellowMom : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);            
            
            startingIslandPosition = _crossingRiverManager.startingIsland.GetYellowMomPosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetYellowMomPosition();

            transform.position = startingIslandPosition.position;
        }

        protected override void CheckLoseCondition()
        {
            var redMom = _crossingRiverManager.Resolve<CrossingRiverRedMom>();
            var redKidOne = _crossingRiverManager.Resolve<CrossingRiverRedKidOne>();
            var redKidTwo = _crossingRiverManager.Resolve<CrossingRiverRedKidTwo>();

            if (redMom.isAcrossTheRiver != isAcrossTheRiver)
            {
                if (redKidOne.isAcrossTheRiver == isAcrossTheRiver || redKidTwo.isAcrossTheRiver == isAcrossTheRiver)
                {
                    _crossingRiverManager.onGameLost?.Invoke();
                }
            }
        }
    }
}
