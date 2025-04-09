
using UnityEngine;
using UnityEngine.Events;

namespace Cacophony
{
    public struct HandDataEventArgs
    {
        public SimpleHandPose handPose;
        public Vector3 handPosition;
    }

    public class IHandDataConnector : MonoBehaviour
    {
        [Tooltip("The time in seconds to retain the last hand when hand is lost")]
        public float lostHandPersistence = 0.25f;

        [Tooltip("The region in which hands will be detected for gestures")]
        public Collider interactionBounds;

        [Header("Hand Events")]
        [Tooltip("Subscribe to OnNewData to get the latest hand data")]
        public UnityEvent<HandDataEventArgs> OnNewData;
        [Tooltip("Subscribe to be notified when a new hand is found")]
        public UnityEvent OnHandFound;
        [Tooltip("Subscribe to be notified when no hands are present")]
        public UnityEvent OnNoHandPresentAfterTimeout;   
    }
}