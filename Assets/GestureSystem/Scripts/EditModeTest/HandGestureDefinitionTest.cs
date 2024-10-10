using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Cacophony;
using Cacophony.Testing;

public class HandGestureDefinitionTest
{
    public HandGestureDefinition definition;
    public DetectableHandPose fistPose;
    public DetectableHandPose pointPose;
    public DetectableHandPose extendPose;

    [OneTimeSetUp]
    public void SetupGestureDefinition()
    {
        definition = ScriptableObject.CreateInstance<HandGestureDefinition>();
        definition.positivePoses = new();
        definition.negativePoses = new();

        fistPose = ScriptableObject.CreateInstance<DetectableHandPose>();
        fistPose.handPose = Helper.CreateFistPose();

        pointPose = ScriptableObject.CreateInstance<DetectableHandPose>();
        pointPose.handPose = Helper.CreatePointingPose();

        extendPose = ScriptableObject.CreateInstance<DetectableHandPose>();
        extendPose.handPose = Helper.CreateExtendedPose();
    }

    [TearDown]
    public void ResetGestureDefinition()
    {
        definition.positivePoses.Clear();
        definition.negativePoses.Clear();
    }

    [Test]
    public void IdenticalNegativePoseNegatesPerfectPositiveMatch()
    {
        definition.positivePoses.Add(fistPose);
        definition.negativePoses.Add(fistPose);

        float confidence = definition.Evaluate(fistPose.handPose);
        Assert.AreEqual(0, confidence);
    }

    [Test]
    public void InverseNegativePoseDoesNotAffectPerfectMatch()
    {
        definition.positivePoses.Add(fistPose);
        definition.negativePoses.Add(extendPose);

        float confidence = definition.Evaluate(fistPose.handPose);
        Assert.AreEqual(1, confidence);
    }

    [Test]
    public void MatchToOneOfTwoPositivesReturnsHighestConfidence()
    {
        definition.positivePoses.Add(fistPose);
        definition.positivePoses.Add(pointPose);

        float confidence = definition.Evaluate(fistPose.handPose);
        Assert.AreEqual(1, confidence);

        confidence = definition.Evaluate(pointPose.handPose);
        Assert.AreEqual(1, confidence);
    }

    [Test]
    public void PartialMatchToOneOfTwoReturnsHighestConfidence()
    {
        definition.positivePoses.Add(fistPose);
        definition.positivePoses.Add(pointPose);

        float confidence = definition.Evaluate(extendPose.handPose);
        Assert.Greater(confidence, 0);
        Assert.Less(confidence, 1);
    }

    [Test]
    public void MatchingOnlyNegativeResultReturnsZero()
    {
        definition.negativePoses.Add(fistPose);

        float confidence = definition.Evaluate(fistPose.handPose);
        Assert.AreEqual(0, confidence);
    }

}
