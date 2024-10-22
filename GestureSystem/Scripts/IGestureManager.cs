using UnityEngine;

namespace Cacophony 
{
    public abstract class IGestureManager<T> : MonoBehaviour
    {
        public IGestureDetector<T> gestureDetector;
        public IActionProcessor actionProcessor;

        void Start()
        {
            if(actionProcessor != null && gestureDetector != null)
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