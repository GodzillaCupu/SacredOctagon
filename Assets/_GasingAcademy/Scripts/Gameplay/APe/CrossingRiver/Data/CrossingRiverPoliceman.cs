namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverPoliceman : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);
            
            startingIslandPosition = CrossingRiverManager.startingIsland.GetPolicePosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetPolicePosition();
            
            transform.position = startingIslandPosition.position;
        }


    }
}
