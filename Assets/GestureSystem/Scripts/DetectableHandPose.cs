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

            float dirDif = Vector3.Dot(input.palmDirection, handPose.palmDirection);
            float normDif = Vector3.Dot(input.palmNormal, handPose.palmNormal);
            float palmConfidence = (dirDif + normDif) / 2;

            return fingerConfidence * palmConfidence;
        }

        private float CorrelatePose(SimpleFinger a, SimpleFinger b)
        {
            return (Mathf.Abs(a.curl - b.curl) + Mathf.Abs(a.bend - b.bend) + Mathf.Abs(a.splay - b.splay)) / 3f;
        }
    }
}
