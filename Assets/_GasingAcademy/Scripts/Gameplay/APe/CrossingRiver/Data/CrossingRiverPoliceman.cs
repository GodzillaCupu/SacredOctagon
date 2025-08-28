namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverPoliceman : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);
            
            startingIslandPosition = _crossingRiverManager.startingIsland.GetPolicePosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetPolicePosition();
            
            transform.position = startingIslandPosition.position;
        }


    }
}
