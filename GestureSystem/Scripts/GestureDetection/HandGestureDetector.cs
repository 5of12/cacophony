using UnityEngine;

namespace Cacophony
{
    public enum GestureState { IDLE, DETECTING, READY, HOLD, RESET}
    
    public class HandGestureDetector : IGestureDetector<SimpleHandPose>
    {
        public HandGestureDefinition readyGesture;
        public HandGestureDefinition handGesture;
        private HandGestureDefinition _activeGesture;

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
            SetGesture(readyGesture);
        }
        public override void DisableDetector()
        {
            detectorOn = false;
            reset = true;
        }

        public void Initialise()
        {
            state = GestureState.IDLE;
        }

        public override void Evaluate(SimpleHandPose pose)
        {
            if (_activeGesture != null)
            {
                float previousConfidence = _confidence;
                _confidence = UpdateConfidence(pose);
                float futureConfidence = _confidence + (_confidence - previousConfidence);
                ResolveState(futureConfidence);
            }
        }

        public float UpdateConfidence(SimpleHandPose input)
        {
            float currentSample = _activeGesture.Evaluate(input);
        
            return _confidenceBuffer.SmoothConfidence(currentSample);
        }

        public void ResolveState(float currentConfidence)
        {
            if (reset) state = GestureState.RESET;

            switch (state)
            {
                case GestureState.IDLE:
                    if (detectorOn)
                    {
                        state = GestureState.DETECTING;
                    }
                    break;
                case GestureState.DETECTING:
                    if (currentConfidence > readyGesture.confidenceThreshold)
                    {
                        SetGesture(handGesture);
                        state = GestureState.READY;
                    }
                    break;
                case GestureState.READY:
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
                        SetGesture(readyGesture);
                    }
                    else 
                    {
                        OnHold?.Invoke();
                    }
                    break;
                case GestureState.RESET:
                    state = GestureState.IDLE;
                    SetGesture(readyGesture);
                    OnCancel?.Invoke();
                    detectorOn = false;
                    reset = false;
                    break;
            }
        }

        private void SetGesture(HandGestureDefinition gesture)
        {
            _activeGesture = gesture;
            _confidenceBuffer = new ConfidenceBuffer(3);
            _confidence = 0;
        }

        public float GetConfidence()
        {
            return _confidence;
        }
    }
}