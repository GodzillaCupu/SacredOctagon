namespace DGE.Gameplay.APe.Utils
{
    using UnityEngine;
    
    public class CrossingRiverRobber : CrossingRiverObjectType
    {
        protected override void Start()
        {
            base.Start();

            _crossingRiverManager.Register(this);

            startingIslandPosition = _crossingRiverManager.startingIsland.GetRobberPosition();
            targetIslandPosition = _crossingRiverManager.targetIsland.GetRobberPosition();

            transform.position = startingIslandPosition.position;
        }
        protected override void CheckLoseCondition()
        {
            var police = _crossingRiverManager.Resolve<CrossingRiverPoliceman>();
            if (police.isAcrossTheRiver != isAcrossTheRiver)
            {
                foreach (var objectKvp in _crossingRiverManager.GetAllObject())
                {
                    if (objectKvp.Value == police) continue;
                    if (objectKvp.Value == this) continue;
                    if (objectKvp.Value.isAcrossTheRiver == isAcrossTheRiver)
                    {
                        // Lose 
                        _crossingRiverManager.onGameLost?.Invoke();
                    }
                }
            }
        }
    }
}
