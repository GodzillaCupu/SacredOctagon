namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverYellowKidTwo : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);
            
            startingIslandPosition = CrossingRiverManager.startingIsland.GetYellowKidTwoPosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetYellowKidTwoPosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
