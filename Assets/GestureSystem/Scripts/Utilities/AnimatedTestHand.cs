using UnityEngine;

namespace Cacophony
{
    public class AnimatedTestHand : MonoBehaviour
    {
        public DetectableHandPose startPose;
        public DetectableHandPose endPose;

        public DetectableHandPose animatedPose;

        public float speed = 1;

        void Start()
        {
            if (animatedPose == null) animatedPose = ScriptableObject.CreateInstance<DetectableHandPose>();
        }

        void Update()
        {
            float time = Mathf.PingPong(Time.time * speed, 1);
            
            animatedPose.handPose.thumb.curl  = Mathf.Lerp(startPose.handPose.thumb.curl, endPose.handPose.thumb.curl, time);
            animatedPose.handPose.thumb.bend  = Mathf.Lerp(startPose.handPose.thumb.bend, endPose.handPose.thumb.bend, time);
            animatedPose.handPose.thumb.splay = Mathf.Lerp(startPose.handPose.thumb.splay, endPose.handPose.thumb.splay, time);

            animatedPose.handPose.index.curl  = Mathf.Lerp(startPose.handPose.index.curl, endPose.handPose.index.curl, time);
            animatedPose.handPose.index.bend  = Mathf.Lerp(startPose.handPose.index.bend, endPose.handPose.index.bend, time);
            animatedPose.handPose.index.splay = Mathf.Lerp(startPose.handPose.index.splay, endPose.handPose.index.splay, time);

            animatedPose.handPose.middle.curl = Mathf.Lerp(startPose.handPose.middle.curl, endPose.handPose.middle.curl, time);
            animatedPose.handPose.middle.bend = Mathf.Lerp(startPose.handPose.middle.bend, endPose.handPose.middle.bend, time);
            animatedPose.handPose.middle.splay= Mathf.Lerp(startPose.handPose.middle.splay, endPose.handPose.middle.splay, time);

            animatedPose.handPose.ring.curl   = Mathf.Lerp(startPose.handPose.ring.curl, endPose.handPose.ring.curl, time);
            animatedPose.handPose.ring.bend   = Mathf.Lerp(startPose.handPose.ring.bend, endPose.handPose.ring.bend, time);
            animatedPose.handPose.ring.splay  = Mathf.Lerp(startPose.handPose.ring.splay, endPose.handPose.ring.splay, time);

            animatedPose.handPose.pinky.curl  = Mathf.Lerp(startPose.handPose.pinky.curl, endPose.handPose.pinky.curl, time);
            animatedPose.handPose.pinky.bend  = Mathf.Lerp(startPose.handPose.pinky.bend, endPose.handPose.pinky.bend, time);
            animatedPose.handPose.pinky.splay = Mathf.Lerp(startPose.handPose.pinky.splay, endPose.handPose.pinky.splay, time);

            animatedPose.handPose.palmDirection = Vector3.Lerp(startPose.handPose.palmDirection, endPose.handPose.palmDirection, time);
            animatedPose.handPose.palmNormal = Vector3.Lerp(startPose.handPose.palmNormal, endPose.handPose.palmNormal, time);
        }

    }
}