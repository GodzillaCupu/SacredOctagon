namespace DGE.Gameplay.APe.Utils
{
    using UnityEngine;
    
    public class CrossingRiverRobber : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            CrossingRiverManager.Register(this);

            startingIslandPosition = CrossingRiverManager.startingIsland.GetRobberPosition();
            targetIslandPosition = CrossingRiverManager.targetIsland.GetRobberPosition();

            transform.position = startingIslandPosition.position;
        }
        protected override void CheckLoseCondition()
        {
            var police = CrossingRiverManager.Resolve<CrossingRiverPoliceman>();
            if (police.isAcrossTheRiver != isAcrossTheRiver)
            {
                foreach (var objectKvp in CrossingRiverManager.GetAllObject())
                {
                    if (objectKvp.Value == police) continue;
                    if (objectKvp.Value == this) continue;
                    if (objectKvp.Value.isAcrossTheRiver == isAcrossTheRiver)
                    {
                        // Lose 
                        CrossingRiverManager.onGameLost?.Invoke();
                    }
                }
            }
        }
    }
}
