using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class IGestureDetector<T> : MonoBehaviour
{
    public UnityEvent OnStart;
    public UnityEvent OnHold;
    public UnityEvent OnEnd;
    public UnityEvent OnCancel;

    public abstract void Evaluate(T input);
}
