using System.Collections;
using System.Collections.Generic;
using Cacophony;
using UnityEngine;
using TMPro;
using UnityEditor.PackageManager.Requests;

public class PrototypeConsumer : MonoBehaviour
{
    public HandGestureManager manager;
    public Color idleColor = Color.gray;
    public Color holdingColor = Color.blue;
    public Color successColor = Color.green;
    public Color cancelledColor = Color.red;
    public TMP_Text nametext;
    public TMP_Text statetext;
    private IEnumerator resetRoutine;

    void OnEnable()
    {
        if (manager != null)
        {
            Initialise(gameObject.name);
        }
    }

    public void Initialise(string name)
    {
        manager.actionProcessor.OnStart.AddListener ( HandleStart );    
        manager.actionProcessor.OnHold.AddListener ( HandleHold );    
        manager.actionProcessor.OnEnd.AddListener ( HandleEnd );    
        manager.actionProcessor.OnCancel.AddListener ( HandleCancel );    
        
        nametext.text = name;
        statetext.color = idleColor;
        resetRoutine = Reset(); 
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
        // Debug.Log("Action Start"); 
        statetext.text = "Active";
        statetext.color = holdingColor;
        StopCoroutine(resetRoutine);
    }
    
    private void HandleHold() 
    { 
        // Debug.Log("Action Hold"); 
        statetext.text = "Active";
        statetext.color = holdingColor;
    }

    private void HandleEnd() 
    { 
        // Debug.Log("Action End"); 
        statetext.text = "Complete";
        statetext.color = successColor;

        StopCoroutine(resetRoutine);
        resetRoutine = Reset();
        StartCoroutine(resetRoutine);
    }
    private void HandleCancel() 
    { 
        // Debug.Log("Action Cancel"); 
        statetext.text = "Cancel";
        statetext.color = cancelledColor;
        
        StopCoroutine(resetRoutine);
        resetRoutine = Reset();
        StartCoroutine(resetRoutine);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.5f);
        statetext.text = "Idle";
        statetext.color = idleColor;
    }
}
