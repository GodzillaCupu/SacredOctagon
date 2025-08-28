namespace DGE.Gameplay.APe.Utils
{    public class CrossingRiverRedKidOne :  CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);
            
            startingIslandPosition = _crossingRiverManager.startingIsland.GetRedKidOnePosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetRedKidOnePosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
