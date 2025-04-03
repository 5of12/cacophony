using UnityEngine;
using UnityEngine.Events;

namespace Cacophony {

    public enum ActionEventType { START, INPROGRESS, COMPLETE, CANCEL };
    public struct ActionEventArgs
    {
        public ActionEventType eventType;
        public Vector3 position;
        public float progress;
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
        public UnityEvent<ActionEventArgs> OnCancel;

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
