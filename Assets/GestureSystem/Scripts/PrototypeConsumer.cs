using System.Collections;
using System.Collections.Generic;
using Cacophony;
using UnityEngine;

public class PrototypeConsumer : MonoBehaviour
{
    public HandGestureManager manager;
    public Color idleColor = Color.gray;
    public Color holdingColor = Color.blue;
    public Color successColor = Color.green;
    public Color cancelledColor = Color.red;
    private Material material;


    void OnEnable()
    {
        manager.actionProcessor.OnStart.AddListener ( HandleStart );    
        manager.actionProcessor.OnHold.AddListener ( HandleHold );    
        manager.actionProcessor.OnEnd.AddListener ( HandleEnd );    
        manager.actionProcessor.OnCancel.AddListener ( HandleCancel );    
        material = GetComponent<MeshRenderer>().material;
        material.color = idleColor;
    }

    void OnDisable()
    {
        manager.actionProcessor.OnStart.RemoveListener ( HandleStart );    
        manager.actionProcessor.OnHold.RemoveListener ( HandleHold );    
        manager.actionProcessor.OnEnd.RemoveListener ( HandleEnd );    
        manager.actionProcessor.OnCancel.RemoveListener ( HandleCancel );    
    }

    private void HandleStart() 
    { 
        Debug.Log("Action Start"); 
        material.color = holdingColor;
    }
    
    private void HandleHold() 
    { 
        // Debug.Log("Action Hold"); 
        material.color = holdingColor;
    }

    private void HandleEnd() 
    { 
        Debug.Log("Action End"); 
        material.color = successColor;
    }
    private void HandleCancel() 
    { 
        Debug.Log("Action Cancel"); 
        material.color = cancelledColor;
    }
}
