using UnityEngine;

namespace Cacophony 
{

    public class GestureManager : MonoBehaviour
    {
        public HandGestureDetector gestureDetector;
        public AnimatedTestHand testHand;

        void Start()
        {
        }

        void Update()
        {
            gestureDetector.Evaluate(testHand.animatedPose.handPose);
        }
    }

}