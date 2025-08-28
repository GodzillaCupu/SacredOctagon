namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverYellowKidOne : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);
            
            startingIslandPosition = _crossingRiverManager.startingIsland.GetYellowKidOnePosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetYellowKidOnePosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
