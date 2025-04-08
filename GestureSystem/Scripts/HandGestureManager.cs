using UnityEngine;

namespace Cacophony 
{

    public class HandGestureManager : IGestureManager<SimpleHandPose>
    {
        public SimpleHandPose currentHandPose;
        public Vector3 currentPosition;
        public IHandDataConnector handDataConnector;
        void OnEnable()
        {
            currentHandPose = new();
            if (handDataConnector == null)
            {
                handDataConnector = FindFirstObjectByType<IHandDataConnector>();
                if (handDataConnector == null)
                {
                    Debug.LogError("No hand data connector found. Please assign one in the inspector or ensure one is present in the scene.");
                }
                else
                {
                    handDataConnector.OnNewData.AddListener(OnHandDataReceived);
                    handDataConnector.OnHandFound.AddListener(EnableGesture);
                    handDataConnector.OnNoHandPresentAfterTimeout.AddListener(DisableGesture);
                }
            }
        }

        void OnDisable()
        {
            if (handDataConnector != null)
            {
                handDataConnector.OnNewData.RemoveListener(OnHandDataReceived);
                handDataConnector.OnHandFound.RemoveListener(EnableGesture);
                handDataConnector.OnNoHandPresentAfterTimeout.RemoveListener(DisableGesture);
            }
        }

        void Update()
        {
            Evaluate(currentPosition, currentHandPose);
        }

        private void OnHandDataReceived(HandDataEventArgs eventArgs)
        {
            currentHandPose = eventArgs.handPose;
            currentPosition = eventArgs.handPosition;
        }

        public void EnableGesture()
        {
            gestureDetector.EnableDetector();
        }

        public void DisableGesture()
        {
            gestureDetector.DisableDetector();
        }

        public void SetHandPose(SimpleHandPose pose)
        {
            currentHandPose = pose;
        }

        public void SetHandPosition(Vector3 position)
        {
            currentPosition = position;
        }

        public void SetHandData(SimpleHandPose pose, Vector3 position)
        {
            currentHandPose = pose;
            currentPosition = position;
        }

        public void SetGestureEnabled(bool isOn)
        {
            if (isOn){
                EnableGesture();
            }
            else {
                DisableGesture();
            }
        }
    }

}