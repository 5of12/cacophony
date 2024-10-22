using UnityEngine;

namespace Cacophony
{
    [CreateAssetMenu(menuName = "Cacophony/DetectableHandPose", fileName = "HandPose")]
    public class DetectableHandPose : IDetectable<SimpleHandPose>
    {
        public SimpleHandPose handPose;
        
        public override float Evaluate(SimpleHandPose input)
        {
            // Sum the dot products between available finger curls and normalise
            // Sum the dot products between palm directions
            // Return the product

            float confidence = 0;
            float count = 0;

            for(int i = 0; i < handPose.fingers.Length; i++)
            {
                confidence += CorrelatePose(input.fingers[i], handPose.fingers[i]);
                count += 1f;
            }
            float fingerConfidence = 1 - (confidence / count);

            float dirDif = handPose.palmDirection != Vector3.zero ? Vector3.Dot(input.palmDirection.normalized, handPose.palmDirection.normalized) : 1;
            float normDif = handPose.palmNormal != Vector3.zero ? Vector3.Dot(input.palmNormal.normalized, handPose.palmNormal.normalized) : 1;
            float palmConfidence = dirDif * normDif > 0.5f ? 1 : 0;

            return fingerConfidence * palmConfidence;
        }

        private float CorrelatePose(SimpleFinger a, SimpleFinger b)
        {
            return (Mathf.Abs(a.curl - b.curl) + Mathf.Abs(a.bend - b.bend) + Mathf.Abs(a.splay - b.splay)) / 3f;
        }
    }
}
