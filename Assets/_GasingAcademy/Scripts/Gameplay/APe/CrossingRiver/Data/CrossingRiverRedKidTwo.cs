namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverRedKidTwo : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);
            
            startingIslandPosition = CrossingRiverManager.startingIsland.GetRedKidTwoPosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetRedKidTwoPosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
