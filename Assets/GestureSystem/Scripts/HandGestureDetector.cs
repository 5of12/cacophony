using UnityEngine;
using UnityEngine.Events;

namespace Cacophony
{
    public enum GestureState { IDLE, DETECTING, HOLD, RESET}
    
    public class HandGestureDetector : IGestureDetector<SimpleHandPose>
    {
        public HandGestureDefinition handGesture;

        private ConfidenceBuffer _confidenceBuffer;
        
        [SerializeField]
        private float _confidence;

        public GestureState state;

        [Header("Debugging")]
        public bool detectorOn = false;
        public bool reset = false;


        void OnEnable()
        {
            _confidenceBuffer = new ConfidenceBuffer(3);
            state = GestureState.IDLE;
        }

        public override void Evaluate(SimpleHandPose pose)
        {
            _confidence = UpdateConfidence(pose);
            ResolveState(_confidence);
        }

        public float UpdateConfidence(SimpleHandPose input)
        {
            float currentSample = handGesture.Evaluate(input);
        
            return _confidenceBuffer.SmoothConfidence(currentSample);
        }

        private void ResolveState(float currentConfidence)
        {
            if (reset) state = GestureState.RESET;

            switch (state)
            {
                case GestureState.IDLE:
                    state = detectorOn ? GestureState.DETECTING : GestureState.IDLE;
                    break;
                case GestureState.DETECTING:
                    if (currentConfidence >= handGesture.confidenceThreshold)
                    {
                        OnStart.Invoke();
                        state = GestureState.HOLD;
                    }
                    break;
                case GestureState.HOLD:
                    if (currentConfidence < handGesture.confidenceThreshold)
                    {
                        OnEnd.Invoke();
                        state = GestureState.DETECTING;
                    }
                    else 
                    {
                        OnHold.Invoke();
                    }
                    break;
                case GestureState.RESET:
                    state = GestureState.IDLE;
                    OnCancel.Invoke();
                    detectorOn = false;
                    reset = false;
                    break;
            }
        }
    }
}