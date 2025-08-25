
using UnityEngine;

namespace DGE.Gameplay.APe.Data
{
    public class CrossingRiverIsland : MonoBehaviour
    {
        [SerializeField] private Transform policePosition;
        [SerializeField] private Transform robberPosition;
        [SerializeField] private Transform redHairedMomPosition;
        [SerializeField] private Transform redHairedKidOnePosition;
        [SerializeField] private Transform redHairedKidTwoPosition;
        [SerializeField] private Transform yellowHairedMomPosition;
        [SerializeField] private Transform yellowHairedKidOnePosition;
        [SerializeField] private Transform yellowHairedKidTwoPosition;

        public Transform GetPolicePosition()
        {
            return policePosition;
        }

        public Transform GetRobberPosition()
        {
            return robberPosition;
        }

        public Transform GetRedMomPosition()
        {
            return redHairedMomPosition;
        }

        public Transform GetRedKidOnePosition()
        {
            return redHairedKidOnePosition;
        }

        public Transform GetRedKidTwoPosition()
        {
            return redHairedKidTwoPosition;
        }

        public Transform GetYellowMomPosition()
        {
            return yellowHairedMomPosition;
        }

        public Transform GetYellowKidOnePosition()
        {
            return yellowHairedKidOnePosition;
        }

        public Transform GetYellowKidTwoPosition()
        {
            return yellowHairedKidTwoPosition;
        }
    }
}
