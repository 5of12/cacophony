using System.Collections;
using System.Collections.Generic;
using Cacophony;
using UnityEngine;
using UnityEngine.Events;

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
            OnStart?.Invoke();
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
                        OnEnd?.Invoke();
                        detecting = false;
                    }
                    else
                    {
                        OnHold?.Invoke();
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
