using NUnit.Framework;
using UnityEngine;

namespace Cacophony 
{
namespace Testing
{
    public class HandGestureDetectorTest
    {
        public GameObject gameObject;
        public HandGestureDetector detector;
        public HandGestureDefinition gesture;


        [OneTimeSetUp]
        public void SetupDetector()
        {
            gameObject = new GameObject();
            detector = gameObject.AddComponent<HandGestureDetector>();
            gesture = ScriptableObject.CreateInstance<HandGestureDefinition>();
            gesture.confidenceThreshold = 0.95f;
            detector.handGesture = gesture;
        }

        // A Test behaves as an ordinary method
        [Test]
        public void IdleStateMovesToDetectingWhenEnabled()
        {
            detector.detectorOn = false;
            detector.reset = false;
            detector.state = GestureState.IDLE;

            detector.ResolveState(0);
            Assert.AreEqual(GestureState.IDLE, detector.state);
            detector.detectorOn = true;
            detector.ResolveState(0);
            Assert.AreEqual(GestureState.DETECTING, detector.state);

        }

        [Test]
        public void DetectingStateMovesToHoldWhenConfidenceHigh()
        {
            detector.state = GestureState.DETECTING;

            detector.ResolveState(1);
            Assert.AreEqual(GestureState.HOLD, detector.state);
        }

        [Test]
        public void HoldStateMovesToDetectingWhenConfidenceLow()
        {
            detector.state = GestureState.HOLD;

            detector.ResolveState(0);
            Assert.AreEqual(GestureState.DETECTING, detector.state);
        }

        [Test]
        public void ResetTriggeredDuringDetectingReturnsToIdle()
        {
            detector.state = GestureState.DETECTING;
            detector.detectorOn = true;
            
            detector.reset = true;
            detector.ResolveState(1);

            Assert.AreEqual(GestureState.IDLE, detector.state);
            Assert.AreEqual(false, detector.detectorOn);
            Assert.AreEqual(false, detector.reset);
        }

        [Test]
        public void ResetTriggeredDuringHoldReturnsToIdle()
        {
            detector.state = GestureState.HOLD;
            detector.detectorOn = true;
            
            detector.reset = true;
            detector.ResolveState(1);

            Assert.AreEqual(GestureState.IDLE, detector.state);
            Assert.AreEqual(false, detector.detectorOn);
            Assert.AreEqual(false, detector.reset);
        }
    }
}
}

