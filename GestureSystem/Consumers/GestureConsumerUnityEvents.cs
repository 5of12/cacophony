using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Cacophony
{
    public class GestureConsumerUnityEvents : IGestureConsumer
    {
        private IEnumerator resetRoutine;

        [Header("Events")]
        public UnityEvent OnGestureStart;
        public UnityEvent<float> OnGestureHoldWithProgress;
        public UnityEvent<Vector3> OnGestureHoldWithPosition;
        public UnityEvent OnGestureEnd;
        public UnityEvent OnGestureCancel;

        protected override void Initialise()
        {
            base.Initialise();
            resetRoutine = Reset();
        }

        protected override void HandleStart(ActionEventArgs pos)
        {
            OnGestureStart.Invoke();
            StopCoroutine(resetRoutine);
        }

        protected override void HandleHold(ActionEventArgs pos)
        {
            OnGestureHoldWithProgress.Invoke(pos.progress);
            OnGestureHoldWithPosition.Invoke(pos.position);
        }

        protected override void HandleEnd(ActionEventArgs pos)
        {
            OnGestureEnd.Invoke(); 
            StopCoroutine(resetRoutine);
            resetRoutine = Reset();
            StartCoroutine(resetRoutine);
        }

        protected override void HandleCancel(ActionEventArgs pos)
        {
            OnGestureCancel.Invoke();
            StopCoroutine(resetRoutine);
            resetRoutine = Reset();
            StartCoroutine(resetRoutine);
        }

        IEnumerator Reset()
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
}
    
      
