using UnityEngine;

namespace Cacophony 
{

    public class GestureManager : MonoBehaviour
    {
        public HandGestureDetector gestureDetector;
        public AnimatedTestHand testHand;
        public IActionProcessor actionProcessor;

        void Start()
        {
            actionProcessor.Initialise(gestureDetector);
        }

        void Update()
        {
            actionProcessor.Evaluate(Vector3.zero);

            gestureDetector.Evaluate(testHand.animatedPose.handPose);
        }
    }

}