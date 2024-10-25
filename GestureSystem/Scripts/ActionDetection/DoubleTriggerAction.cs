using UnityEngine;

namespace Cacophony {
    [CreateAssetMenu(menuName = "Cacophony/DoubleTriggerAction", fileName = "Double Trigger")]
    
    public class DoubleTriggerAction : IActionProcessor
    {
        private Vector3 currentPosition;
        [Tooltip("If the user performs a successful Trigger within this time threshold, it will fire OnEnd.")]
        [Range(0, 2)]
        [SerializeField] private float _maxTimeToConsiderDoubleTrigger = 0.558f;
        private float _timeOfLastTrigger = -1000f;
        private bool _waitingForFirstTrigger = false;
        private bool detecting = false;
        
        public override void Initialise(IDetectionSource detector)
        {
            base.Initialise();
            detector.OnStart.AddListener( HandleStart );
            
            // There is no notion of 'Hold' for Double Trigger, it's two impulses...
            //detector.OnHold.AddListener( HandleHold );
            detector.OnEnd.AddListener( HandleEnd );
            detector.OnCancel.AddListener( () => OnCancel?.Invoke() );
        }
        
        public override void Evaluate(Vector3 position)
        {
            currentPosition = position;
        }

        private void OnStartTriggered()
        {
            float deltaTriggerTime = Time.time - _timeOfLastTrigger;
            _timeOfLastTrigger = Time.time;

            if (_waitingForFirstTrigger)
            {
                // Trigger #1
                _waitingForFirstTrigger = false;
                return;
            }

            if (deltaTriggerTime <= _maxTimeToConsiderDoubleTrigger && !_waitingForFirstTrigger)
            {
                // Trigger #2
                OnEnd?.Invoke(new ActionEventArgs { position = currentPosition });
                _waitingForFirstTrigger = true;
                detecting = false;
            }
        }          
        
        private void HandleStart()
        {
            OnStart?.Invoke(new ActionEventArgs { position = currentPosition });
            detecting = true;
            OnStartTriggered();
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
