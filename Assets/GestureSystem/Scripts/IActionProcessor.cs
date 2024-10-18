using UnityEngine;
using UnityEngine.Events;

namespace Cacophony {
    public abstract class IActionProcessor : ScriptableObject
    {
        [HideInInspector]
        public UnityEvent OnStart;
        [HideInInspector]
        public UnityEvent OnHold;
        [HideInInspector]
        public UnityEvent OnEnd;
        [HideInInspector]
        public UnityEvent OnCancel;

        public virtual void Initialise(IDetectionSource detector = null)
        {
            OnStart = new();
            OnHold = new();
            OnEnd = new();
            OnCancel = new();
        }

        public abstract void Evaluate(Vector3 position);
        
    }
}
