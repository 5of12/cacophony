using NUnit.Framework;
using UnityEngine;

using Cacophony;
using Cacophony.Testing;

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
        gesture.positivePoses = new();
        gesture.negativePoses = new();
        gesture.confidenceThreshold = 0.95f;
        detector.handGesture = gesture;
    }

    [TearDown]
    public void ResetDetector()
    {
        gesture.positivePoses = new();
        gesture.negativePoses = new();        
        gesture.confidenceThreshold = 0.95f;
    }

    #region State machine Tests
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
    #endregion

    #region EvaluationTests

    [Test]
    public void PerfectMatchingPoseTriggersHoldAsBufferFills()
    {
        detector.Initialise();
        detector.state = GestureState.DETECTING;
        detector.detectorOn = true;
        SimpleHandPose fistPose = Helper.CreateFistPose();
        DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
        detectable.handPose = fistPose;
        gesture.positivePoses.Add(detectable);

        detector.Evaluate(fistPose);
        Assert.AreEqual(GestureState.DETECTING, detector.state);
        detector.Evaluate(fistPose);
        Assert.AreEqual(GestureState.HOLD, detector.state);
        detector.Evaluate(fistPose);
        Assert.AreEqual(GestureState.HOLD, detector.state);
    }

    [Test]
    public void PerfectMatchingPoseTriggersHoldImmediatelyWhenThresholdLow()
    {
        detector.Initialise();
        detector.state = GestureState.DETECTING;
        detector.detectorOn = true;
        SimpleHandPose fistPose = Helper.CreateFistPose();
        DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
        detectable.handPose = fistPose;
        gesture.positivePoses.Add(detectable);
        gesture.confidenceThreshold = 0.05f;

        detector.Evaluate(fistPose);
        Assert.AreEqual(GestureState.HOLD, detector.state);
    }

    [Test]
    public void PoorlyMatchingPoseDoesNotTriggerHoldWhenBufferFull()
    {
        detector.Initialise();
        detector.state = GestureState.DETECTING;
        detector.detectorOn = true;
        SimpleHandPose pointPose = Helper.CreatePointingPose();
        SimpleHandPose extendPose = Helper.CreateExtendedPose();
        DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
        detectable.handPose = pointPose;
        gesture.positivePoses.Add(detectable);

        detector.Evaluate(extendPose);
        detector.Evaluate(extendPose);
        detector.Evaluate(extendPose);
        Assert.AreEqual(GestureState.DETECTING, detector.state);
    }

    #endregion
}


