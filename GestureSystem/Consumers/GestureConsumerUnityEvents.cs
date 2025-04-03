using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Cacophony
{
    public class GestureConsumer : MonoBehaviour
    {
        public HandGestureManager manager;
        private IEnumerator resetRoutine;

        [Header("Events")]
        public UnityEvent OnGestureStart;
        public UnityEvent<float> OnGestureHoldWithProgress;
        public UnityEvent<Vector3> OnGestureHoldWithPosition;
        public UnityEvent OnGestureEnd;
        public UnityEvent OnGestureCancel;

        private void Awake()
        {
            if (manager == null)
            {
                manager = FindFirstObjectByType<HandGestureManager>();
                if (manager == null)
                {
                    Debug.LogError("Could not find HandGestureManager for GestureConsumer: " + name);
                }
            }
        }

        void OnEnable()
        {
            if (manager != null)
            {
                Initialise();
            }
        }

        public void Initialise()
        {
            manager.actionProcessor.OnStart.AddListener(HandleStart);
            manager.actionProcessor.OnHold.AddListener(HandleHold);
            manager.actionProcessor.OnEnd.AddListener(HandleEnd);
            manager.actionProcessor.OnCancel.AddListener(HandleCancel); 
            resetRoutine = Reset();
        }

        void OnDisable()
        {
            manager.actionProcessor.OnStart.RemoveListener(HandleStart);
            manager.actionProcessor.OnHold.RemoveListener(HandleHold);
            manager.actionProcessor.OnEnd.RemoveListener(HandleEnd);
            manager.actionProcessor.OnCancel.RemoveListener(HandleCancel);
        }

        protected virtual void HandleStart(ActionEventArgs pos)
        {
            //Debug.Log("HandleStart");
            OnGestureStart.Invoke();
            StopCoroutine(resetRoutine);
        }

        protected virtual void HandleHold(ActionEventArgs pos)
        {
            //Debug.Log("HandleHold:" + pos.progress + );
            OnGestureHoldWithProgress.Invoke(pos.progress);
            OnGestureHoldWithPosition.Invoke(pos.position);
        }

        protected virtual void HandleEnd(ActionEventArgs pos)
        {
            //Debug.Log("HandleEnd");
            OnGestureEnd.Invoke(); 
            StopCoroutine(resetRoutine);
            resetRoutine = Reset();
            StartCoroutine(resetRoutine);
        }

        protected virtual void HandleCancel()
        {
            //Debug.Log("HandleCancel");
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
    
      
