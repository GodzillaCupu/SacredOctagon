namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverYellowKidTwo : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);
            
            startingIslandPosition = _crossingRiverManager.startingIsland.GetYellowKidTwoPosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetYellowKidTwoPosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
