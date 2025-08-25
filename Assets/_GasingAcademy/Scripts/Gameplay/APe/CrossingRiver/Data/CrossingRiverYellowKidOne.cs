namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverYellowKidOne : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);
            
            startingIslandPosition = CrossingRiverManager.startingIsland.GetYellowKidOnePosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetYellowKidOnePosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
