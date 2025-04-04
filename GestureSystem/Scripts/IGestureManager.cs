using UnityEngine;
using UnityEngine.Events;

namespace Cacophony 
{
    public abstract class IGestureManager<T> : MonoBehaviour
    {
        [Tooltip("The gesture detector to use for this manager. Initialised on start only")]
        public IGestureDetector<T> gestureDetector;
        [Tooltip("The action processor to use for this manager. Initialised on start only")]
        public IActionProcessor actionProcessor;
        [Tooltip("Initialise on start only if configuring from the inspector")]
        public bool initOnStart = true;

        [Tooltip("Event to observe when the manager has completed initialisation")]
        public UnityEvent Ready;
        private bool ready = false;

        void Start()
        {
            if(initOnStart && actionProcessor != null && gestureDetector != null)
            {
                var ap = Instantiate(actionProcessor);
                actionProcessor = ap;
                actionProcessor.Initialise(gestureDetector);
                ready = true;
                Ready.Invoke();
            }
        }

        public virtual void Evaluate(Vector3 pos, T data)
        {
            actionProcessor.Evaluate(pos);
            gestureDetector.Evaluate(data);
        }

        public bool IsReady()
        {
            return ready;
        }
    }

}