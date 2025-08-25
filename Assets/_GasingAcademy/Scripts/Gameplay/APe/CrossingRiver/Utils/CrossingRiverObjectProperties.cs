namespace DGE.Gameplay.APe.Utils
{
    using DGE.Utils;
    using DGE.Gameplay.APe;
    using DGE.Gameplay.APe.Manager;
    using UnityEngine;
    using DGE.Audio;

    public class CrossingRiverObjectProperties : MonoBehaviour, IInteractable
    {
        private CrossingRiverManager _crossingRiverManager;
        private CrossingRiverObjectType _crossingRiverObjectType;

        private Vector3 _targetVector;
        private float _boatYPosition;
        [SerializeField] private float maxHeight = 3;
        [SerializeField] private float totalAnimationTime = 1f;

        [SerializeField] private bool isAdult = true;

        private Transform islandPosition;

        private BoatContext _boatInUse;

        private float _rotationInDegree = 180;
        private bool _isJumpOff = true;
        private bool _isOnStartingIsland = true;

        private void Start()
        {
            _crossingRiverObjectType = GetComponent<CrossingRiverObjectType>();
        }

        public void Interact()
        {
            // Lift
            _crossingRiverManager.SelectedObject = this;

            Jump();
        }

        private void Jump()
        {
            if (_boatInUse == null)
            {

                _boatInUse = _crossingRiverManager.boat.GetAvailableBoatSeat();
                if (_boatInUse == null) return;

                if (_crossingRiverManager.GetIsMainIsland() != _isOnStartingIsland)
                {
                    _boatInUse = null;
                    return;
                }
                transform.SetParent(_crossingRiverManager.boat.transform);

                _targetVector = _boatInUse.boatSeatTransform.position;
                _targetVector.y = _boatInUse.boatSeatTransform.localPosition.y;
                _boatInUse.isInUse = true;
                _boatInUse.isSomethingTryToJumping = true;
                _boatInUse.isAdultPassenger = isAdult;
                _isJumpOff = false;

                if (_crossingRiverManager.GetIsMainIsland()) _rotationInDegree = 180;
                else _rotationInDegree = -180;

                _crossingRiverManager.boat.onCrossingToTargetIsland.AddListener(SetIsAcrossTheRiver);
                PlaySFXJump();
                MoveX();
                MoveY();
                Rotate();
            }
            else
            {
                if (_crossingRiverManager.GetIsMainIsland())
                {
                    islandPosition = _crossingRiverObjectType.startingIslandPosition;
                    transform.SetParent(_crossingRiverManager.startingIsland.transform);
                    _isOnStartingIsland = true;
                }
                else
                {
                    islandPosition = _crossingRiverObjectType.targetIslandPosition;
                    transform.SetParent(_crossingRiverManager.targetIsland.transform);
                    _isOnStartingIsland = false;
                }

                _targetVector = islandPosition.position;
                _boatInUse.isInUse = false;
                _boatInUse.isSomethingTryToJumping = true;
                _boatInUse.isAdultPassenger = false;
                _isJumpOff = true;

                if (_crossingRiverManager.GetIsMainIsland()) _rotationInDegree = -180;
                else _rotationInDegree = 180;

                _crossingRiverManager.boat.onCrossingToTargetIsland.RemoveListener(SetIsAcrossTheRiver);
                PlaySFXJump();
                MoveX();
                MoveY();
                Rotate();
            }

            void MoveX()
            {
                LeanTween.moveX(gameObject, _targetVector.x, totalAnimationTime)
                    .setEase(LeanTweenType.linear)
                    .setOnComplete(
                        () =>
                        {
                            _boatInUse.isSomethingTryToJumping = false;
                            if (_isJumpOff) _boatInUse = null;
                            _crossingRiverManager.CheckWinCondition();
                        }
                    );
            }

            void MoveY()
            {
                LeanTween.moveY(gameObject, maxHeight, totalAnimationTime / 2)
                    .setEase(LeanTweenType.easeOutSine)
                    .setOnComplete(
                        () =>
                        {
                            LeanTween.moveY(gameObject, maxHeight, totalAnimationTime / 2)
                                .setEase(LeanTweenType.easeInSine);
                        }
                    );
            }

            void Rotate()
            {
                LeanTween.rotate(gameObject, transform.rotation.eulerAngles + Vector3.forward * _rotationInDegree, totalAnimationTime / 2)
                    .setEase(LeanTweenType.easeInSine)
                    .setOnComplete(
                        () =>
                        {
                            LeanTween.rotate(gameObject, transform.rotation.eulerAngles + Vector3.forward * _rotationInDegree, totalAnimationTime / 2)
                            .setEase(LeanTweenType.easeOutSine);
                        }
                    );
            }

            void PlaySFXJump() => AudioManager.Instance.Play(Audio.Component.AudioType_Enum.SFX, "Jump");

        }

        private void SetIsAcrossTheRiver(bool isActive)
        {
            _crossingRiverObjectType.isAcrossTheRiver = isActive;
        }

        public bool GetIsOnStartingIsland()
        {
            return _isOnStartingIsland;
        }

        public bool GetIsJumpOff()
        {
            return _isJumpOff;
        }

        public void ResetLevel()
        {
            if (_boatInUse != null)
            {
                _crossingRiverManager.boat.onCrossingToTargetIsland.RemoveListener(SetIsAcrossTheRiver);
            }

            transform.SetParent(_crossingRiverManager.startingIsland.transform);
            _isOnStartingIsland = true;
            _isJumpOff = true;
            _boatInUse = null;
        }
    }
}