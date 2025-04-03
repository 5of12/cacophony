using UnityEngine;

namespace Cacophony {
    [CreateAssetMenu(menuName = "Cacophony/DirectionMoveAction", fileName = "Direction")]
    public class DirectionMoveAction : IActionProcessor
    {

        [Tooltip("Distance source can move before the gesture is cancelled")]
        public float minDistanceM;

        [Tooltip("Direction in which to move, zero vector allows move in any direction")]
        public Vector3 moveDirection;

        [Tooltip("Angle of the cone for allowed movement along the direction")]
        [Range(10,45)]
        public float directionTolerence = 45;

        private Vector3 currentPosition;
        private Vector3 startPosition;
        private bool detecting = false;

        public override void Initialise(IDetectionSource detector)
        {
            base.Initialise();
            detector.OnStart.AddListener( HandleStart );
            detector.OnHold.AddListener( HandleHold );
            detector.OnEnd.AddListener( HandleEnd );
            detector.OnCancel.AddListener( () => OnCancel?.Invoke(new ActionEventArgs { eventType = ActionEventType.CANCEL } ) );
        }
        
        public override void Evaluate(Vector3 position)
        {
            currentPosition = position;
        }
        
        private void HandleStart()
        {
            startPosition = currentPosition;
            OnStart?.Invoke(new ActionEventArgs { position = currentPosition, progress = 0, eventType = ActionEventType.START });
            detecting = true;
        }

        private void HandleHold()
        {
            if (detecting)
            {
                float angle = moveDirection.magnitude > 0 ? Vector3.Angle(moveDirection, currentPosition - startPosition) : 0;
                float distanceMoved = angle < directionTolerence ? Vector3.Distance(startPosition, currentPosition) : 0;
                bool sufficientlyMoved = distanceMoved >= minDistanceM;

                if (sufficientlyMoved)
                {
                    // Should we set progress = 1?
                    OnEnd?.Invoke(new ActionEventArgs { position = currentPosition, eventType = ActionEventType.COMPLETE });
                    detecting = false;   
                }
                else
                {
                    OnHold?.Invoke(new ActionEventArgs { position = currentPosition, progress = distanceMoved / minDistanceM, eventType = ActionEventType.INPROGRESS });
                }
            }
        }

        private void HandleEnd()
        {
            if (detecting)
            {
                OnCancel?.Invoke(new ActionEventArgs { position = currentPosition, progress = 0, eventType = ActionEventType.CANCEL });
            }
            detecting = false;
        }
    }
}
