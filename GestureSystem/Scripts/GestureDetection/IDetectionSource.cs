using UnityEngine;
using UnityEngine.Events;

namespace Cacophony
{
    public abstract class IDetectionSource : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnStart;
        [HideInInspector]
        public UnityEvent OnHold;
        [HideInInspector]
        public UnityEvent OnEnd;
        [HideInInspector]
        public UnityEvent OnCancel;
        public IDetectionSource()
        {
            if (OnStart == null) OnStart = new();
            if (OnHold == null) OnHold = new();
            if (OnEnd == null) OnEnd = new();
            if (OnCancel == null) OnCancel = new();
        }
    }
}