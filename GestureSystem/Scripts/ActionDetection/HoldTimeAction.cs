using UnityEngine;

namespace Cacophony {
    [CreateAssetMenu(menuName = "Cacophony/HoldTimeAction", fileName = "Hold")]
    public class HoldTimeAction : IActionProcessor
    {
        [Tooltip("Time gesture must be held to trigger the action")]
        public float holdTimeS;

        [Tooltip("Distance source can move before the gesture is cancelled")]
        public float maxDistanceM;

        private float startTime;
        private Vector3 currentPosition;
        private Vector3 startPosition;
        private bool detecting = false;

        public override void Initialise(IDetectionSource detector)
        {
            base.Initialise();
            detector.OnStart.AddListener( HandleStart );
            detector.OnHold.AddListener( HandleHold );
            detector.OnEnd.AddListener( HandleEnd );
            detector.OnCancel.AddListener( () => OnCancel?.Invoke() );
        }
        
        public override void Evaluate(Vector3 position)
        {
            currentPosition = position;
        }
        
        private void HandleStart()
        {
            startPosition = currentPosition;
            startTime = Time.time;
            OnStart?.Invoke(new ActionEventArgs { position = currentPosition});
            detecting = true;
        }

        private void HandleHold()
        {
            if (detecting)
            {
                if (Vector3.Distance(currentPosition, startPosition) < maxDistanceM)
                {
                    if (Time.time - startTime > holdTimeS)
                    {
                        OnEnd?.Invoke(new ActionEventArgs { position = currentPosition });
                        detecting = false;
                    }
                    else
                    {
                        float time = Mathf.InverseLerp(startTime, startTime + holdTimeS, Time.time);
                        OnHold?.Invoke(new ActionEventArgs { position = currentPosition, progress = time });
                    }
                }
                else
                {
                    OnCancel?.Invoke();
                    detecting = false;
                }
            }
        }

        private void HandleEnd()
        {
            if (detecting)
            {
                OnCancel?.Invoke();
            }
            detecting = false;
        }
    }
}
