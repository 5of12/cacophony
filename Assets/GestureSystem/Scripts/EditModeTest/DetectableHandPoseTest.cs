using NUnit.Framework;
using UnityEngine;

namespace Cacophony
{
namespace Testing
{
    public class DetectableHandPoseTest
    {
        public DetectableHandPose detectable;

        [OneTimeSetUp]
        public void CreateDetectable()
        {
            detectable = ScriptableObject.CreateInstance<DetectableHandPose>();
        }

        [Test]
        public void EvaluatingTheSamePoseReturnsFullConfidence()
        {
            SimpleHandPose poseA = Helper.CreateFistPose();
            detectable.handPose = poseA;
            
            float confidence = detectable.Evaluate(poseA);

            Assert.AreEqual(1, confidence);
        }

        [Test]
        public void EvaluatingOppositePosesReturnsLowConfidence()
        {
            SimpleHandPose poseA = Helper.CreateFistPose();
            SimpleHandPose poseB = Helper.CreateExtendedPose();
            detectable.handPose = poseA;
            
            float confidence = detectable.Evaluate(poseB);

            Assert.Less(confidence, 0.1f);
        }

        [Test]
        public void EvaluatingSimilarPosesReturnsModerateConfidence()
        {
            SimpleHandPose poseA = Helper.CreateFistPose();
            SimpleHandPose poseB = Helper.CreatePointingPose();
            detectable.handPose = poseA;
            
            float confidence = detectable.Evaluate(poseB);
            Assert.Greater(confidence, 0f);
            Assert.Less(confidence, 1f);
        }

        [Test]
        public void OrthoganolPalmDirectionWithAlignedNormalReturnsZeroConfidence()
        {
            SimpleHandPose poseA = Helper.CreateFistPose();
            SimpleHandPose poseB = Helper.CreateFistPose();

            detectable.handPose = poseA;
            
            // Rotate the palm 90 around the normal 
            poseB.palmDirection = new Vector3(1, 0, 0);
            float confidence = detectable.Evaluate(poseB);

            Assert.AreEqual(0, confidence);
        }

        [Test]
        public void OrthoganolPalmDirectionAndNormalReturnsZeroConfidence()
        {
            SimpleHandPose poseA = Helper.CreateFistPose();
            SimpleHandPose poseB = Helper.CreateFistPose();

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
            SimpleHandPose poseA = Helper.CreateFistPose();
            SimpleHandPose poseB = Helper.CreateFistPose();

            detectable.handPose = poseA;
            
            // Rotate the palm 90 around the normal and the normal about the direction
            poseB.palmDirection = new Vector3(0, -1, 0);
            float confidence = detectable.Evaluate(poseB);

            Assert.AreEqual(0f, confidence);
        }

        [Test]
        public void ZeroVectorDirectionDoesntAffectConfidence()
        {
            SimpleHandPose poseA = Helper.CreateFistPose();
            SimpleHandPose poseB = Helper.CreateFistPose();

            detectable.handPose = poseA;
            
            // When
            poseB.palmDirection = new Vector3(0, 0, 0);
            float confidence = detectable.Evaluate(poseB);
            
            Assert.AreEqual(1f, confidence);
        }

        [Test]
        public void NonUnitVectorDirectionDoesntAffectConfidence()
        {
            SimpleHandPose poseA = Helper.CreateFistPose();
            SimpleHandPose poseB = Helper.CreateFistPose();

            detectable.handPose = poseA;
            
            // When 
            poseA.palmDirection = new Vector3(0, -1, 0);
            poseB.palmDirection = new Vector3(0, -2, 0);
            float confidence = detectable.Evaluate(poseB);

            Assert.AreEqual(1f, confidence);
        }
    }

}
}