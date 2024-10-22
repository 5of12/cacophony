using UnityEngine;
using UnityEngine.Events;

namespace Cacophony {
    public struct ActionEventArgs
    {
        public Vector3 position;
    }

    public abstract class IActionProcessor : ScriptableObject
    {
        [HideInInspector]
        public UnityEvent<ActionEventArgs> OnStart;
        [HideInInspector]
        public UnityEvent<ActionEventArgs> OnHold;
        [HideInInspector]
        public UnityEvent<ActionEventArgs> OnEnd;
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
