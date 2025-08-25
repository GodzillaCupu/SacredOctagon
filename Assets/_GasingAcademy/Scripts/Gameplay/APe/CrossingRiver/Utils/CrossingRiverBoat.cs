namespace DGE.Gameplay.APe.Utils
{
    using DGE.Core;
    using DGE.Audio;
    using DGE.Audio.Component;
    using DGE.Gameplay.APe.Manager;
    using DGE.Gameplay.APe.Utils;
    using DGE.Utils;
    using UnityEngine;
    using UnityEngine.Events;
    public class CrossingRiverBoat : MonoBehaviour, IUpdatable
    {
        [HideInInspector] public UnityEvent<bool> onCrossingToTargetIsland = new();

        private CrossingRiverManager _crossingRiverManager;

        [Header("Main")]
        [SerializeField] private BoatContext boatSeatOne;
        [SerializeField] private BoatContext boatSeatTwo;

        [SerializeField] private float boatSpeed = 0.5f;
        [SerializeField] private float boatMoveDistance = 6f;

        [Space]
        [Header("Animation")]
        [SerializeField] private AnimationCurve animCurveY = new AnimationCurve();
        private float defaultYPos = 0;

        private bool _isMoving;

        private void Start()
        {
            defaultYPos = transform.localPosition.y;
            UpdateManager.Add(this);
        }
        private void OnDestroy()
        {
            UpdateManager.Remove(this);
        }

        void IUpdatable.OnUpdate()
        {
            transform.position = new Vector3(transform.position.x, animCurveY.Evaluate(Time.time) + defaultYPos, transform.position.z);
        }

        public void MoveBoat()
        {
            if (_isMoving) return;
            if (boatSeatOne.isSomethingTryToJumping || boatSeatTwo.isSomethingTryToJumping) return;
            if (!boatSeatOne.isAdultPassenger && !boatSeatTwo.isAdultPassenger) return;

            AudioManager.Instance.Play(AudioType_Enum.SFX, "Rowing");
            if (_crossingRiverManager.GetIsMainIsland())
            {
                _isMoving = true;
                LeanTween.moveX(this.gameObject, transform.position.x - boatMoveDistance, boatSpeed).setOnComplete(() =>
                    {
                        _isMoving = false;
                        AudioManager.Instance.Stop(AudioType_Enum.SFX);
                    }
                );
                _crossingRiverManager.SetIsMainIsland(true);
                onCrossingToTargetIsland?.Invoke(false);
            }
            else
            {
                _isMoving = true;
                LeanTween.moveX(this.gameObject, transform.position.x + boatMoveDistance, boatSpeed).setOnComplete(() =>
                    {
                        _isMoving = false;
                        AudioManager.Instance.Stop(AudioType_Enum.SFX);
                    }
                );
                _crossingRiverManager.SetIsMainIsland(true);
                onCrossingToTargetIsland?.Invoke(false);
            }


            _crossingRiverManager.CheckLoseCondition();
        }

        public BoatContext GetAvailableBoatSeat()
        {
            if (!boatSeatOne.isInUse)
            {
                return boatSeatOne;
            }
            else if (!boatSeatTwo.isInUse)
            {
                return boatSeatTwo;
            }
            else
            {
                return null;
            }
        }

        public void ResetLevel()
        {
            _isMoving = false;
            boatSeatOne.isAdultPassenger = false;
            boatSeatOne.isInUse = false;
            boatSeatOne.isSomethingTryToJumping = false;

            boatSeatTwo.isAdultPassenger = false;
            boatSeatTwo.isInUse = false;
            boatSeatTwo.isSomethingTryToJumping = false;

            if (!_crossingRiverManager.GetIsMainIsland())
            {
                transform.position += Vector3.right * boatMoveDistance;
            }

            _crossingRiverManager.SetIsMainIsland(true);
        }
    }
}
