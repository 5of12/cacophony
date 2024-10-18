using System.Collections;
using System.Collections.Generic;
using Cacophony;
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

        public abstract void Initialise(IDetectionSource detector);
        public abstract void Evaluate(Vector3 position);
        
    }
}
