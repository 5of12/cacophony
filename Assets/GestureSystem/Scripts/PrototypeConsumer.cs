using System.Collections;
using System.Collections.Generic;
using Cacophony;
using UnityEngine;

public class PrototypeConsumer : MonoBehaviour
{
    public GestureManager manager;

    void OnEnable()
    {
        manager.actionProcessor.OnStart.AddListener ( HandleStart );    
        manager.actionProcessor.OnHold.AddListener ( HandleHold );    
        manager.actionProcessor.OnEnd.AddListener ( HandleEnd );    
        manager.actionProcessor.OnCancel.AddListener ( HandleCancel );    
    }

    void OnDisable()
    {
        manager.actionProcessor.OnStart.RemoveListener ( HandleStart );    
        manager.actionProcessor.OnHold.RemoveListener ( HandleHold );    
        manager.actionProcessor.OnEnd.RemoveListener ( HandleEnd );    
        manager.actionProcessor.OnCancel.RemoveListener ( HandleCancel );    
    }

    private void HandleStart() { Debug.Log("Action Start"); }
    private void HandleHold() { Debug.Log("Action Hold"); }
    private void HandleEnd() { Debug.Log("Action End"); }
    private void HandleCancel() { Debug.Log("Action Cancel"); }
}
