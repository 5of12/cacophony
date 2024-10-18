using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cacophony {
    [CreateAssetMenu(menuName = "Cacophony/PassthroughAction", fileName = "Passthrough")]
    public class PassthroughAction : IActionProcessor
    {
        [Tooltip("Set a distance to filter out small movements")]
        [Range(0,1)]
        public float deadzoneDistance;

        private Vector3 currentPosition;
        private Vector3 startPosition;

        public override void Initialise(IDetectionSource detector)
        {
            detector.OnStart.AddListener( HandleStart );
            detector.OnHold.AddListener( HandleHold );
            detector.OnEnd.AddListener( () => OnEnd?.Invoke() );
            detector.OnCancel.AddListener( () => OnCancel?.Invoke() );
        }

        public override void Evaluate(Vector3 position)
        {
            currentPosition = position;
        }
        
        private void HandleStart()
        {
            startPosition = currentPosition;
            OnStart?.Invoke();
        }

        private void HandleHold()
        {
            if (Vector3.Distance(currentPosition, startPosition) >= deadzoneDistance)
            {
                OnHold?.Invoke();
            }
        }
    }
}
