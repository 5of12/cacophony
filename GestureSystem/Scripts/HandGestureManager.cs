using UnityEngine;

namespace Cacophony 
{

    public class HandGestureManager : IGestureManager<SimpleHandPose>
    {
        public SimpleHandPose currentHandPose;
        public Vector3 currentPosition;

        void OnEnable()
        {
            currentHandPose = new();
        }

        void Update()
        {
            Evaluate(currentPosition, currentHandPose);
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