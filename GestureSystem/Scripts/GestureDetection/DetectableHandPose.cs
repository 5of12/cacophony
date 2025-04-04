using UnityEngine;

namespace Cacophony
{
    [CreateAssetMenu(menuName = "Cacophony/DetectableHandPose", fileName = "HandPose")]
    public class DetectableHandPose : IDetectable<SimpleHandPose>
    {
        public SimpleHandPose handPose;

        public float curlContribution = 1f;
        public float bendContribution = 1f;
        public float splayContribution = 1f;
        public float directionContribution = 1f;

        public override float Evaluate(SimpleHandPose input)
        {
            float confidence = 0;
            float count = 0;

            for(int i = 0; i < handPose.fingers.Length; i++)
            {
                float poseConfidence = CorrelatePose(input.fingers[i], handPose.fingers[i]);
                confidence += poseConfidence > 0.5f ? poseConfidence : 0;
                count += 1f;
            }
            float fingerConfidence = confidence / count;

            float dirDif = handPose.palmDirection != Vector3.zero ? Vector3.Dot(input.palmDirection.normalized, handPose.palmDirection.normalized) : 1;
            float normDif = handPose.palmNormal != Vector3.zero ? Vector3.Dot(input.palmNormal.normalized, handPose.palmNormal.normalized) : 1;
            float palmConfidence = Mathf.Lerp(1, dirDif * normDif, directionContribution);

            return Mathf.Clamp01(fingerConfidence * palmConfidence);
        }

        private float CorrelatePose(SimpleFinger a, SimpleFinger b)
        {
            float divisor = curlContribution + bendContribution + splayContribution;
            float curl = Mathf.Abs(a.curl - b.curl) * curlContribution;
            float bend = Mathf.Abs(a.bend - b.bend) * bendContribution;
            float splay = Mathf.Abs(a.splay - b.splay) * splayContribution;

            return 1 - ((bend + curl + splay) / divisor);
        }
    }
}
