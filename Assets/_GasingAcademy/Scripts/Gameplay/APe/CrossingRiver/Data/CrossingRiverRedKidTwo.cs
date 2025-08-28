namespace DGE.Gameplay.APe.Utils
{
    public class CrossingRiverRedKidTwo : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);
            
            startingIslandPosition = _crossingRiverManager.startingIsland.GetRedKidTwoPosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetRedKidTwoPosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
