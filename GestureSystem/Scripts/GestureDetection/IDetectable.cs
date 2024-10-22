using UnityEngine;

namespace Cacophony
{

    public abstract class IDetectable<T> : ScriptableObject
    {

        public abstract float Evaluate(T input);
    }

}