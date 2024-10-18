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
            if (OnStart == null) OnStart = new();
            if (OnHold == null) OnHold = new();
            if (OnEnd == null) OnEnd = new();
            if (OnCancel == null) OnCancel = new();
        }

        public abstract void Evaluate(Vector3 position);
        
    }
}
