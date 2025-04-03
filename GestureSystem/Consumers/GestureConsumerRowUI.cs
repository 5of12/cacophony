using System.Collections;
using Cacophony;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GestureConsumerRowUI : MonoBehaviour
{
    public string gestureName;
    public HandGestureManager manager;
    public Color idleColor = Color.gray;
    public Color holdingColor = Color.blue;
    public Color successColor = Color.green;
    public Color cancelledColor = Color.red;
    public TMP_Text nametext;
    public TMP_Text statetext;
    public Toggle gestureEnableToggle;
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
        gestureName = name;
        nametext.text = name;
        statetext.color = idleColor;
        gestureEnableToggle.onValueChanged.AddListener(HandleGestureToggled);
        resetRoutine = Reset(); 
    }

    private void HandleGestureToggled(bool isOn)
    {
        if (manager != null){
            manager.enabled = isOn;
        }
    }

    void OnDisable()
    {
        manager.actionProcessor.OnStart.RemoveListener ( HandleStart );    
        manager.actionProcessor.OnHold.RemoveListener ( HandleHold );    
        manager.actionProcessor.OnEnd.RemoveListener ( HandleEnd );    
        manager.actionProcessor.OnCancel.RemoveListener ( HandleCancel );    
    }

    private void HandleStart(ActionEventArgs pos) 
    {
        statetext.text = "Active";
        statetext.color = holdingColor;
        StopCoroutine(resetRoutine);
    }
    
    private void HandleHold(ActionEventArgs pos) 
    {
        statetext.text = "Active";
        statetext.color = holdingColor;
    }

    private void HandleEnd(ActionEventArgs pos) 
    {
        statetext.text = "Complete";
        statetext.color = successColor;

        StopCoroutine(resetRoutine);
        resetRoutine = Reset();
        StartCoroutine(resetRoutine);
    }
    private void HandleCancel(ActionEventArgs pos) 
    {
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
