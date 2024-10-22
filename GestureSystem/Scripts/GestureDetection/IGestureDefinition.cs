using System.Collections.Generic;
using UnityEngine;

namespace Cacophony
{

    public abstract class IGestureDefinition<TP, TI> : ScriptableObject
    {
        public List<TP> positivePoses;
        public List<TP> negativePoses;

        public abstract float Evaluate(TI input); 
    }

}