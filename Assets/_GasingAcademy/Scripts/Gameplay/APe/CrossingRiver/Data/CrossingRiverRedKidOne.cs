namespace DGE.Gameplay.APe.Utils
{    public class CrossingRiverRedKidOne :  CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);
            
            startingIslandPosition = CrossingRiverManager.startingIsland.GetRedKidOnePosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetRedKidOnePosition();

            transform.position = startingIslandPosition.position;
        }

    }
}
