using UnityEngine;

namespace Cacophony 
{
    public abstract class IGestureManager<T> : MonoBehaviour
    {
        public IGestureDetector<T> gestureDetector;
        public IActionProcessor actionProcessor;
        [Tooltip("Initialise on start only if configuring from the inspector")]
        public bool initOnStart = false;

        void Start()
        {
            if(initOnStart && actionProcessor != null && gestureDetector != null)
            {
                actionProcessor.Initialise(gestureDetector);
            }
        }

        public virtual void Evaluate(Vector3 pos, T data)
        {
            actionProcessor.Evaluate(pos);
            gestureDetector.Evaluate(data);
        }
    }

}