using UnityEngine;

namespace Cacophony {
    [CreateAssetMenu(menuName = "Cacophony/DoubleTriggerAction", fileName = "Double Trigger")]
    
    public class DoubleTriggerAction : IActionProcessor
    {
        private Vector3 currentPosition;
        [Tooltip("If the user performs a successful Trigger within this time threshold, it will fire OnEnd.")]
        [Range(0, 2)]
        [SerializeField] private float _maxTimeToConsiderDoubleTrigger = 0.558f;
        private float frameTolerance = 0.05f;
        private float _timeOfLastTrigger = -1000f;
        private bool _waitingForFirstTrigger = false;
        
        public override void Initialise(IDetectionSource detector)
        {
            base.Initialise();
            detector.OnStart.AddListener( HandleStart );
            // NOTE: There is no notion of 'Hold/End/Cancel' for Double Trigger Action
            // OnEnd is fired after the second trigger of OnStart
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
            }
            else if (deltaTriggerTime <= _maxTimeToConsiderDoubleTrigger)
            {
                // Trigger #2
                OnEnd?.Invoke(new ActionEventArgs { position = currentPosition });
                _waitingForFirstTrigger = true;
            }
        }          
        
        private void HandleStart()
        {
            if (_waitingForFirstTrigger){
                OnStart?.Invoke(new ActionEventArgs { position = currentPosition} );
            }
            else if (Time.time - _timeOfLastTrigger > (_maxTimeToConsiderDoubleTrigger + frameTolerance))
            {
                _waitingForFirstTrigger = true;
                OnStart?.Invoke(new ActionEventArgs { position = currentPosition });
            }
            OnStartTriggered();
        }
    }
}
