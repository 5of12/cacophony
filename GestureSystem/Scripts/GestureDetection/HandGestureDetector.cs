using UnityEngine;

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

        [Header("Exposed for Debugging")]
        public bool detectorOn = false;
        public bool reset = false;


        void OnEnable()
        {
            Initialise();
        }

        public override void EnableDetector()
        {
            detectorOn = true;
        }
        public override void DisableDetector()
        {
            detectorOn = false;
            reset = true;
        }

        public void Initialise()
        {
            _confidenceBuffer = new ConfidenceBuffer(3);
            state = GestureState.IDLE;
            _confidence = 0;
        }

        public override void Evaluate(SimpleHandPose pose)
        {
            float previousConfidence = _confidence;
            _confidence = UpdateConfidence(pose);
            float futureConfidence = _confidence + (_confidence - previousConfidence);
            ResolveState(futureConfidence);
        }

        public float UpdateConfidence(SimpleHandPose input)
        {
            float currentSample = handGesture.Evaluate(input);
        
            return _confidenceBuffer.SmoothConfidence(currentSample);
        }

        public void ResolveState(float currentConfidence)
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
                        OnStart?.Invoke();
                        state = GestureState.HOLD;
                    }
                    break;
                case GestureState.HOLD:
                    if (currentConfidence < handGesture.confidenceThreshold)
                    {
                        OnEnd?.Invoke();
                        state = GestureState.DETECTING;
                    }
                    else 
                    {
                        OnHold?.Invoke();
                    }
                    break;
                case GestureState.RESET:
                    state = GestureState.IDLE;
                    OnCancel?.Invoke();
                    detectorOn = false;
                    reset = false;
                    break;
            }
        }

        public float GetConfidence()
        {
            return _confidence;
        }
    }
}