using UnityEngine;

namespace Cacophony 
{

    public class HandGestureManager : IGestureManager<SimpleHandPose>
    {
        public AnimatedTestHand testHand;

        void Update()
        {
            Evaluate(Vector3.zero, testHand.animatedPose.handPose);
        }

    }

}