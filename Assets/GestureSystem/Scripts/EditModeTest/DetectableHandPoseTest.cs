using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

namespace Cacophony
{
    public class DetectableHandPoseTest
    {

        private SimpleHandPose CreateFistPose()
        {
            SimpleHandPose pose = new SimpleHandPose();
            foreach(var finger in pose.fingers)
            {
                finger.curl = 1;
                finger.bend = 1;
                finger.splay = 0;
            }
            pose.palmNormal = new Vector3(0 -1, 0);
            pose.palmDirection = new Vector3(0, 0, 1);
            return pose;
        }

        private SimpleHandPose CreatePointingPose()
        {
            SimpleHandPose pose = CreateFistPose();
            pose.index.curl = 0;
            pose.index.bend = 0;

            return pose;
        }

        private SimpleHandPose CreateExtendedPose()
        {
            SimpleHandPose pose = new SimpleHandPose();
            foreach(var finger in pose.fingers)
            {
                finger.splay = 1f;
            }
            pose.palmNormal = new Vector3(0 -1, 0);
            pose.palmDirection = new Vector3(0, 0, 1);
            return pose;
        }


        [Test]
        public void EvaluatingTheSamePoseReturnsFullConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            float confidence = detectable.Evaluate(poseA);

            Assert.AreEqual(1, confidence);
        }

        [Test]
        public void EvaluatingOppositePosesReturnsLowConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            SimpleHandPose poseB = CreateExtendedPose();
            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            float confidence = detectable.Evaluate(poseB);

            Assert.Less(confidence, 0.1f);
        }

        [Test]
        public void EvaluatingSimilarPosesReturnsModerateConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            SimpleHandPose poseB = CreatePointingPose();
            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            float confidence = detectable.Evaluate(poseB);
            Assert.Greater(confidence, 0f);
            Assert.Less(confidence, 1f);
        }

        [Test]
        public void OrthoganolPalmDirectionWithAlignedNormalReturnsZeroConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            SimpleHandPose poseB = CreateFistPose();

            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            // Rotate the palm 90 around the normal 
            poseB.palmDirection = new Vector3(1, 0, 0);
            float confidence = detectable.Evaluate(poseB);

            Assert.AreEqual(0, confidence);
        }

        [Test]
        public void OrthoganolPalmDirectionAndNormalReturnsZeroConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            SimpleHandPose poseB = CreateFistPose();

            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            // Rotate the palm 90 around the normal and the normal about the direction
            poseB.palmDirection = new Vector3(1, 0, 0);
            poseB.palmNormal = new Vector3(0, 0, -1);

            float confidence = detectable.Evaluate(poseB);
            Assert.AreEqual(0f, confidence);
        }

        [Test]
        public void InversePalmDirectionReturnsZeroConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            SimpleHandPose poseB = CreateFistPose();

            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            // Rotate the palm 90 around the normal and the normal about the direction
            poseB.palmDirection = new Vector3(0, -1, 0);
            float confidence = detectable.Evaluate(poseB);

            Assert.AreEqual(0f, confidence);
        }

        [Test]
        public void ZeroVectorDirectionDoesntAffectConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            SimpleHandPose poseB = CreateFistPose();

            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            // When
            poseB.palmDirection = new Vector3(0, 0, 0);
            float confidence = detectable.Evaluate(poseB);
            
            Assert.AreEqual(1f, confidence);
        }

        [Test]
        public void NonUnitVectorDirectionDoesntAffectConfidence()
        {
            SimpleHandPose poseA = CreateFistPose();
            SimpleHandPose poseB = CreateFistPose();

            DetectableHandPose detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
            detectable.handPose = poseA;
            
            // When 
            poseA.palmDirection = new Vector3(0, -1, 0);
            poseB.palmDirection = new Vector3(0, -2, 0);
            float confidence = detectable.Evaluate(poseB);

            Assert.AreEqual(1f, confidence);
        }
    }

}
