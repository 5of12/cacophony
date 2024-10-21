using UnityEngine;

namespace Cacophony 
{

    public class HandGestureManager : IGestureManager<SimpleHandPose>
    {
        public SimpleHandPose currentHandPose;
        public Vector3 currentPosition;

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

        public void SetHandData(SimpleHandPose pose, Vector3 position)
        {
            currentHandPose = pose;
            currentPosition = position;
        }
    }

}